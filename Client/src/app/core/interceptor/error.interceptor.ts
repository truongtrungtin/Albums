import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 404) {
          // Redirect to the Not Found route
          this.router.navigate(['/404']);
          this.toastr.error('Page not found', 'Error 404');
        } else if (error.status === 500) {
          // Redirect to the Server Error route
          this.router.navigate(['/500']);
          this.toastr.error('Internal server error', 'Error 500');
        } else if (error.status === 401) {
          // Redirect to the Server Error route
          this.router.navigate(['/401']);
          this.toastr.error('Unauthorized', 'Error 401');
        }else if (error.status === 0) {
          // Redirect to the Connection Refused route
          this.router.navigate(['/connection-error']);
          this.toastr.error('Connection refused', 'Network Error');
        } else {
          // Display a generic error message
          this.toastr.error('An unexpected error occurred', 'Error');
        }
        // For other HTTP errors, propagate the error as is
        return throwError(error);
      })
    );
  }
}
