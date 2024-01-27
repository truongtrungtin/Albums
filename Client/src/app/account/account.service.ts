import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { User } from '../shared/models/user';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, switchMap } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr'; // Import ToastrService

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  // private apiUrl = 'https://albums-tt.ddns.net/api/Account/';
  private apiUrl = 'https://localhost:7272/api/Account/';

  private userSource = new BehaviorSubject<User | null>(null);
  userSource$ = this.userSource.asObservable();
  redirectUrl: string | null = null;

  constructor(private http: HttpClient, private toastr: ToastrService) {}

  // Method to perform user login
  login(loginModel: any): Observable<User> {
    return this.http.post<any>(`${this.apiUrl}login`, loginModel).pipe(
      map((response) => {
        this.setUser(response.data);
        this.toastr.success('Login successful');
        return response.data;
      }),
      catchError((error) => {
        console.log(error);
        // Handle registration error
        this.toastr.error(error.message);
        return throwError(() => new Error(error.message));

      })
    );
  }

  // Method to perform user registration with email check
  register(registerModel: any): Observable<User> {
    // Check if the email already exists
    return this.checkEmail(registerModel.email).pipe(
      switchMap((emailExists) => {
        if (emailExists) {
          // Email already exists, return an error Observable
          this.toastr.error('Email already exists');
          return throwError(() => new Error('Email already exists'));
        } else {
          // Email does not exist, proceed with registration
          return this.http.post<any>(`${this.apiUrl}register`, registerModel).pipe(
            map((response) => {
              this.setUser(response.data);
              this.toastr.success('Registration successful');
              return response.data;
            }),
            catchError((error) => {
              // Handle registration error
              this.toastr.error('Registration failed');
              return throwError(() => new Error('Registration failed'));
            })
          );
        }
      })
    );
  }

  // Method to check if an email exists
  checkEmail(email: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}check-email?email=${email}`).pipe(
      map((response) => {
        return response;
      })
    );
  }

  // Method to set the current user and token in the service and localStorage
  setUser(user: User | null): void {
    if (user) {
      localStorage.setItem('token', user.token);
      this.userSource.next(user);
    } else {
      localStorage.removeItem('token');
    }
  }

  // Method to handle logout
  logout(): void {
    // Implement your logout logic here
    // Example: Clear user data, navigate to login page, etc.
    this.setUser(null); // Notify about user changes
  }

  // Method to get user details from localStorage
  

  loadUser(token: string){
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
    return this.http.get<any>(`${this.apiUrl}`, {headers}).pipe(
      map((response) => {
        this.setUser(response.data);
        return response.data;
      }),
      catchError((error) => {
        this.toastr.error('Error loading user',error);
  
        // Rethrow the error to propagate it to the caller
        return throwError(() => error);
      })
    );
  }

  isAuthenticated(): boolean {
    // Implement your authentication logic here
    // For example, check if the user is logged in based on your token logic
    const token = localStorage.getItem('token');
    return !!token; // Returns true if token exists, indicating the user is authenticated
  }

 
}
