import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { StoreService } from './store.service';
import { Brand } from '../shared/models/brand';
import { Type } from '../shared/models/type';
import { StoreParams } from '../shared/models/storeParams';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';
import { ToastrService } from 'ngx-toastr';
import { ProductUploadComponent } from './product-upload/product-upload.component';
import { HttpClient } from '@angular/common/http';
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
    ) {}

  ngOnInit() {
    this.reloadData();
  }

  // Method to reload data on component initialization
  reloadData() {
    this.loadBrandsAndTypes();
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
        },
        error: (error) => this.handleError(error)
      });
      this.toastr.success('Products loaded');
  }

  onSubmitFormUpload() {
    this.uploadFiles(this.productUploadComponent.selectedFile);
  }

  uploadFiles(files: File[]) {
    this.storeService.uploadToServer(files).subscribe({
      next: (response) => {
        // Handle the success response
        console.log(response.code)
        if(response.isSuccess){
          this.toastr.success('Upload successful');
          this.productUploadComponent.resetFileInput(); 
          this.loadProducts();
          this.closebutton.nativeElement.click();

        }
      },
      error: (error) => {
        // Handle the error response
        if (error.error && error.error.errors) {
          // Handle validation errors
          const validationErrors = error.error.errors;
          this.toastr.success('Upload failed',validationErrors);
          // Display or handle validation errors as needed
        } else {
          this.toastr.success('Upload failed');
          // Handle other types of errors
        }
      },
    });
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
    this.params.selectedType = {id:0, name:"All"};
    this.params.selectedBrand = {id:0, name:"All"};
    this.params.sort = 'NameAsc';
    this.loadProducts();
  }

  // Applies selected sorting option
  applySort() {
    this.loadProducts();
  }

  // Generic error handler for HTTP requests
  private handleError(error: any) {
    console.error('An error occurred:', error);
  }


}
