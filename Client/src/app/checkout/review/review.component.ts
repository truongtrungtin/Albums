// review.component.ts
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';
import { Basket, BasketItem } from 'src/app/shared/models/basket';

@Component({
  selector: 'app-review',
  templateUrl: './review.component.html',
  styleUrls: ['./review.component.scss'],
})
export class ReviewComponent implements OnInit {
  constructor(public basketService: BasketService, private router: Router, private toastr: ToastrService) {}

  ngOnInit(): void {
    
  }
  extractImageName(item: BasketItem): string | null {
    if (item && item.pictureUrl) {
      const parts = item.pictureUrl.split('/');
      if (parts.length > 0) {
        return parts[parts.length - 1];
      }
    }
    return null;
  }


  submitOrder(): void{
    this.router.navigate(['/store']);
    this.basketService.clearBasket();
    this.toastr.success('You have successfully placed your order!');
  }
}
