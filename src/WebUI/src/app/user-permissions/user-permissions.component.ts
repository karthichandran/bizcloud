import { Component, OnDestroy, OnInit } from '@angular/core';
import { UntypedFormBuilder,  Validators } from '@angular/forms';
import { fuseAnimations } from '@fuse/animations';
import { UserRoleService } from '../user-role/user-role.service';
import { ToastrService } from 'ngx-toastr';
import * as _ from 'lodash';
import { UserPermissionsService } from './user-permissions.service';
import { LoginService} from '../login/login.service';
@Component({
  selector: 'app-user-permissions',
  templateUrl: './user-permissions.component.html',
  styleUrls: ['./user-permissions.component.scss'],
  animations: fuseAnimations
})
export class UserPermissionsComponent implements OnInit, OnDestroy {

  reportingToDDL: any[] = [{ id: '1', description: 'Managers' }, { id: '2', description: 'Groups' }];
  rolesList: any[] = [];
  rolesDDl: any[] = [];
  rowData: any[] = [];
  columnDef: any[] = [];
  selectedRole: any;
  baseRolePermissionsList: any[] = [];
  constructor(private _formBuilder: UntypedFormBuilder, private roleService: UserRoleService, private toastr: ToastrService, private permissionServ: UserPermissionsService, private loginService: LoginService) {

  }

  ngOnInit(): void {
    
    this.columnDef = [
      { 'header': 'Name', 'field': 'name', 'type': 'textbox' },
      { 'header': 'Create', 'field': 'createPerm', 'type': 'checkbox', 'checkall': true },
      { 'header': 'Edit', 'field': 'editPerm', 'type': 'checkbox', 'checkall': true },
      { 'header': 'View', 'field': 'viewPerm', 'type': 'checkbox', 'checkall': true },
      { 'header': 'Delete', 'field': 'deletePerm', 'type': 'checkbox', 'checkall': true }
    ];
    this.getRolesList();
  }

  getRolesList() {
    this.roleService.getUserRoles().subscribe(response => {
      this.rolesList = _.cloneDeep(response);
      response.splice(0, 0, { 'roleID': 0, 'name': '' });
     this.rolesDDl= response;
    });
  }

  selectedRoleEve(eve) {
    this.permissionServ.getUserPermissionsByRoleId(eve).subscribe(response => {
      this.baseRolePermissionsList = _.cloneDeep(response);
      this.rowData = response;
    });
  }

  save() {    
      let model = this.rowData;
    _.forEach(model, o => {
      o.roleID = this.selectedRole;
    });
    
    this.permissionServ.save(model).subscribe(response => {
      this.toastr.success("Saved Succussfully");
      this.loginService.refreshToken();
      this.selectedRoleEve(this.selectedRole);
    });
  }

  /**
     * On destroy
     */
  ngOnDestroy(): void {
  }
}
