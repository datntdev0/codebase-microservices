import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { PermissionDto } from '@shared/proxies/identity-proxies';
import { PermissionService } from 'src/modules/authorization/services/permission-service';

export interface PermissionNode {
  permission?: number;
  permissionName?: string;
  parent?: number;
  checked: boolean;
  indeterminate: boolean;
  children?: PermissionNode[];
}

@Component({
  standalone: false,
  selector: 'app-permission-tree',
  templateUrl: './permission-tree.html',
})
export class PermissionTreeComponent {
  @Input() selectedPermissions: number[] = [];
  @Output() permissionsChange = new EventEmitter<number[]>();

  private readonly permissionService = inject(PermissionService);

  permissionTree: PermissionNode[] = [];

  buildTree(): void {
    this.permissionService.getPermissions().subscribe(permissions => {
      this.buildPermissionTree(permissions);
      if (this.selectedPermissions.length > 0) {
        this.setSelectedPermissions(this.selectedPermissions);
      }
    });
  }

  private buildPermissionTree(permissions: PermissionDto[]): void {
    const permissionMap = new Map<number, PermissionNode>();
    
    permissions.forEach(p => {
      permissionMap.set(p.permission!, {
        permission: p.permission,
        permissionName: p.permissionName,
        parent: p.parent,
        checked: false,
        indeterminate: false,
        children: []
      });
    });

    const rootNodes: PermissionNode[] = [];
    permissionMap.forEach((node) => {
      if (node.parent === undefined || node.parent === null || node.parent === 0) {
        rootNodes.push(node);
      } else {
        const parentNode = permissionMap.get(node.parent);
        if (parentNode) {
          parentNode.children = parentNode.children || [];
          parentNode.children.push(node);
        }
      }
    });

    this.permissionTree = rootNodes;
  }

  setSelectedPermissions(permissions: number[]): void {
    this.selectedPermissions = permissions;
    this.resetPermissionNodes(this.permissionTree);
    
    permissions.forEach(permId => {
      const node = this.findPermissionNode(this.permissionTree, permId);
      if (node) {
        node.checked = true;
        this.updateParentState(node);
      }
    });
  }

  getSelectedPermissions(): number[] {
    const selected: number[] = [];
    this.collectSelectedPermissions(this.permissionTree, selected);
    return selected;
  }

  reset(): void {
    this.resetPermissionNodes(this.permissionTree);
    this.emitChange();
  }

  onPermissionChange(permission: PermissionNode, event: any): void {
    const isChecked = event.target.checked;
    permission.checked = isChecked;
    permission.indeterminate = false;

    if (permission.children && permission.children.length > 0) {
      this.updateChildren(permission, isChecked);
    }

    this.updateParentState(permission);
    this.emitChange();
  }

  private updateChildren(parent: PermissionNode, checked: boolean): void {
    if (!parent.children) return;

    parent.children.forEach(child => {
      child.checked = checked;
      child.indeterminate = false;
      if (child.children && child.children.length > 0) {
        this.updateChildren(child, checked);
      }
    });
  }

  private updateParentState(childNode: PermissionNode): void {
    if (!childNode.parent) return;

    const parent = this.findPermissionNode(this.permissionTree, childNode.parent);
    if (!parent || !parent.children) return;

    const checkedCount = parent.children.filter(c => c.checked).length;
    const indeterminateCount = parent.children.filter(c => c.indeterminate).length;

    if (checkedCount === 0 && indeterminateCount === 0) {
      parent.checked = false;
      parent.indeterminate = false;
    } else if (checkedCount === parent.children.length) {
      parent.checked = true;
      parent.indeterminate = false;
    } else {
      parent.checked = false;
      parent.indeterminate = true;
    }

    this.updateParentState(parent);
  }

  private findPermissionNode(nodes: PermissionNode[], permissionId: number): PermissionNode | null {
    for (const node of nodes) {
      if (node.permission === permissionId) {
        return node;
      }
      if (node.children && node.children.length > 0) {
        const found = this.findPermissionNode(node.children, permissionId);
        if (found) return found;
      }
    }
    return null;
  }

  private collectSelectedPermissions(nodes: PermissionNode[], result: number[]): void {
    nodes.forEach(node => {
      if (node.checked && node.permission !== undefined) {
        result.push(node.permission);
      }
      if (node.children && node.children.length > 0) {
        this.collectSelectedPermissions(node.children, result);
      }
    });
  }

  private resetPermissionNodes(nodes: PermissionNode[]): void {
    nodes.forEach(node => {
      node.checked = false;
      node.indeterminate = false;
      if (node.children && node.children.length > 0) {
        this.resetPermissionNodes(node.children);
      }
    });
  }

  private emitChange(): void {
    this.permissionsChange.emit(this.getSelectedPermissions());
  }
}
