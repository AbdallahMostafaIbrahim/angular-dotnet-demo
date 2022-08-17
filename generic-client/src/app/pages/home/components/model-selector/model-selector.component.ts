import { Component, OnInit } from '@angular/core';
import { MatSelectChange } from '@angular/material/select';
import { ModelService } from '../../services/model.service';

@Component({
  selector: 'app-model-selector',
  templateUrl: './model-selector.component.html',
  styleUrls: ['./model-selector.component.scss'],
})
export class ModelSelectorComponent implements OnInit {
  constructor(private service: ModelService) {}

  models: string[] = [];
  currentModel: string = '';

  onModelChange(event: MatSelectChange) {
    this.service.setCurrentModel(event.value);
  }

  ngOnInit(): void {
    this.service.getModels().subscribe((data) => {
      this.models = data.models;
    });
    this.service.currentModel.subscribe((currentModel) => {
      this.currentModel = currentModel;
    });
  }
}
