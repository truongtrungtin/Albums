// login.component.ts
import { Component } from '@angular/core';
import { AccountService } from '../account.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoadingService } from 'src/app/core/services/loading.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  loginForm: FormGroup;
  loading: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private accountService: AccountService,
    private loadingService: LoadingService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
      rememberMe: [false],
    });

    this.route.queryParams.subscribe(params => {
      this.accountService.redirectUrl = params['returnUrl' || '/']
    })
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      this.loadingService.loading();
  
      this.accountService.login(this.loginForm.value).subscribe({
        next: () => {
          this.loadingService.idle();
  
          // Get the returnUrl from the service and navigate accordingly
          const returnUrl = this.accountService.redirectUrl ? this.accountService.redirectUrl : '/store';
          this.router.navigateByUrl(returnUrl);
          this.accountService.redirectUrl = null;
        },
        error: () => {
          this.loadingService.idle();
        }
      });
    }
  }
}
