import { SelectionModel } from '@angular/cdk/collections';
import { FlatTreeControl } from '@angular/cdk/tree';
import { Component, OnInit } from '@angular/core';
import { MatSelectChange } from '@angular/material/select';
import {
  MatTreeFlatDataSource,
  MatTreeFlattener,
} from '@angular/material/tree';
import {
  FieldData,
  FieldFlatNode,
  FieldNode,
  ModelMetadata,
  NavigationType,
} from '../../../../lib/interfaces/model';
import { ModelService } from '../../services/model.service';

function generateTree(
  data: ModelMetadata[],
  currentModel: string,
  nameSpace: string = '',
  previousModels: { name: string; type: NavigationType }[] = []
) {
  const mainModel = data.find((model) => model.name === currentModel)!;
  const tree: FieldNode[] = [];

  for (var i = 0; i < mainModel.fields.length; i++) {
    const field = mainModel.fields[i];
    tree.push({
      name: `${nameSpace}${field.name}`,
      displayName: field.displayName,
      navigationTypes: previousModels.map((model) => model.type),
    });
  }
  if (mainModel.navigations.length > 0) {
    for (var i = 0; i < mainModel.navigations.length; i++) {
      const navigation = mainModel.navigations[i];
      if (!previousModels.find((m) => m.name === navigation.reference))
        tree.push({
          name: `${nameSpace}${navigation.name}`,
          displayName: navigation.name,
          children: generateTree(
            data,
            navigation.reference,
            `${nameSpace}${navigation.name}.`,
            [...previousModels, { name: currentModel, type: navigation.type }]
          ),
        });
    }
  }

  return tree;
}

@Component({
  selector: 'app-tree-view',
  templateUrl: './tree-view.component.html',
  styleUrls: ['./tree-view.component.scss'],
})
export class TreeViewComponent implements OnInit {
  constructor(private service: ModelService) {
    this.dataSource.data = [];
  }

  modelMetadata: ModelMetadata[] = [];
  checklistSelection = new SelectionModel<FieldFlatNode>(true);

  private _transformer = (node: FieldNode, level: number): FieldFlatNode => {
    return {
      displayName: node.displayName,
      navigationTypes: node.navigationTypes,
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

  descendantsAllSelected(node?: FieldFlatNode): boolean {
    if (node) {
      const descendants = this.treeControl.getDescendants(node);
      const descAllSelected =
        descendants.length > 0 &&
        descendants.every((child) => {
          return this.checklistSelection.isSelected(child);
        });
      return descAllSelected;
    }
    return false;
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
  childItemSelectionToggle(node: FieldFlatNode): void {
    this.checklistSelection.toggle(node);
    this.checkAllParentsSelection(node);
  }

  /** Toggle the to-do item selection. Select/deselect all the descendants node */
  parentSelectionToggle(node: FieldFlatNode): void {
    this.checklistSelection.toggle(node);
    const selected = this.checklistSelection.isSelected(node);
    var descendants: FieldFlatNode[] = this.treeControl.getDescendants(node);
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
    this.service.currentModel.subscribe((model) => {
      if (model) {
        this.service.getMetadata().subscribe(({ data }) => {
          this.dataSource.data = [
            {
              name: model,
              displayName: model,
              children: generateTree(data, model),
            },
          ];
          this.modelMetadata = data;
        });
        this.service.setSelectedFields([]);
        this.checklistSelection.clear();
      }
    });
    this.checklistSelection.changed.subscribe((c) => {
      this.service.setSelectedFields(c.source.selected);
    });
  }
}
