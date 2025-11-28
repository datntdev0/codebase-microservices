import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';

@Component({
  standalone: false,
  selector: 'app-paginator',
  templateUrl: './paginator.html',
})
export class PaginatorComponent implements OnChanges {
  @Input() currentPage: number = 1;
  @Input() totalPages: number = 1;
  @Input() maxVisiblePages: number = 5;
  @Output() pageChange = new EventEmitter<number>();

  visiblePages: number[] = [];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['currentPage'] || changes['totalPages'] || changes['maxVisiblePages']) {
      this.updateVisiblePages();
    }
  }

  updateVisiblePages(): void {
    const pages: number[] = [];
    const half = Math.floor(this.maxVisiblePages / 2);
    let start = Math.max(1, this.currentPage - half);
    let end = Math.min(this.totalPages, start + this.maxVisiblePages - 1);

    if (end - start + 1 < this.maxVisiblePages) {
      start = Math.max(1, end - this.maxVisiblePages + 1);
    }

    for (let i = start; i <= end; i++) {
      pages.push(i);
    }

    this.visiblePages = pages;
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages && page !== this.currentPage) {
      this.pageChange.emit(page);
    }
  }
}
