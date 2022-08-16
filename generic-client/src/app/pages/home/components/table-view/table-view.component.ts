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
import { TableViewItem } from './table-view-datasource';

@Component({
  selector: 'table-view',
  templateUrl: './table-view.component.html',
  styleUrls: ['./table-view.component.scss'],
})
export class TableViewComponent implements AfterViewInit, OnChanges {
  // @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatTable) table!: MatTable<TableViewItem>;
  @Input() dataSource: any[] = [];
  @Input() displayedColumns: string[] = [];

  constructor() {}

  ngOnChanges(changes: SimpleChanges): void {
    console.log(changes);
    // if (changes.dataSource) {
    //   this.dataSource = changes.dataSource.currentValue;
    // }
  }

  ngAfterViewInit(): void {
    // this.dataSource.sort = this.sort;
    // this.dataSource.paginator = this.paginator;
    // this.table.dataSource = this.dataSource;
  }
}
