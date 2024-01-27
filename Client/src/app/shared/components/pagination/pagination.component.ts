import { Component, Input, Output, EventEmitter } from '@angular/core';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss']
})
export class PaginationComponent {
  @Input() totalItems: number = 0;
  @Input() pageSize: number = 0;
  @Output() pageChanged  = new EventEmitter<PageChangedEvent>(); // Emitting a number (new page number)

  onPageChanged(event: PageChangedEvent) {
    this.pageChanged.emit(event); // Emit the new page number
  }
}
