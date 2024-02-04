import { Component, inject, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FileAttachment } from 'src/app/shared/models/FileAttachment';
import { StoreService } from '../store.service';

@Component({
  selector: 'app-modal-image',
  templateUrl: './modal-image.component.html',
  styleUrls: ['./modal-image.component.scss'],
})
export class ModalImageComponent {

  activeModal = inject(NgbActiveModal);

  @Input() file!: FileAttachment;
  constructor(
    private storeService: StoreService,
  ) { }
  downloadFile(fileName: string, fileUrl: string): void {
    this.storeService.downloadFile(fileName,fileUrl);
  }
}
