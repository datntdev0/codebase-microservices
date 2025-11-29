import { Component, Input, Output, EventEmitter, TemplateRef } from '@angular/core';

export interface DatatableColumn {
  key: string;
  title: string;
  minWidth?: string;
  sortable?: boolean;
  template?: (item: any) => string;
}

@Component({
  standalone: false,
  selector: 'app-datatable',
  templateUrl: './datatable.html',
})
export class DatatableComponent {
  @Input() data: any[] = [];
  @Input() columns: DatatableColumn[] = [];
  @Input() actionsTemplate?: TemplateRef<any>;
  @Input() checkboxEnabled: boolean = true;
  @Input() currentPage: number = 1;
  @Input() totalPages: number = 1;
  @Output() pageChange = new EventEmitter<number>();
  
  selectedItems: Set<any> = new Set();
  allSelected: boolean = false;

  toggleSelectAll() {
    if (this.allSelected) {
      this.data.forEach(item => this.selectedItems.add(item));
    } else {
      this.selectedItems.clear();
    }
  }

  toggleSelectItem(item: any) {
    if (this.selectedItems.has(item)) {
      this.selectedItems.delete(item);
    } else {
      this.selectedItems.add(item);
    }
    this.allSelected = this.selectedItems.size === this.data.length;
  }

  isSelected(item: any): boolean {
    return this.selectedItems.has(item);
  }

  getCellValue(item: any, column: DatatableColumn): string {
    if (column.template) {
      return column.template(item);
    }
    return item[column.key] || '';
  }

  onPageChange(page: number): void {
    this.pageChange.emit(page);
  }
}
