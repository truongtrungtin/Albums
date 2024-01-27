import { Component, Input } from '@angular/core';
import { StoreParams } from '../../models/storeParams';

@Component({
  selector: 'app-pagination-header',
  templateUrl: './pagination-header.component.html',
  styleUrls: ['./pagination-header.component.scss']
})
export class PaginationHeaderComponent {
  @Input() totalItems: number = 0;
  @Input() pageNumber: number = 0;
  @Input() pageSize: number = 0;
}
