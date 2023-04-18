import { Component, OnDestroy, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { fuseAnimations } from '@fuse/animations';
import {UserRoleService } from '../user-role/user-role.service';
import { ToastrService } from 'ngx-toastr';
import { MatTabChangeEvent } from '@angular/material/tabs';
@Component({
  selector: 'app-user-role',
  templateUrl: './user-role.component.html',
  styleUrls: ['./user-role.component.scss'],
  animations: fuseAnimations
})
export class UserRoleComponent implements OnInit, OnDestroy {
  form: UntypedFormGroup;
  reportingToDDL: any[] = [{ id: '1', description: 'Managers' }, { id: '2', description: 'Groups' }];
  rowData: any[] = [];
  columnDef: any[] = [];
 
  constructor(private _formBuilder: UntypedFormBuilder, private roleService: UserRoleService, private toastr: ToastrService) {
    
  }

  ngOnInit(): void {
    // Reactive Form
    this.form = this._formBuilder.group({
      roleID:[''],
      name: ['', Validators.required],
      reportingTo: ['', Validators.required],
      isOrganizationRole: [true, Validators.required]
    });
    this.columnDef = [
      { 'header': 'Name', 'field': 'name', 'type': 'textbox' },    
      { 'header': 'Seletions', 'field': 'reportingTo', 'type': 'selection', 'options': this.reportingToDDL },
      { 'header': 'Is Organization Role', 'field': 'isOrganizationRole', 'type': 'label' },
      { 'header': '', 'field': '', 'type': 'button', 'activity': ['edit'], 'width': 80 }
    ];
    this.getRolesList();
  }

  getRolesList() {
    this.roleService.getUserRoles().subscribe(response => {
      this.rowData = response;
    });
  }
  getRoleById(roleId:number) {
    this.roleService.getUserRoleEntry(roleId).subscribe(response => {
      response.reportTo = parseInt(response.reportTo);
      this.form.patchValue(response);
    });
  }

  selectedRow(eve) {   
    this.getRoleById(eve.roleID);
    var ele = document.getElementsByClassName('mat-tab-label') as HTMLCollectionOf<HTMLElement>;
    ele[0].click();
  }
  clear() {
    this.form.reset();
    this.form.get('isOrganizationRole').setValue(true);
  }
  tabChanged(eve: MatTabChangeEvent) {
    if (eve.index == 1) {
      this.getRolesList();
    }
  }

  save() {
    if (this.form.valid) {
      let model = this.form.value;
      if (model.roleID == "" || model.roleID == null) {
        model.roleID = 0;
      }
      let isNew: boolean = (model.roleID > 0) ? false : true;
      this.roleService.save(model, isNew).subscribe(response => {
        this.toastr.success("Saved Succussfully");
        this.getRoleById(response);
      })
    }
  }

  /**
     * On destroy
     */
  ngOnDestroy(): void {
  }
}
