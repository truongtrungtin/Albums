// registration.component.ts
import { Component } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { AccountService } from '../account.service';
import { LoadingService } from 'src/app/core/services/loading.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, catchError, debounceTime, map, of, switchMap } from 'rxjs';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
  registrationForm: FormGroup;
  loading: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private accountService: AccountService,
    private loadingService: LoadingService,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.registrationForm = this.formBuilder.group({
      email: ['', {
        validators: [Validators.required, Validators.email],
        asyncValidators: [this.emailAsyncValidator.bind(this)],
        updateOn: 'blur' // Trigger the async validation on blur
      }],
      password: ['', [Validators.required, Validators.minLength(6), this.passwordValidator]],
      confirmPassword: ['', [Validators.required]],
      displayName: ['', [Validators.required]],
    }, { validators: this.passwordMatchValidator });
  }

  onSubmit(): void {
    if (this.registrationForm.valid) {
      this.loadingService.loading();

      this.accountService.register(this.registrationForm.value).subscribe({
        next: () => {
          // success callback
          this.loadingService.idle();
          this.toastr.success('Registration Successful');
          this.router.navigateByUrl('/account/login');
        },
        error: (error) => {
          // error callback
          this.loadingService.idle();
          this.toastr.error(error.message, 'Registration Failed');
        }
      });
    }
  }

  // Custom validator to check if password and confirmPassword match
  passwordMatchValidator(group: FormGroup): null | { passwordMismatch: true } {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;

    return password === confirmPassword ? null : { passwordMismatch: true };
  }

  passwordValidator(control: AbstractControl): { [key: string]: boolean } | null {
    const value: string = control.value || '';
    const hasNonAlphanumeric = /[^\w\d]/.test(value);
    const hasUppercase = /[A-Z]/.test(value);

    if (!hasNonAlphanumeric || !hasUppercase) {
      return { 'passwordRequirements': true };
    }

    return null;
  }

  // Asynchronous validator to check if email is available
  emailAsyncValidator(control: AbstractControl): Promise<ValidationErrors | null> {
    const email = control.value;
  
    return new Promise((resolve) => {
      this.accountService.checkEmail(email).subscribe({
        next: (emailExists) => {
          if (emailExists) {
            resolve({ 'emailTaken': true });
          } else {
            resolve(null);
          }
        },
        error: () => {
          resolve(null); // Assume email availability on error
        }
      });
    });
  }
  
}
