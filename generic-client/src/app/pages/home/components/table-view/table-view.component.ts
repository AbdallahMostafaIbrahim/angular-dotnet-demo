import {
  AfterViewInit,
  Component,
  Input,
  OnChanges,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable } from '@angular/material/table';
import { FieldData } from '../../../../lib/interfaces/model';

@Component({
  selector: 'table-view',
  templateUrl: './table-view.component.html',
  styleUrls: ['./table-view.component.scss'],
})
export class TableViewComponent implements AfterViewInit, OnChanges {
  @Input() dataSource: any[] = [];
  @Input() columns: FieldData[] = [];

  constructor() {}

  displayedColumns: string[] = [];
  realColumns: string[] = [];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['columns']) {
      this.displayedColumns = this.columns.map((field) => field.displayName);
      this.realColumns = this.columns.map((field) => field.name);
    }
  }

  ngAfterViewInit(): void {}
}
