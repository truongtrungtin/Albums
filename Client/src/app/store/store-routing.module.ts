import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreComponent } from './store.component';
import { ProductDetailsComponent } from './product-details/product-details.component';

const routes: Routes = [
  {path: '', component:StoreComponent},
  {path: 'share', component:ProductDetailsComponent, data: { breadcrumb: {alias:'share'} }},
  {path: ':id', component:ProductDetailsComponent, data: { breadcrumb: {alias:'productName'} }},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StoreRoutingModule { }
