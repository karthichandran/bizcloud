import { Component, OnDestroy, OnInit ,ViewChild, ElementRef} from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { fuseAnimations } from '@fuse/animations';
import { IRoleReportingTo } from '../models/RoleReportingTo';
import * as Xlsx from 'xlsx';
import { UserService} from '../user/user.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { ToastrService } from 'ngx-toastr';
import { UserRoleService} from '../user-role/user-role.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss'],
  animations: fuseAnimations
})
export class UserComponent implements OnInit, OnDestroy {
  form: UntypedFormGroup;
  reportingToDDL: IRoleReportingTo[] = [{ id: 1, description: 'Admin' }, { id: 2, description: 'MD' }, { id: 3, description: 'Manager' }, { id: 4, description: 'Ceo' }];
  genderDDl: any[] = [{ 'id': 1, 'description': 'Male' }, { 'id': 2, 'description': 'Female' }];
  isActiveDDl: any = [{ 'id': '', 'description': '' },{ 'id': true, 'description': 'Yes' }, { 'id': false, 'description': 'No' }]
  rowData: any[] = [];
  columnDef: any[] = [];
  color: string = 'primary';
  rolesList: any[] = [];
  roleRowData: any[] = [];
  roleColumnDef: any[] = [];

  //search
  userNameFilter: string;
  codeFilter: string;
  isActiveFilter: boolean;

  constructor(private _formBuilder: UntypedFormBuilder, private userService: UserService, private toastr: ToastrService, private roleService: UserRoleService) {
  }

  ngOnInit(): void {
    // Reactive Form
    this.form = this._formBuilder.group({
      userID: [''],
      userName: ['', Validators.required],
      loginName: [''],
      userPassword:[''],
      code: ['', Validators.required],
      email: ['', Validators.email],
      isd:['+91'],
      mobileNo: ['', Validators.required],
      genderID: ['', Validators.required],
      dateOfBirth: [''],
      fromDate: [''],
      toDate: [''],
      isActive:[''],
      isAgent: [''],
      isAdmin: ['']
    });

    this.columnDef = [{ 'header': 'Name', 'field': 'userName', 'type': 'label', 'width': 250 },
      { 'header': 'Employee ID', 'field': 'code', 'type': 'label', 'width': 80 },
      { 'header': 'Email', 'field': 'email', 'type': 'label', 'width': 250},
      { 'header': 'Mobile', 'field': 'mobileNo', 'type': 'label', 'width': 120 },
      { 'header': 'Date Of Birth', 'field': 'birthDate', 'type': 'label', 'width': 100},
      { 'header': 'Is Active', 'field': 'isActive', 'type': 'label', 'width': 80 },
      { 'header': 'Is Agent', 'field': 'isAgent', 'type': 'label', 'width': 80 },
      { 'header': 'Is Admin', 'field': 'isAdmin', 'type': 'label', 'width': 80 },
      { 'header': '', 'field': '', 'type': 'button', 'activity': ['edit'], 'width': 80 }
    ];
    this.roleColumnDef = [{ 'header': 'Name', 'field': 'name', 'type': 'label'},
      { 'header': 'Select All', 'field': 'isSelected', 'type': 'checkbox', 'checkall': true }
    ]


    this.getUserRoles();
  }

  clear() {
    this.form.reset();
    this.form.get("isd").setValue("+91");
    this.roleRowData = this.rolesList;
  }

  save() {
    if (this.form.valid) {
      let userRoles: any[] = [];
      _.forEach(this.roleRowData, o => {
        if(o.isSelected)
        userRoles.push({ "roleID": o.roleID,"userID":0 });
      });
      let model = this.form.value;
      model.isAdmin = (model.isAdmin ==true) ? true : false;
      model.dateOfBirth = moment(model.dateOfBirth).local().format("DD-MMM-YYYY");
      if (model.userID == "" || model.userID == null ) {
        model.userID = 0;
      }
      let UserVM = {
        'userDto': model,
        'userRolesDto':userRoles
      };
      let isNew: boolean = (model.userID > 0) ? false : true;
      this.userService.save(UserVM, isNew).subscribe(response => {
        this.toastr.success("Saved Succussfully");
        this.getUserById(response);
      })
    }
  }

  /**
     * On destroy
     */
  ngOnDestroy(): void {
  }

  selectedRows(eve) {
    this.getUserById(eve.userID);
    var ele = document.getElementsByClassName('mat-tab-label') as HTMLCollectionOf<HTMLElement>;
    ele[0].click();
  }

  download() {
    const ws: Xlsx.WorkSheet = Xlsx.utils.json_to_sheet(this.rowData);
    const wb: Xlsx.WorkBook = Xlsx.utils.book_new();
    Xlsx.utils.book_append_sheet(wb, ws, "Sheet1");
    Xlsx.writeFile(wb, '.xls');
  }

  tabChanged(eve: MatTabChangeEvent) {
    if (eve.index == 1) {
      this.rowData = [...this.rowData];
    }
    if (eve.index == 0) {
      this.roleRowData = [...this.roleRowData];
    }
  }

  getAllUsers() {
    this.userService.getUsers(this.userNameFilter,this.codeFilter,this.isActiveFilter).subscribe((response) => {
      _.forEach(response, o => {
        o.birthDate = moment(o.dateOfBirth).year() > 2001 ? moment(o.dateOfBirth).local().format("DD-MMM-YYYY") : "";
      });
      this.rowData = response;
    });
  }

  getUserById(userID: number) {
    this.userService.getUserById(userID).subscribe((response) => {
      this.form.patchValue(response.userDto);
      let roles = _.cloneDeep(this.rolesList);
      _.forEach(response.userRolesDto, o => {
        let inx = _.findIndex(roles, r => r.roleID == o.roleID);
        roles[inx].isSelected = true;
      })
      this.roleRowData = roles;
    });
  }

  getUserRoles() {
    this.roleService.getUserRoles().subscribe(response => {
      _.forEach(response, o => {
        o.isSelected = false;
      });
      this.rolesList = _.cloneDeep(response);
      this.roleRowData = response;
    });
  }

  search() {
    this.getAllUsers();
  }

  clearFilter() {
    this.userNameFilter = "";
    this.codeFilter = "";
    this.isActiveFilter = null;
  }
}
