import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Pagination } from '../shared/models/Pagination';
import { StoreParams } from '../shared/models/storeParams';
import { ApiResponse } from '../shared/models/ApiResponse';
import { CreateProduct } from '../shared/models/createProduct';
import { Catalog } from '../shared/models/Catalog';
import { FileAttachment } from '../shared/models/FileAttachment';
import { Profile } from '../shared/models/Profile';
import { CreateProfile } from '../shared/models/createProfile';

@Injectable({
  providedIn: 'root',
})
export class StoreService {
  private apiUrl = 'https://api-albums.ddns.net/api/v1';
  // private apiUrl = 'https://localhost:7272/api/v1';
  // private apiUrl = 'http://localhost:5193/api/v1';
  // Inject HttpClient for making HTTP requests
  constructor(private http: HttpClient) { }
  // Fetches products with optional filtering, sorting, and pagination


  getFileAttachments(
    storeParams: StoreParams
  ): Observable<ApiResponse<Pagination<FileAttachment>>> {
    // Create HttpParams for request
    let params = this.createHttpParams({
      sort: storeParams.sort,
      page: storeParams.page,
      pageSize: storeParams.pageSize,
      fileExtention: storeParams.selectedFileExtention?.catalogCode,
      fileType: storeParams.selectedFileType?.catalogCode,
      profile: storeParams.selectedProfile?.profileId,
      search: storeParams.search,
    }); // Construct the URL for the request
    const url = `${this.apiUrl}/FileAttachment`;
    const headers = this.getHeaderAuthorization();

    // Perform the HTTP GET request
    return this.http.get<ApiResponse<Pagination<FileAttachment>>>(`${url}`, {
      params,
      headers,
    });
  }

  getFileAttachment(id: string) {
    const url = `${this.apiUrl}/FileAttachment/` + id;
    const headers = this.getHeaderAuthorization();

    return this.http.get<FileAttachment>(url, {
      headers
    });
  }

  getProfiles(): Observable<Profile[]> {
    const url = `${this.apiUrl}/Profile`;
    const headers = this.getHeaderAuthorization();

    return this.http.get<Profile[]>(url, {
      headers
    });
  }

  getFileExtentions(): Observable<Catalog[]> {
    const url = `${this.apiUrl}/FileAttachment/fileextentions`;
    const headers = this.getHeaderAuthorization();

    return this.http.get<Catalog[]>(url, {
      headers
    });
  }

  getFileTypes(): Observable<Catalog[]> {
    const url = `${this.apiUrl}/FileAttachment/filetypes`;
    const headers = this.getHeaderAuthorization();

    return this.http.get<Catalog[]>(url, {
      headers
    });
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


  uploadProfileToServer(profile: CreateProfile): Observable<Profile> {
    const formData = new FormData();

    // Append product properties
    formData.append('profileName', profile.profileName);
    formData.append('avatar', profile.avatar, profile.avatar.name);

    const headers = this.getHeaderAuthorization();

    return this.http.post<any>(`${this.apiUrl}/Profile`, formData, {
      headers,
    });
  }


  uploadToServer(createProduct: CreateProduct): Observable<ApiResponse<Pagination<FileAttachment>>> {
    const formData = new FormData();
    formData.append('profileId', createProduct.profileId);
    formData.append('file', createProduct.file);

    if (createProduct.latitude && createProduct.longitude) {
      formData.append('latitude', createProduct.latitude);
      formData.append('longitude', createProduct.longitude);
    }
    const headers = this.getHeaderAuthorization();

    return this.http.post<any>(`${this.apiUrl}/FileAttachment/`, formData, { headers });
  }


}
