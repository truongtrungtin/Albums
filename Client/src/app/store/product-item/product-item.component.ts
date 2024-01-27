// product-item.component.ts

import { Component, Input, ElementRef, OnDestroy, AfterViewInit } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { Product } from 'src/app/shared/models/Product';
import videojs from 'video.js';
import Player from "video.js/dist/types/player";

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.scss']
})
export class ProductItemComponent {
  @Input() product: Product | null = null;

  constructor(private basketService: BasketService, private el: ElementRef) {}

  extractImageName(): string | null {
    if (this.product && this.product.url) {
      const parts = this.product.url.split('/');
      if (parts.length > 0) {
        return parts[parts.length - 1];
      }
    }
    return null;
  }

  addItemToBasket() {
    this.product && this.basketService.addItemToBasket(this.product);
  }

  isImage(): boolean {
    return !!this.product && this.isImageType(this.product.productType);
  }

  isVideo(): boolean {
    return !!this.product && this.isVideoType(this.product.productType);
  }

  private isImageType(type: string): boolean {
    const imageExtensions = ['heif', 'png', 'jpg', 'jpeg', 'image/jpeg'];
    return imageExtensions.includes(type.toLowerCase());
  }

  private isVideoType(type: string): boolean {
    const videoExtensions = ['hevc', 'video/mp4', 'mov']; // Added 'mov' for MOV files
    return videoExtensions.includes(type.toLowerCase());
  }
  downloadFile() {
    // Implement the download logic based on the file type (image or video)
    if (this.isImage()) {
      // For image download logic
      const imageName = this.extractImageName();
      if (imageName) {
        const imageUrl = 'assets/images/products/' + imageName;
        const link = document.createElement('a');
        link.href = imageUrl;
        link.download = imageName;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      }
    } else if (this.isVideo()) {
      // For video download logic
      if (this.product && this.product.url) {
        const videoUrl = this.product.url; // Replace with the actual video URL
        window.open(videoUrl, '_blank');
      }
    }
  }

  showImageInfo() {
    // Placeholder function to log information for image
    if (this.product) {
      console.log(`Image Info: ${this.product.name}, Type: ${this.product.productType}`);
    }
  }

  showVideoInfo() {
    // Placeholder function to log information for video
    if (this.product) {
      console.log(`Video Info: ${this.product.name}, Type: ${this.product.productType}`);
    }
  }
}
