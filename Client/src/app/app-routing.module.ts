import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { ConnectionRefusedComponent } from './core/connection-refused/connection-refused.component';

const routes: Routes = [
  {path: '', component:HomeComponent, data: { breadcrumb: 'Home' }},
  // {path: 'store', loadChildren:()=> import('./store/store.module').then(m=>m.StoreModule), data: { breadcrumb: 'Shop' }},
  // {path: 'basket', loadChildren:()=> import('./basket/basket.module').then(m=>m.BasketModule), data: { breadcrumb: 'Basket' }},
  {path: 'account', loadChildren:()=> import('./account/account.module').then(m=>m.AccountModule), data: { breadcrumb: 'Account' }},
  {path: 'gallery', loadChildren:()=> import('./store/store.module').then(m=>m.StoreModule), data: { breadcrumb: 'Gallery' }},

  // {path: 'checkout', loadChildren: () => import('./checkout/checkout.module').then(m => m.CheckoutModule), data: { breadcrumb: 'Checkout' }},
  {path: '404', component:NotFoundComponent, data: { breadcrumb: 'Not Found' }},
  {path: '500', component:ServerErrorComponent, data: { breadcrumb: 'Server error' }},
  {path: 'connection-error', component:ConnectionRefusedComponent, data: { breadcrumb: 'Connection Error' }},

  {path: '**', component: NotFoundComponent, pathMatch:'full'}

];

@NgModule({
  imports: [RouterModule.forRoot(routes, {useHash: true})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
