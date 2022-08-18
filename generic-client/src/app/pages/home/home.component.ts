import { Component, OnInit } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { FieldFlatNode, IPage } from 'src/app/lib/interfaces/model';
import { ModelService } from './services/model.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  constructor(public service: ModelService) {}

  currentModel: string = '';
  selectedFields: FieldFlatNode[] = [];
  data: any[] = [];
  count: number = 0;
  filter: string = '';
  page: IPage = { skip: 0, take: 10 };
  sort: Sort = { active: '', direction: '' };

  refetch(): void {
    this.service
      .getData({
        includes: this.selectedFields.map((f) => f.name),
        page: this.page,
        sort: this.sort,
      })
      .subscribe((data) => {
        this.data = data.data;
        this.count = data.count;
      });
  }

  ngOnInit(): void {
    this.service.currentModel.subscribe((model) => {
      this.currentModel = model;
      this.data = [];
      this.selectedFields = [];
      this.page = { skip: 0, take: 10 };
      this.sort = { active: '', direction: '' };
    });
    this.service.selectedFields.subscribe((fields) => {
      this.selectedFields = fields;
      if (fields.length > 0) {
        this.refetch();
      }
    });
  }

  onPageChange(page: IPage) {
    this.page = page;
    this.refetch();
  }
  onSortChange(sort: Sort) {
    this.sort = sort;
    this.refetch();
  }
  onSelectedFieldsChange(fields: FieldFlatNode[]) {
    this.selectedFields = fields;
    this.refetch();
  }
}
