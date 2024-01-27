// basket.component.ts
import { Component, OnInit } from '@angular/core';
import { Basket, BasketItem } from '../shared/models/basket';
import { BasketService } from './basket.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss']
})
export class BasketComponent implements OnInit {
  basket: Basket | null = new Basket();
  constructor(public basketService: BasketService) {}
 
  ngOnInit(): void {
    this.basketService.basketSubject$.subscribe((basket) => {
      this.basket = basket;
    });
  }

  extractImageName(item: BasketItem): string | null {
    if(item && item.pictureUrl){
      const parts = item.pictureUrl.split('/');
      if(parts.length > 0){
        return parts[parts.length - 1];
      }
    }
    return null;
  }


  decrementQuantity(item: BasketItem): void {
    this.basketService.decrementQuantity(item)
  }

  incrementQuantity(item: BasketItem): void {
    this.basketService.incrementQuantity(item)
  }

  removeItem(item: BasketItem): void {
    this.basketService.removeItem(item)
  }


}
