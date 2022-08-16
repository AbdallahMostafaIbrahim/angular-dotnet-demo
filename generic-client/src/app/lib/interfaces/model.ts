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
  [key: string]: {
    fields: FieldData[];
    navigations: NavigationData[];
  };
}
