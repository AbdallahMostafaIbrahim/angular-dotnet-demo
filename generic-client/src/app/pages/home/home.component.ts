import { Component, OnInit } from '@angular/core';
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
  page: IPage = { skip: 0, take: 10 };

  refetch(): void {
    this.service
      .getData(
        this.selectedFields.map((f) => f.name),
        this.page
      )
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
}
