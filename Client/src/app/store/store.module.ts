import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreComponent } from './store.component';
import { ProductItemComponent } from './product-item/product-item.component';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { ProductUploadComponent } from './product-upload/product-upload.component';

import { StoreRoutingModule } from './store-routing.module';
import { NgxDropzoneModule } from 'ngx-dropzone';
import { ProductProfilesComponent } from './product-profiles/product-profiles.component';



@NgModule({
  declarations: [
    StoreComponent,
    ProductItemComponent,
    ProductDetailsComponent,
    ProductUploadComponent,
    ProductProfilesComponent
  ],
  imports: [
    CommonModule,
    StoreRoutingModule,
    FormsModule,
    SharedModule,
    NgxDropzoneModule
  ],
  exports: [
    StoreComponent,
    ProductItemComponent,
    ProductDetailsComponent,
    ProductUploadComponent,
    ProductProfilesComponent
  ]
})
export class StoreModule { }

