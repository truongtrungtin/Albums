import { Component, ViewChild } from '@angular/core';
import { StoreComponent } from '../store.component';
import { Profile } from 'src/app/shared/models/Profile';

@Component({
  selector: 'app-product-profiles',
  templateUrl: './product-profiles.component.html',
  styleUrls: ['./product-profiles.component.scss'],
})
export class ProductProfilesComponent {
  files: File[] = [];
  selectedFile: File[] = [];
  @ViewChild('fileInputRef') fileInputRef: any;

  constructor(private storeComponent: StoreComponent) {}

  onSelect(event: any) {
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
      this.fileInputRef.nativeElement.value = '';
    }
  }

}
