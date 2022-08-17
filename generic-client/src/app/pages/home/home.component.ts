import { Component, OnInit } from '@angular/core';
import {
  FieldData,
  FieldFlatNode,
  ModelMetadata,
} from 'src/app/lib/interfaces/model';
import { ModelService } from './services/model.service';
import { SelectionModel } from '@angular/cdk/collections';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  constructor(public service: ModelService) {}

  currentModel: string = '';

  ngOnInit(): void {
    this.service.currentModel.subscribe((model) => {
      this.currentModel = model;
    });
  }
}
