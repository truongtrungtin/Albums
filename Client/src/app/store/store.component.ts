import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { StoreService } from './store.service';
import { StoreParams } from '../shared/models/storeParams';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';
import { ToastrService } from 'ngx-toastr';
import { ProductUploadComponent } from './product-upload/product-upload.component';
import * as exifr from 'exifr';
import { LocationImage } from '../shared/models/locationImage';
import { CreateProduct } from '../shared/models/createProduct';
import { LoadingService } from '../core/services/loading.service';
import { Catalog } from '../shared/models/Catalog';
import { Profile } from '../shared/models/Profile';
import { ProductProfilesComponent } from './product-profiles/product-profiles.component';
import { CreateProfile } from '../shared/models/createProfile';

@Component({
  selector: 'app-store',
  templateUrl: './store.component.html',
  styleUrls: ['./store.component.scss']
})
export class StoreComponent implements OnInit {
  @Input() title: string = '';
  @ViewChild(ProductUploadComponent) productUploadComponent!: ProductUploadComponent;
  @ViewChild(ProductProfilesComponent) productProfilesComponent!: ProductProfilesComponent;

  @ViewChild('closebuttonupload') closebuttonupload: any;
  @ViewChild('closebuttoncreateprofile') closebuttoncreateprofile: any;

  // Store-related data
  params: StoreParams = new StoreParams();

  // Injecting StoreService for data fetching
  constructor(
    private storeService: StoreService,
    private toastr: ToastrService,
    private loadingService: LoadingService,

  ) { }

  ngOnInit() {
    this.reloadData();
  }

  // Method to reload data on component initialization
  reloadData() {
    this.loadExtentionsAndTypes();
    this.loadFileAttachments();
  }

  //#region fileAttachments
  // Fetches products from the store service
  loadFileAttachments() {
    this.storeService.getFileAttachments(this.params)
      .subscribe({
        next: (response) => {
          const data = response.data;
          this.params.fileAttachments = data.data;
          this.params.page = data.pageIndex;
          this.params.totalPages = data.totalPages;
          this.params.totalItems = data.totalItems;
          this.toastr.success('Products loaded');
        },
        error: (error) => {
          this.handleError(error)
        }
      });
  }

  async onSubmitFormUpload() {
    const files = (this.productUploadComponent.selectedFile as File[]) || [];
    if (files.length > 0) {
      for (var i = 0; i < files.length; i++) {
        const product: CreateProduct = {
          name: files[i].name,
          locationImage: await this.onFileSelected(files[i]),
          productType: files[i].type,
          file: files[i],
        };
        this.uploadFiles(product);
      }
    } else {
      this.handleError('Invalid files variable: not an array')
    }
  }
  uploadFiles(files: CreateProduct) {
    this.storeService.uploadToServer(files).subscribe({
      next: (response) => {
        // Handle the success response
        if (response.isSuccess) {
          this.toastr.success('Upload successful');
          this.productUploadComponent.resetFileInput();
          this.loadFileAttachments();
          this.closebuttonupload.nativeElement.click();
          this.productUploadComponent.resetFileInput();
        } else {
          this.toastr.error(response.message);
          // this.handleError(error)
        }
      },
      error: (error) => {
        console.log(error)
        if (error.error && error.error.errors) {
          const validationErrors = error.error.errors;
          this.handleError(validationErrors)
        } else {
          this.handleError(error)
        }
      },
    });
  }


  //#endregion

  //#region profile

  async onSubmitFormCreateProfile() {
    const displayName = (document.getElementById('profileName') as HTMLInputElement)?.value;
    const files = (this.productProfilesComponent.selectedFile as File[]) || [];
    if (files.length > 0) {
      for (var i = 0; i < files.length; i++) {
        const profile: CreateProfile = {
          profileName: displayName,
          avatar: files[i],
        };
        this.uploadProfile(profile);
      }
    } else {
      this.handleError('Please provide a display name and select an avatar file.');
    }
  }
  uploadProfile(files: CreateProfile) {
    this.storeService.uploadProfileToServer(files).subscribe({
      next: (response) => {
        // Handle the success response
        this.toastr.success('Upload successful');
        this.loadExtentionsAndTypes();
        this.closebuttoncreateprofile.nativeElement.click();
        this.productProfilesComponent.resetFileInput();
      },
      error: (error) => {
        console.log(error)
        if (error.error && error.error.errors) {
          const validationErrors = error.error.errors;
          this.handleError(validationErrors)
        } else {
          this.handleError(error)
        }
      },
    });
  }

  //#endregion



  onFileSelected(event: any): Promise<LocationImage> {
    return new Promise((resolve, reject) => {
      const file = event;
      const reader = new FileReader();

      reader.onload = async (e: any) => {
        const blob = new Blob([e.target.result], { type: file.type });
        const locationImage = await this.extractGpsData(blob);
        resolve(locationImage);
      };

      reader.readAsArrayBuffer(file);
    });
  }

  async extractGpsData(blob: Blob): Promise<LocationImage> {
    try {
      const locationImage: LocationImage = new LocationImage();
      const gpsData = await exifr.gps(blob);

      if (gpsData) {
        const latitude = gpsData.latitude;
        const longitude = gpsData.longitude;
        locationImage.Latitude = latitude;
        locationImage.Longitude = longitude;
        return locationImage;
      } else {
        console.log('No GPS data found in the image.');
        return locationImage;
      }
    } catch (error) {
      throw this.handleError(error);
    }
  }



  loadExtentionsAndTypes() {
    this.storeService.getFileExtentions().subscribe({
      next: (extentions) => this.params.fileExtentions = [{ catalogCode: "", catalogText_vi: "All" }, ...extentions],
      error: (error) => this.handleError(error)
    });
    this.storeService.getFileTypes().subscribe({
      next: (types) => this.params.fileTypes = [{ catalogCode: "", catalogText_vi: "All" }, ...types],
      error: (error) => this.handleError(error)
    });
    this.storeService.getProfiles().subscribe({
      next: (profiles) => this.params.profiles = [{ profileCode: "", profileName: "All", profileId: "", avatar: "", createBy: "" }, ...profiles],
      error: (error) => this.handleError(error)
    });
  }

  onPageChanged(event: PageChangedEvent) {
    this.params.page = event.page;
    this.loadFileAttachments();
  }

  selectExtention(extention: Catalog) {
    this.params.selectedFileExtention = extention;
    this.loadFileAttachments();
  }

  selectType(type: Catalog) {
    this.params.selectedFileType = type;
    this.loadFileAttachments();
  }

  selectProfile(profile: Profile) {
    this.params.selectedProfile = profile;
    this.loadFileAttachments();
  }

  applySearch() {
    this.loadFileAttachments();
  }

  resetSearch() {
    this.params.search = '';
    this.params.selectedFileType = { catalogCode: "", catalogText_vi: "All" };
    this.params.selectedFileExtention = { catalogCode: "", catalogText_vi: "All" };
    this.params.sort = 'NameAsc';
    this.loadFileAttachments();
  }

  applySort() {
    this.loadFileAttachments();
  }

  private handleError(error: any) {
    // console.error('An error occurred:', error);
    this.toastr.error(error.statusText);
  }
}
