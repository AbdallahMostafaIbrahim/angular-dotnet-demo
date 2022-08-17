export interface FieldData {
  name: string;
  displayName: string;
  type: string;
  foreignModel?: string;
}

export interface NavigationData {
  name: string;
  reference: string;
  type: 'Collection' | 'Reference';
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
}

export interface FieldFlatNode {
  name: string;
  displayName: string;
  level: number;
  expandable: boolean;
}
