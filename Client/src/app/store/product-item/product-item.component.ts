// product-item.component.ts

import { Component, Input, ElementRef, OnDestroy, AfterViewInit } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { FileAttachment } from 'src/app/shared/models/FileAttachment';
import { Product } from 'src/app/shared/models/Product';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.scss']
})
export class ProductItemComponent {
  @Input() file: FileAttachment | null = null;

  constructor(private basketService: BasketService, private el: ElementRef) {}

  extractImageName(): string | null {
    if (this.file && this.file.fileUrl) {
      const parts = this.file.fileUrl.split('/');
      if (parts.length > 0) {
        return parts[parts.length - 1];
      }
    }
    return null;
  }

  // addItemToBasket() {
  //   this.file && this.basketService.addItemToBasket(this.file);
  // }

  isImage(): boolean {
    return !!this.file && this.isImageType(this.file.fileExtention);
  }

  isVideo(): boolean {
    return !!this.file && this.isVideoType(this.file.fileExtention);
  }

  private isImageType(type: string): boolean {
    const imageExtensions = ['.heif', '.png', '.jpg', '.jpeg'];
    return imageExtensions.includes(type.toLowerCase());
  }

  private isVideoType(type: string): boolean {
    const videoExtensions = ['.hevc', '.mp4', '.mov']; // Added 'mov' for MOV files
    return videoExtensions.includes(type.toLowerCase());
  }
  downloadFile() {
    // Implement the download logic based on the file type (image or video)
    if (this.isImage()) {
      // For image download logic
      const imageName = this.extractImageName();
      if (imageName) {
        const imageUrl = 'assets/images/files/' + imageName;
        const link = document.createElement('a');
        link.href = imageUrl;
        link.download = imageName;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      }
    } else if (this.isVideo()) {
      // For video download logic
      if (this.file && this.file.fileUrl) {
        const videoUrl = this.file.fileUrl; // Replace with the actual video URL
        window.open(videoUrl, '_blank');
      }
    }
  }

  showImageInfo() {
    // Placeholder function to log information for image
    if (this.file) {
      console.log(`Image Info: ${this.file.fileAttachmentName}, Type: ${this.file.fileType}`);
    }
  }

  showVideoInfo() {
    // Placeholder function to log information for video
    if (this.file) {
      console.log(`Video Info: ${this.file.fileAttachmentName}, Type: ${this.file.fileType}`);
    }
  }
}
