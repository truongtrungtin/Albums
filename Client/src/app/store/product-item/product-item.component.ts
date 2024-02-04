// product-item.component.ts

import { Component, Input, OnInit, ViewChild, inject} from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FileAttachment } from 'src/app/shared/models/FileAttachment';
import { ModalImageComponent } from '../modal-image/modal-image.component';
import { ModalVideoComponent } from '../modal-video/modal-video.component';



@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.scss']
})
export class ProductItemComponent{
  @Input() file: FileAttachment | null = null;
  private modalService = inject(NgbModal);

  openModalImage() {
    const modalRef = this.modalService.open(ModalImageComponent);
		modalRef.componentInstance.file = this.file;
  }
  openModalVideo() {
    const modalRef = this.modalService.open(ModalVideoComponent);
		modalRef.componentInstance.file = this.file;
  }

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
