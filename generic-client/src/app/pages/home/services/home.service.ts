import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ModelMetadata } from 'src/app/lib/interfaces/model';

const BASE_URL = 'http://localhost:4000/';

@Injectable({
  providedIn: 'root',
})
export class HomeService {
  constructor(private httpClient: HttpClient) {}

  getModels() {
    return this.httpClient.get<{ models: string[] }>(`${BASE_URL}api/Model`);
  }

  getMetadata(model: string) {
    return this.httpClient.get<{
      data: ModelMetadata;
    }>(`${BASE_URL}api/Model/fields/${model}`);
  }

  getData(model: string, includes?: string[]) {
    return this.httpClient.post<{
      data: any[];
      count: number;
    }>(`${BASE_URL}api/Model/data/${model}`, { includes });
  }
}
