import { Component, ViewChild } from '@angular/core';
import { StoreComponent } from '../store.component';

@Component({
  selector: 'app-product-upload',
  templateUrl: './product-upload.component.html',
  styleUrls: ['./product-upload.component.scss'],
})
export class ProductUploadComponent {
  files: File[] = [];
  selectedFile: File[] = [];
  @ViewChild('fileInputRef') fileInputRef: any;

  constructor(private storeComponent: StoreComponent) {}

  onSelect(event: any) {
    console.log(event);
    this.files.push(...event.addedFiles);
  }

  onRemove(event: any) {
    this.files.splice(this.files.indexOf(event), 1);
  }

  onSelectFile(fileInput: any) {
    this.selectedFile = <File[]>fileInput.target.files;
  }
  resetFileInput() {
    this.files = [];
    this.selectedFile = [];
    // Reset the file input
    if (this.fileInputRef) {
      this.fileInputRef.nativeElement.value = '';
    }
  }

}
