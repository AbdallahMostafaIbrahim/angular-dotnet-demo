import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import {
  FieldFlatNode,
  FilterParams,
  IPage,
  ModelMetadata,
} from 'src/app/lib/interfaces/model';

const BASE_URL = 'http://localhost:4000';

@Injectable({
  providedIn: 'root',
})
export class ModelService {
  constructor(private httpClient: HttpClient) {}

  private currentModelSubject: BehaviorSubject<string> =
    new BehaviorSubject<string>('');
  currentModel: Observable<string> = this.currentModelSubject.asObservable();

  private metadataSubject: BehaviorSubject<ModelMetadata[]> =
    new BehaviorSubject<ModelMetadata[]>([]);
  metdata: Observable<ModelMetadata[]> = this.metadataSubject.asObservable();

  private selectedFieldsSubject: BehaviorSubject<FieldFlatNode[]> =
    new BehaviorSubject<FieldFlatNode[]>([]);
  selectedFields: Observable<FieldFlatNode[]> =
    this.selectedFieldsSubject.asObservable();

  setSelectedFields(fields: FieldFlatNode[]) {
    this.selectedFieldsSubject.next(fields);
  }

  setMetadata(metadata: ModelMetadata[]) {
    this.metadataSubject.next(metadata);
  }

  setCurrentModel(model: string) {
    this.currentModelSubject.next(model);
  }

  getModels() {
    return this.httpClient.get<{ models: string[] }>(`${BASE_URL}/api/Model`);
  }

  getMetadata() {
    return this.httpClient.get<{
      data: ModelMetadata[];
    }>(`${BASE_URL}/api/Model/fields/${this.currentModelSubject.value}`);
  }

  getData({ includes, page, sort }: FilterParams) {
    return this.httpClient.post<{
      data: any[];
      count: number;
    }>(`${BASE_URL}/api/Model/data/${this.currentModelSubject.value}`, {
      includes,
      skip: page?.skip || 0,
      take: page?.take || 10,
      orderBy:
        sort?.active && sort?.direction
          ? `${sort.active} ${sort.direction}`
          : undefined,
    });
  }

  exportAsCSV({ includes, page, sort }: FilterParams) {
    return this.httpClient.post(
      `${BASE_URL}/api/Model/export/${this.currentModelSubject.value}`,
      {
        includes,
        skip: page?.skip || 0,
        take: page?.take || 10,
        orderBy:
          sort?.active && sort?.direction
            ? `${sort.active} ${sort.direction}`
            : undefined,
      },
      { responseType: 'blob' }
    );
  }
}
