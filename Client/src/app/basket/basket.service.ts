import { Injectable } from '@angular/core';
import { BehaviorSubject} from 'rxjs';
import { Basket, BasketItem, BasketTotal } from '../shared/models/basket';
import { HttpClient } from '@angular/common/http';
import { Product } from '../shared/models/Product';
import { ToastrService } from 'ngx-toastr';
import { deliveryOption } from '../shared/models/deliveryOption';

@Injectable({
  providedIn: 'root',
})
export class BasketService {
  selectedDeliveryOption: deliveryOption | undefined;
  private basketSubject: BehaviorSubject<Basket | null> =
    new BehaviorSubject<Basket | null>(null);
  basketSubject$ = this.basketSubject.asObservable();
  private basketTotalSubject =
    new BehaviorSubject<BasketTotal>({
      subtotal: 0,
      shipping: 0,
      total: 0,
    });
  basketTotalSubject$ = this.basketTotalSubject.asObservable();
  private readonly basketUrl = 'https://localhost:7272/api/Basket/';

  constructor(private http: HttpClient, private toastr: ToastrService) {}

  getBasket(basketId: string) {
    return this.http.get<Basket>(this.basketUrl + basketId).subscribe({
      next: (basket) => {
        this.basketSubject.next(basket);
        this.calculateShippingAndTotal();
      },
    });
  }

  setBasket(basket: Basket | null) {
    return this.http.post<Basket>(this.basketUrl, basket).subscribe({
      next: (basket) => {
        this.basketSubject.next(basket);
        this.calculateShippingAndTotal();
      },
    });
  }

  getBasketSubjectCurrentValue() {
    return this.basketSubject.value;
  }

  updateShippingPrice(price: number) {
    const updateBasketTotal = this.basketTotalSubject.value;
    updateBasketTotal.shipping = price;
    updateBasketTotal.total = updateBasketTotal?.subtotal + price;
    this.basketTotalSubject.next(updateBasketTotal)
  }

  calculateShippingAndTotal(): void {
    const basket = this.getBasketSubjectCurrentValue();
    const selectedDeliveryOption = this.selectedDeliveryOption;

    const shipping = selectedDeliveryOption ? selectedDeliveryOption.price : 0;
    const subtotal = this.calculateSubtotal();
    const total = subtotal + shipping;
    this.basketTotalSubject.next({ shipping, subtotal, total });
  }

  calculateSubtotal(): number{
    const basket = this.getBasketSubjectCurrentValue();
    let subtotal = 0;

    if(basket){
      subtotal = basket.items.reduce(
        (total, item) => item.price * item.quantity + total,
        0
      );
    }
    return subtotal;
  }

  addItemToBasket(item: Product, quantity = 1) {
    const cartItem = this.mapProductToBasket(item);
    const basket = this.getBasketSubjectCurrentValue() ?? this.createBasket();
    basket.items = this.upsertItem(basket.items, cartItem, quantity);

    this.setBasket(basket);
    this.toastr.success('Item added to cart!');
  }
  private upsertItem(
    items: BasketItem[],
    basketItem: BasketItem,
    quantity: number
  ): BasketItem[] {
    const item = items.find((p) => p.id === basketItem.id);
    if (item) {
      item.quantity += quantity;
    } else {
      basketItem.quantity = quantity;
      items.push(basketItem);
    }
    return items;
  }
  private mapProductToBasket(item: Product): BasketItem {
    const basketItem: BasketItem = {
      id: item.productId,
      productName: item.name,
      price: item.price,
      quantity: 0,
      pictureUrl: item.pictureUrl,
      productBrand: item.productBrand,
      productType: item.productType,
    };
    return basketItem;
  }

  private createBasket(): Basket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  decrementQuantity(item: BasketItem): void {
    if (item.quantity > 1) {
      item.quantity--;
      this.updateBasket();
    }
    if(item.quantity === 0){
      this.clearBasket();
    }
  }

  incrementQuantity(item: BasketItem): void {
    item.quantity++;
    this.updateBasket();
  }

  removeItem(item: BasketItem): void {
    const basket = this.getBasketSubjectCurrentValue();
    if (basket) {
      const itemIndex = basket.items.findIndex((p) => p.id === item.id);
      if (itemIndex !== -1) {
        basket.items.splice(itemIndex, 1);
        this.updateBasket();
        this.toastr.success('Item removed to cart!');
      }
      if(basket.items.length === 0){
        this.clearBasket();
      }
    }
  }


  clearBasket(): void{
    this.basketSubject.next(null);
    localStorage.removeItem('basket_id');
  }

  private updateBasket(): void {
    const currentBasket = this.getBasketSubjectCurrentValue();
    this.setBasket(currentBasket);
  }
}
