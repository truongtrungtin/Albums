import { Component, OnInit } from '@angular/core';
import { StoreService } from '../store.service';
import { ActivatedRoute } from '@angular/router';
import { Product } from 'src/app/shared/models/Product';
import { BreadcrumbService } from 'xng-breadcrumb';
import { BasketService } from 'src/app/basket/basket.service';
import { FileAttachment } from 'src/app/shared/models/FileAttachment';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  fileAttachment?: FileAttachment;
  quantity: number = 1;

  constructor(
    private storeService:StoreService, 
    private activatedRoute: ActivatedRoute,
    private breadcrumbService: BreadcrumbService,
    private basketService: BasketService,
    ){}
  ngOnInit(): void {
    this.loadFileAttachment();
  }


  loadFileAttachment(){
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if(id){
      this.storeService.getFileAttachment(id).subscribe({
        next:file => {
          this.fileAttachment = file,
          this.breadcrumbService.set('@fileAttachmentName',file.fileAttachmentName)
        },
        error: error => console.log(error)
      });
    }
  }

  extractImageName(): string | null {
    if(this.fileAttachment && this.fileAttachment.fileUrl){
      const parts = this.fileAttachment.fileUrl.split('/');
      if(parts.length > 0){
        return parts[parts.length - 1];
      }
    }
    return null;
  }

  // incrementQuantity() {
  //   this.quantity++;
  // }

  // decrementQuantity() {
  //   if (this.quantity > 1) {
  //     this.quantity--;
  //   }
  // }

  // addItemToBasket(){
  //   this.product && this.basketService.addItemToBasket(this.product, this.quantity);
  //  }
}
