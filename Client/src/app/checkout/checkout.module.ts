import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CheckoutRoutingModule } from './checkout-routing.module';
import { SharedModule } from '../shared/shared.module';
import { CheckoutComponent } from './checkout.component';
import { AddressComponent } from './address/address.component';
import { ShipmentComponent } from './shipment/shipment.component';
import { ReviewComponent } from './review/review.component';
import { MatInputModule } from '@angular/material/input';


@NgModule({
  declarations: [
    CheckoutComponent,
    AddressComponent,
    ShipmentComponent,
    ReviewComponent
  ],
  imports: [
    CommonModule,
    CheckoutRoutingModule,
    SharedModule,
    MatInputModule,
  ]
})
export class CheckoutModule { }
