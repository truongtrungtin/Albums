import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../shared/models/Product';
import { Pagination } from '../shared/models/Pagination';
import { StoreParams } from '../shared/models/storeParams';
import { ApiResponse } from '../shared/models/ApiResponse';
import { CreateProduct } from '../shared/models/createProduct';
import { FileDetails, fileToJson } from '../shared/models/fileJson';
import { Type } from '../shared/models/type';

@Injectable({
  providedIn: 'root',
})
export class StoreService {
  // private apiUrl = 'https://api-albums.ddns.net/api/v1';
  private apiUrl = 'https://localhost:7272/api/v1';
  // private apiUrl = 'http://localhost:5193/api/v1';
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
  getTypes(): Observable<Type[]> {
    const url = `${this.apiUrl}/Products/Types`;
    return this.http.get<Type[]>(url);
  }

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

  uploadToServer(createProduct: CreateProduct, file: File): Observable<ApiResponse<Pagination<Product>>> {
    const headers = this.getHeaderAuthorization();
    console.log(    JSON.stringify(file))
    return this.http.post<any>(`${this.apiUrl}/Products/`, createProduct, {
      headers,
    });
  }

}
