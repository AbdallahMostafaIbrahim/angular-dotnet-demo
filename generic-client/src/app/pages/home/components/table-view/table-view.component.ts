import {
  AfterViewInit,
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { FieldData, FieldFlatNode } from '../../../../lib/interfaces/model';
import { ModelService } from '../../services/model.service';

@Component({
  selector: 'table-view',
  templateUrl: './table-view.component.html',
  styleUrls: ['./table-view.component.scss'],
})
export class TableViewComponent implements OnInit {
  constructor(private service: ModelService) {}

  data: any[] = [];
  displayedColumns: string[] = [];
  realColumns: string[] = [];
  selectedFields: FieldFlatNode[] = [];

  ngOnInit(): void {
    this.service.selectedFields.subscribe((value) => {
      value = value.filter((field) => field.navigationTypes);
      this.selectedFields = value;
      this.realColumns = value.map((field) => field.name);
      this.displayedColumns = value.map((field) => field.displayName);
      if (value.length > 0) {
        this.service.getData(value.map((f) => f.name)).subscribe((data) => {
          this.data = data.data;
        });
      }
    });
  }
}
