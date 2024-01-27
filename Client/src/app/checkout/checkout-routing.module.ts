import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CheckoutComponent } from './checkout.component';
import { RouterModule, Routes } from '@angular/router';
import { canActivate } from '../core/guards/auth.guard';
import { AddressComponent } from './address/address.component';
import { ShipmentComponent } from './shipment/shipment.component';
import { ReviewComponent } from './review/review.component';

const routes: Routes = [
  {
    path: '',
    component: CheckoutComponent,
    canActivate: [canActivate],
    children: [
      { path: 'address', component: AddressComponent },
      { path: 'shipment', component: ShipmentComponent },
      { path: 'review', component: ReviewComponent },
      { path: '', redirectTo: 'address', pathMatch: 'full' },
    ],
  },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CheckoutRoutingModule {}
