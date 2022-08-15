import { Component, OnChanges, OnInit, SimpleChange } from '@angular/core';
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

  // ngOnChanges(changes: SimpleChange): void {
  //   console.log(changes);
  // }

  ngOnInit(): void {
    this.service.getModels().subscribe((data) => {
      this.models = data.models;
    });
  }
}
