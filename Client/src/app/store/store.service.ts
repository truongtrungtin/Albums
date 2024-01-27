import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../shared/models/Product';
import { Pagination } from '../shared/models/Pagination';
import { StoreParams } from '../shared/models/storeParams';
import { ApiResponse } from '../shared/models/ApiResponse';

@Injectable({
  providedIn: 'root',
})
export class StoreService {
  private apiUrl = 'https://api-albums.ddns.net/api/v1';
  // private apiUrl = 'https://localhost:7272/api/v1';

  // Inject HttpClient for making HTTP requests
  constructor(private http: HttpClient) { }
  // Fetches products with optional filtering, sorting, and pagination
  getProducts(
    storeParams: StoreParams
  ): Observable<ApiResponse<Pagination<Product>>> {
    // Create HttpParams for request
    let params = this.createHttpParams({
      sort: storeParams.sort,
      page: storeParams.page,
      pageSize: storeParams.pageSize,
      productBrandId: storeParams.selectedBrand?.id,
      productTypeId: storeParams.selectedType?.id,
      search: storeParams.search,
    }); // Construct the URL for the request
    const url = `${this.apiUrl}/Products`;
    const headers = this.getHeaderAuthorization();

    // Perform the HTTP GET request
    return this.http.get<ApiResponse<Pagination<Product>>>(`${url}`, {
      params,
      headers,
    });
  }

  getProduct(id: number) {
    const url = `${this.apiUrl}/Products/` + id;
    return this.http.get<Product>(url);
  }

  // Fetches all available brands
  // getBrands(): Observable<Brand[]> {
  //   const url = `${this.apiUrl}/Products/Brands`;
  //   return this.http.get<Brand[]>(url);
  // }

  // // Fetches all available types
  // getTypes(): Observable<Type[]> {
  //   const url = `${this.apiUrl}/Products/Types`;
  //   return this.http.get<Type[]>(url);
  // }

  // Helper method to create HttpParams from an object
  private createHttpParams(options: any): HttpParams {
    let params = new HttpParams();
    Object.keys(options).forEach((key) => {
      // Add parameter to HttpParams if it has a truthy value
      if (options[key]) {
        params = params.set(key, options[key].toString());
      }
    });
    return params;
  }

  getHeaderAuthorization() {
    let token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return headers;
  }

  getBase64(file: File) {
    var reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function () {
      console.log(reader.result);
    };
    reader.onerror = function (error) {
      console.log('Error: ', error);
    };
  }

  uploadToServer(files: File[]): Observable<ApiResponse<Pagination<Product>>> {
    const formData = new FormData();

    // Append each file to the FormData
    for (let i = 0; i < files.length; i++) {
      formData.append('files', files[i], files[i].name);
    }

    // Make the HTTP POST request to the CreateProduct API endpoint
    // Include headers in the request options
    const headers = this.getHeaderAuthorization();
    const options = { headers };

    return this.http.post<any>(`${this.apiUrl}/Products/`, formData, {
      headers,
    });
  }
}
