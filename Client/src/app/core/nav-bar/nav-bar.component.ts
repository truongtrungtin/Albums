import { ChangeDetectorRef, Component } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';
import { BasketService } from 'src/app/basket/basket.service';
import { Basket } from 'src/app/shared/models/basket';
import { User } from 'src/app/shared/models/user';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent {
  basket$?: Observable<Basket | null>;
  currentUser$?: Observable<User| null>;
  constructor(
    public basketService: BasketService, 
    public accountService: AccountService,
    private cdr: ChangeDetectorRef,
    private router: Router,
    ) { }

    ngOnInit(): void{
      this.basket$ = this.basketService.basketSubject$;
      this.currentUser$ = this.accountService.userSource$
    }

    Logout(){
      this.accountService.logout();
      this.router.navigate(['/']); 
    }

    triggerChangeDetection(){
      this.cdr.detectChanges();
    }

    
}
