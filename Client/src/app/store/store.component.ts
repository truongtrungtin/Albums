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

  constructor(
    private storeService: StoreService,
    private toastr: ToastrService,
    private loadingService: LoadingService,
  ) { }

  ngOnInit() {
    this.reloadData();
  }

  // Reloads data on component initialization
  reloadData() {
    this.loadExtentionsAndTypes();
    this.loadFileAttachments();
  }

  // Fetches file attachments from the store service
  loadFileAttachments() {
    this.storeService.getFileAttachments(this.params).subscribe({
      next: (response) => {
        const data = response.data;
        this.params.fileAttachments = data.data;
        this.params.page = data.pageIndex;
        this.params.totalPages = data.totalPages;
        this.params.totalItems = data.totalItems;
        this.toastr.success('Products loaded');
        console.log(data)
      },
      error: (error) => this.handleError(error)
    });
  }

  // Handles file upload form submission
  async onSubmitFormUpload() {
    const profileId = (document.getElementById('profileId') as HTMLInputElement)?.value;
    const files = (this.productUploadComponent.selectedFile as File[]) || [];
    if (files.length > 0) {
      for (let i = 0; i < files.length; i++) {
        const locationImage = await this.onFileSelected(files[i]);
       
        const file: CreateProduct = {
          profileId: profileId,
          file: files[i],
          latitude: null,
          longitude: null
        };
        if (locationImage) {
          file.latitude = locationImage.Latitude.toString();
          file.longitude = locationImage.Longitude.toString();
        }
        this.uploadFiles(file);
      }
    } else {
      this.handleError('Invalid files variable: not an array');
    }
  }

  // Uploads files to the server
  uploadFiles(files: CreateProduct) {
    this.storeService.uploadToServer(files).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.toastr.success('Upload successful');
          this.loadFileAttachments();
          this.productUploadComponent.resetFileInput();
          this.closebuttonupload.nativeElement.click();
        } else {
          this.toastr.error(response.message);
        }
      },
      error: (error) => {
        console.log(error);
        if (error.error && error.error.errors) {
          const validationErrors = error.error.errors;
          this.handleValidationErrors(validationErrors);
        } else {
          this.handleServerError(error);
        }
      },
    });
  }

  private handleValidationErrors(validationErrors: any) {
    // Implement logic to handle and display validation errors in your UI
    console.error('Validation Errors:', JSON.stringify(validationErrors));
    // For example, you could display validation errors in a toast or alert
    this.toastr.error('Validation Errors: ' + JSON.stringify(validationErrors));
  }

  // Handles profile creation form submission
  async onSubmitFormCreateProfile() {
    const displayName = (document.getElementById('profileName') as HTMLInputElement)?.value;
    const files = (this.productProfilesComponent.selectedFile as File[]) || [];
    if (displayName && files.length > 0) {
      for (let i = 0; i < files.length; i++) {
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

  // Uploads profile to the server
  uploadProfile(files: CreateProfile) {
    this.storeService.uploadProfileToServer(files).subscribe({
      next: (response) => {
        // Handle the success response
        this.toastr.success('Upload successful');
        this.loadExtentionsAndTypes();
        this.closebuttoncreateprofile.nativeElement.click();
        this.productProfilesComponent.resetFileInput();
      },
      error: (error) => this.handleServerError(error),
    });
  }

  


  // Extracts GPS data from the selected file
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

  // Extracts GPS data from the blob
  async extractGpsData(blob: Blob): Promise<LocationImage> {
    try {
      const locationImage: LocationImage = new LocationImage();
      const gpsData = await exifr.gps(blob);

      if (gpsData) {
        const { latitude, longitude } = gpsData;
        locationImage.Latitude = latitude;
        locationImage.Longitude = longitude;
        return locationImage;
      } else {
        console.log('No GPS data found in the image.');
        return locationImage;
      }
    } catch (error) {
      this.handleError(error);
      return new LocationImage();
    }
  }

  // Loads file extensions and types for filtering
  loadExtentionsAndTypes() {
    this.storeService.getFileExtentions().subscribe({
      next: (extentions) => this.params.fileExtentions = [{ catalogCode: "", catalogText_vi: "All" }, ...extentions],
      error: (error) => this.handleError(error),
    });
    this.storeService.getFileTypes().subscribe({
      next: (types) => this.params.fileTypes = [{ catalogCode: "", catalogText_vi: "All" }, ...types],
      error: (error) => this.handleError(error),
    });
    this.storeService.getProfiles().subscribe({
      next: (profiles) => this.params.profiles = [{ profileCode: "", profileName: "My Profile ", profileId: "", avatar: "", createBy: "" }, ...profiles],
      error: (error) => this.handleError(error),
    });
  }

  // Handles page change event
  onPageChanged(event: PageChangedEvent) {
    this.params.page = event.page;
    this.loadFileAttachments();
  }

  // Selects file extension for filtering
  selectExtention(extention: Catalog) {
    this.params.selectedFileExtention = extention;
    this.loadFileAttachments();
  }

  // Selects file type for filtering
  selectType(type: Catalog) {
    this.params.selectedFileType = type;
    this.loadFileAttachments();
  }

  // Selects profile for filtering
  selectProfile(profile: Profile) {
    this.params.selectedProfile = profile;
    this.loadFileAttachments();
  }

  // Applies search filter
  applySearch() {
    this.loadFileAttachments();
  }

  // Resets search filters
  resetSearch() {
    this.params.search = '';
    this.params.selectedFileType = { catalogCode: "", catalogText_vi: "All" };
    this.params.selectedFileExtention = { catalogCode: "", catalogText_vi: "All" };
    this.params.sort = 'NameAsc';
    this.loadFileAttachments();
  }

  // Applies sort filter
  applySort() {
    this.loadFileAttachments();
  }

  // Handles generic errors
  private handleError(error: any) {
    this.toastr.error(error.statusText);
  }

  // Handles server errors
  private handleServerError(error: any) {
    console.log(error);
    if (error.error && error.error.errors) {
      const validationErrors = error.error.errors;
      this.handleError(validationErrors);
    } else {
      this.handleError(error);
    }
  }
}
