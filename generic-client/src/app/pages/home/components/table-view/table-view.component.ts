import {
  animate,
  state,
  style,
  transition,
  trigger,
} from '@angular/animations';
import { CdkDragDrop, CdkDragStart } from '@angular/cdk/drag-drop';
import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { FieldFlatNode, IPage } from '../../../../lib/interfaces/model';

@Component({
  selector: 'table-view[selectedFields][data]',
  templateUrl: './table-view.component.html',
  styleUrls: ['./table-view.component.scss'],
  animations: [
    trigger('detailExpand', [
      state(
        'collapsed',
        style({ height: '0px', minHeight: '0', opacity: '0' })
      ),
      state('expanded', style({ height: '*', opacity: 1 })),
      transition(
        'expanded <=> collapsed',
        animate('100ms cubic-bezier(0.4, 0.0, 0.2, 1)')
      ),
    ]),
  ],
})
export class TableViewComponent implements OnInit, OnChanges {
  constructor() {}
  pageSizeOptions: number[] = [5, 10, 25, 100];

  @Input()
  selectedFields: FieldFlatNode[] = [];
  @Output()
  selectedFieldsChange = new EventEmitter<FieldFlatNode[]>();
  @Input()
  data: any[] = [];
  @Input()
  count: number = 1000;
  @Input()
  innerTable = false;
  @Input()
  page: IPage = { skip: 0, take: 10 };
  @Output()
  pageChange = new EventEmitter<IPage>();
  @Input()
  sort: Sort = { active: '', direction: '' };
  @Output()
  sortChange = new EventEmitter<Sort>();

  displayedColumns: string[] = ['actions'];
  realColumns: string[] = ['actions'];
  distinctCollections: Set<string> = new Set<string>();
  selectedReferenceFields: FieldFlatNode[] = [];
  selectedCollectionFields: FieldFlatNode[] = [];
  previousIndex?: number;

  onPagination(event: PageEvent) {
    this.page = {
      skip: event.pageSize * event.pageIndex,
      take: event.pageSize,
    };
    this.pageChange.emit(this.page);
  }

  toggleRow(element: { _expanded?: boolean }) {
    element._expanded = !element._expanded;
  }

  sortData(event: Sort) {
    this.sort = event;
    this.sortChange.emit(event);
  }

  generateFieldsForInnerTable(fields: FieldFlatNode[]): FieldFlatNode[] {
    return fields.map((field) => ({
      ...field,
      name: field.name.split('.').splice(1).join('.'),
      navigationTypes: field.navigationTypes?.slice(1),
    }));
  }

  dragStarted(event: CdkDragStart, index: number) {
    this.previousIndex = index;
  }

  headerDrop(event: CdkDragDrop<any, any, any>) {
    if (event) {
      var b = this.selectedFields[event.currentIndex];
      this.selectedFields[event.currentIndex] =
        this.selectedFields[event.previousIndex!];
      this.selectedFields[event.previousIndex!] = b;
      this.selectedFieldsChange.emit(this.selectedFields);
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['selectedFields']) {
      this.selectedFields = this.selectedFields.filter(
        (field) => field.navigationTypes
      );

      this.selectedReferenceFields = this.selectedFields.filter(
        (v) => !v.navigationTypes?.includes('Collection')
      );
      this.selectedCollectionFields = this.selectedFields.filter((v) =>
        v.navigationTypes?.includes('Collection')
      );
      this.realColumns = this.selectedReferenceFields.map(
        (field) => field.name
      );
      this.realColumns.push('actions');
      this.displayedColumns = this.selectedReferenceFields.map(
        (field) => field.displayName
      );
      this.distinctCollections = new Set<string>();
      for (var i = 0; i < this.selectedCollectionFields.length; i++) {
        if (this.selectedCollectionFields[i].name.includes('.')) {
          const field = this.selectedCollectionFields[i];
          if (!field.navigationTypes) continue;
          if (field.navigationTypes[0] === 'Collection') {
            this.distinctCollections.add(field.name.split('.')[0]);
          }
        }
      }
    }
  }

  ngOnInit(): void {}
}
