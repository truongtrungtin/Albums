import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot} from '@angular/router';
import { AccountService } from 'src/app/account/account.service';

export const canActivate: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
) => {
  const accountService = inject(AccountService);
  const router = inject(Router);

  if (accountService.isAuthenticated()) {
    return true;
  } else {
    // Store the attempted URL for redirecting
    accountService.redirectUrl = state.url;
    // Redirect to the login page with the return URL
    return router.createUrlTree(['/account/login'], {
      queryParams: { returnUrl: state.url }
    });
  }
};


