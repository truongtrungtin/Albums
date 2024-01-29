import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { StoreService } from './store.service';
import { Brand } from '../shared/models/brand';
import { Type } from '../shared/models/type';
import { StoreParams } from '../shared/models/storeParams';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';
import { ToastrService } from 'ngx-toastr';
import { ProductUploadComponent } from './product-upload/product-upload.component';
import * as exifr from 'exifr';
import { LocationImage } from '../shared/models/locationImage';
import { Product } from '../shared/models/Product';
import { CreateProduct } from '../shared/models/createProduct';
import { FileDetails, fileToJson } from '../shared/models/fileJson';
import { LoadingService } from '../core/services/loading.service';

@Component({
  selector: 'app-store',
  templateUrl: './store.component.html',
  styleUrls: ['./store.component.scss']
})
export class StoreComponent implements OnInit {
  @Input() title: string = '';
  @ViewChild(ProductUploadComponent) productUploadComponent!: ProductUploadComponent;
  @ViewChild('closebutton') closebutton: any;

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
    // this.loadBrandsAndTypes();
    this.loadProducts();
  }

  // Fetches products from the store service
  loadProducts() {
    this.storeService.getProducts(this.params)
      .subscribe({
        next: (response) => {
          const data = response.data;
          this.params.products = data.data;
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
        this.loadingService.loading();
        var jsonDetails: FileDetails = fileToJson(files[0]);
        const base64Content = btoa(jsonDetails.content as string);
        console.log(base64Content)
        const product: CreateProduct = {
          name: files[i].name,
          locationImage: await this.onFileSelected(files[i]),
          productType: files[i].type,
          file: {
            name: jsonDetails.name,
            size: jsonDetails.size,
            type : jsonDetails.type,
            content: base64Content
          },
        };
        this.uploadFiles(product);
      }
    } else {
      this.handleError('Invalid files variable: not an array')
    }
  }

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
      // console.error('Error extracting GPS data:', error);
      throw this.handleError(error); // You might want to handle the error appropriately in your application
    }
  }
  

  uploadFiles(files: CreateProduct) {
    this.loadingService.loading();
    this.storeService.uploadToServer(files).subscribe({
      next: (response) => {
        // Handle the success response
        if (response.isSuccess) {
          this.toastr.success('Upload successful');
          this.productUploadComponent.resetFileInput();
          this.loadProducts();
          this.closebutton.nativeElement.click();
        }else{
          this.toastr.error(response.message);
          // this.handleError(error)
        }
      },
      error: (error) => {
        console.log(error)
        // Handle the error response
        if (error.error && error.error.errors) {
          // Handle validation errors
          const validationErrors = error.error.errors;
          // this.toastr.error('Upload failed', validationErrors);
          this.handleError(validationErrors)
          // Display or handle validation errors as needed
        } else {
          // this.toastr.error('Upload failed',error);
          this.handleError(error)
          // Handle other types of errors
        }
      },
    });
    this.loadingService.idle();
  }

  // Fetches brands and types from the store service
  loadBrandsAndTypes() {
    // this.storeService.getBrands().subscribe({
    //   next: (brands) => this.params.brands = [{id:0, name:"All"}, ...brands],
    //   error: (error) => this.handleError(error)
    // });
    // this.storeService.getTypes().subscribe({
    //   next: (types) => this.params.types = [{id:0, name:"All"}, ...types],
    //   error: (error) => this.handleError(error)
    // });
  }

  // Pagination handler
  onPageChanged(event: PageChangedEvent) {
    this.params.page = event.page;
    this.loadProducts();
  }

  // Handler for brand selection
  selectBrand(brand: Brand) {
    this.params.selectedBrand = brand;
    this.loadProducts();
  }

  // Handler for type selection
  selectType(type: Type) {
    this.params.selectedType = type;
    this.loadProducts();
  }

  // Applies the search term to filter products
  applySearch() {
    this.loadProducts();
  }

  // Resets search and filter criteria and reloads products
  resetSearch() {
    this.params.search = '';
    this.params.selectedType = { id: 0, name: "All" };
    this.params.selectedBrand = { id: 0, name: "All" };
    this.params.sort = 'NameAsc';
    this.loadProducts();
  }

  // Applies selected sorting option
  applySort() {
    this.loadProducts();
  }

  // Generic error handler for HTTP requests
  private handleError(error: any) {
    // console.error('An error occurred:', error);
    this.toastr.error(error.statusText);
  }
}
