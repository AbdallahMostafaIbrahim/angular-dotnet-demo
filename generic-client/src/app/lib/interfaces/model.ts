import { Sort } from '@angular/material/sort';

export type NavigationType = 'Collection' | 'Reference';

export interface FieldData {
  name: string;
  displayName: string;
  type: string;
  foreignModel?: string;
}

export interface NavigationData {
  name: string;
  reference: string;
  foreignKey: string;
  type: NavigationType;
}

export interface ModelMetadata {
  name: string;
  fields: FieldData[];
  navigations: NavigationData[];
}

export interface FieldNode {
  children?: FieldNode[];
  name: string;
  displayName: string;
  navigationName?: string;
  navigationTypes?: NavigationType[];
}

export interface FieldFlatNode {
  name: string;
  displayName: string;
  level: number;
  expandable: boolean;
  navigationTypes?: NavigationType[];
}

export interface IPage {
  skip: number;
  take: number;
}

export interface FilterParams {
  includes?: string[];
  page?: IPage;
  sort?: Sort;
}
