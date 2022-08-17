import {
  Component,
  OnChanges,
  OnInit,
  SimpleChange,
  SimpleChanges,
} from '@angular/core';
import { MatSelectChange } from '@angular/material/select';
import { FieldData, ModelMetadata } from 'src/app/lib/interfaces/model';
import { HomeService } from './services/home.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  constructor(private service: HomeService) {}

  models: string[] = [];
  currentModel?: string;
  modelMetadata: ModelMetadata = {};
  selectedFields: FieldData[] = [];
  data: any[] = [];

  onModelChange(event: MatSelectChange) {
    this.service.getMetadata(event.value).subscribe((data) => {
      this.modelMetadata = data.data;
      this.selectedFields = this.modelMetadata[event.value].fields.filter(
        (field) => !field.foreignModel
      );
      this.service.getData(event.value).subscribe((data) => {
        this.data = data.data;
      });
    });
  }

  ngOnInit(): void {
    this.service.getModels().subscribe((data) => {
      this.models = data.models;
    });
  }
}
