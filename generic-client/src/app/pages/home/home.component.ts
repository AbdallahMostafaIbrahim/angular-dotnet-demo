import { Component, OnInit } from '@angular/core';
import { MatSelectChange } from '@angular/material/select';
import {
  FieldData,
  FieldFlatNode,
  FieldNode,
  ModelMetadata,
} from 'src/app/lib/interfaces/model';
import { HomeService } from './services/home.service';
import { FlatTreeControl } from '@angular/cdk/tree';
import {
  MatTreeFlatDataSource,
  MatTreeFlattener,
} from '@angular/material/tree';
import { SelectionModel } from '@angular/cdk/collections';

function generateTree(
  data: ModelMetadata[],
  currentModel: string,
  previousModels: string[] = []
) {
  const mainModel = data.find((model) => model.name === currentModel)!;
  const tree: FieldNode[] = [];

  for (var i = 0; i < mainModel.fields.length; i++) {
    const field = mainModel.fields[i];
    tree.push({
      name: field.name,
      displayName: field.displayName,
    });
  }
  if (mainModel.navigations.length > 0) {
    for (var i = 0; i < mainModel.navigations.length; i++) {
      const navigation = mainModel.navigations[i];
      if (!previousModels.includes(navigation.reference))
        tree.push({
          name: navigation.name,
          displayName: navigation.name,
          children: generateTree(data, navigation.reference, [
            ...previousModels,
            currentModel,
          ]),
        });
    }
  }

  return tree;
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  constructor(private service: HomeService) {}

  models: string[] = [];
  currentModel?: string;
  modelMetadata: ModelMetadata[] = [];
  selectedFields: FieldData[] = [];
  checklistSelection = new SelectionModel<FieldFlatNode>(true);
  data: any[] = [];

  private _transformer = (node: FieldNode, level: number): FieldFlatNode => {
    return {
      displayName: node.displayName,
      expandable: !!node.children && node.children.length > 0,
      name: node.name,
      level: level,
    };
  };

  treeControl = new FlatTreeControl<FieldFlatNode>(
    (node) => node.level,
    (node) => node.expandable
  );

  treeFlattener = new MatTreeFlattener<FieldNode, FieldFlatNode, FieldFlatNode>(
    this._transformer,
    (node) => node.level,
    (node) => node.expandable,
    (node) => node.children
  );

  getLevel = (node: FieldFlatNode) => node.level;

  isExpandable = (node: FieldFlatNode) => node.expandable;

  hasChild = (_: number, _nodeData: FieldFlatNode) => _nodeData.expandable;

  dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

  onModelChange(event: MatSelectChange) {
    this.service.getMetadata(event.value).subscribe(({ data }) => {
      this.dataSource.data = [
        {
          name: event.value,
          displayName: event.value,
          children: generateTree(data, event.value),
        },
      ];

      this.modelMetadata = data;
      // this.selectedFields = this.modelMetadata
      //   .find((m) => m.name === event.value)
      //   ?.fields.filter((field) => !field.foreignModel)!;

      this.refetchData();
    });
  }

  refetchData() {
    this.service
      .getData(
        this.currentModel!,
        this.selectedFields.map((f) => f.name)
      )
      .subscribe((data) => {
        this.data = data.data;
      });
  }

  descendantsAllSelected(node: FieldFlatNode): boolean {
    const descendants = this.treeControl.getDescendants(node);
    const descAllSelected =
      descendants.length > 0 &&
      descendants.every((child) => {
        return this.checklistSelection.isSelected(child);
      });
    return descAllSelected;
  }

  /** Whether part of the descendants are selected */
  descendantsPartiallySelected(node: FieldFlatNode): boolean {
    const descendants = this.treeControl.getDescendants(node);
    const result = descendants.some((child) =>
      this.checklistSelection.isSelected(child)
    );
    return result && !this.descendantsAllSelected(node);
  }

  /* Get the parent node of a node */
  getParentNode(node: FieldFlatNode): FieldFlatNode | null {
    const currentLevel = this.getLevel(node);

    if (currentLevel < 1) {
      return null;
    }

    const startIndex = this.treeControl.dataNodes.indexOf(node) - 1;

    for (let i = startIndex; i >= 0; i--) {
      const currentNode = this.treeControl.dataNodes[i];

      if (this.getLevel(currentNode) < currentLevel) {
        return currentNode;
      }
    }
    return null;
  }

  /* Checks all the parents when a leaf node is selected/unselected */
  checkAllParentsSelection(node: FieldFlatNode): void {
    let parent: FieldFlatNode | null = this.getParentNode(node);
    while (parent) {
      this.checkRootNodeSelection(parent);
      parent = this.getParentNode(parent);
    }
  }

  /** Check root node checked state and change it accordingly */
  checkRootNodeSelection(node: FieldFlatNode): void {
    const nodeSelected = this.checklistSelection.isSelected(node);
    const descendants = this.treeControl.getDescendants(node);
    const descAllSelected =
      descendants.length > 0 &&
      descendants.every((child) => {
        return this.checklistSelection.isSelected(child);
      });
    if (nodeSelected && !descAllSelected) {
      this.checklistSelection.deselect(node);
    } else if (!nodeSelected && descAllSelected) {
      this.checklistSelection.select(node);
    }
  }

  /** Toggle a leaf to-do item selection. Check all the parents to see if they changed */
  todoLeafItemSelectionToggle(node: FieldFlatNode): void {
    this.checklistSelection.toggle(node);
    this.checkAllParentsSelection(node);
  }

  /** Toggle the to-do item selection. Select/deselect all the descendants node */
  todoItemSelectionToggle(node: FieldFlatNode): void {
    this.checklistSelection.toggle(node);
    const selected = this.checklistSelection.isSelected(node);
    var descendants: FieldFlatNode[] = this.treeControl.getDescendants(node);
    console.log(selected);
    if (selected) {
      this.checklistSelection.select(...descendants);
    } else {
      this.checklistSelection.deselect(...descendants);
    }
    // Force update for the parent
    descendants.forEach((child) => this.checklistSelection.isSelected(child));
    this.checkAllParentsSelection(node);
  }

  ngOnInit(): void {
    this.service.getModels().subscribe((data) => {
      this.models = data.models;
    });
    this.checklistSelection.changed.subscribe((t) => {
      t.added.forEach((node) => {
        this.selectedFields = [
          ...this.selectedFields,
          {
            name: node.name,
            displayName: node.displayName,
            type: '',
          },
        ];
      });
      t.removed.forEach((node) => {
        this.selectedFields = this.selectedFields.filter(
          (f) => f.name !== node.name
        );
      });
      this.refetchData();
    });
  }
}
