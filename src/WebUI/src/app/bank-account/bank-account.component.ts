import { Component, OnDestroy, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormGroupDirective, ValidatorFn, AbstractControl } from '@angular/forms';
import { fuseAnimations } from '@fuse/animations';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { BankAccountService } from './bank-account.service';
import * as moment from 'moment';
import { ToastrService } from 'ngx-toastr';
import { ConfirmationDialogService } from '../core/confirmation-dialog/confirmation-dialog.service';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-bank-account',
  templateUrl: './bank-account.component.html',
  styleUrls: ['./bank-account.component.scss'],
  animations: fuseAnimations
})
export class BankAccountComponent implements OnInit, OnDestroy {
  form: FormGroup;
  rowData: any[] = [];
  columnDef: any[] = [];
  searchByUserName: string;
  showListGrid: boolean;

  constructor(private _formBuilder: FormBuilder, private acctSvc: BankAccountService,  private toastr: ToastrService,
    private confirmationDialogSrv: ConfirmationDialogService) {

  }

  ngOnInit(): void {
    // Reactive Form
    this.form = this._formBuilder.group({
      accountId: [''],
      userName: ['', Validators.required],
      userPassword: ['', Validators.required],
      bankName:[''],
      laneNo:[''],
      letterA: [''],
      letterB: [''],
      letterC: [''],
      letterD: [''],
      letterE: [''],
      letterF: [''],
      letterG: [''],
      letterH: [''],
      letterI: [''],
      letterJ: [''],
      letterK: [''],
      letterL: [''],
      letterM: [''],
      letterN: [''],
      letterO: [''],
      letterP: ['']
    });

    this.columnDef = [
      { 'header': 'User Name', 'field': 'userName', 'type': 'label' },
      { 'header': 'Bank Name', 'field': 'bankName', 'type': 'label' },
      { 'header': 'Lane No', 'field': 'laneNo', 'type': 'label' },
      //{ 'header': 'Password', 'field': 'userPassword', 'type': 'label' },
      { 'header': '', 'field': '', 'type': 'button', 'activity': ['edit'], 'maxWidth': 80 }
    ];

  }


  save() {
            
        this.saveAccount();   
  }

  saveAccount() {
    let model = this.form.value;
    if (model.accountId == "" || model.accountId == null)
      model.accountId = 0;
  
    this.acctSvc.saveAccount(model).subscribe((response) => {
      this.toastr.success("Saved Successfully");
      this.clear();
    }, error => this.toastr.error(error));
  }

  clear() {
    this.form.reset();
  }
  /**
     * On destroy
     */
  ngOnDestroy(): void {
  }

  selectedRows(eve) {
    //let model = eve.selected[0];
    //if (model.isResident)
    //  model.residency = 'resident';
    //else
    //  model.residency = 'nonResident';
    //this.form.patchValue(model);
    //var ele = document.getElementsByClassName('mat-tab-label') as HTMLCollectionOf<HTMLElement>;
    //ele[0].click();
  }


  showSeller(model: any) {
    this.form.patchValue(model);
  }

  

  tabChanged(eve: MatTabChangeEvent) {
    //console.log('tabChangeEvent => ', eve);
    //console.log('index => ', eve.index);
    if (eve.index == 1) {
      this.search();
      this.showListGrid = true;
    }
    else
      this.showListGrid = false;
  }

  search() {
      this.acctSvc.getAccountList(this.searchByUserName).subscribe((response) => {
      this.rowData = response;
    });

  }

  reset() {
    this.searchByUserName = "";
    this.search();
  }

  editRow(eve) {
    let model = eve;
    this.form.patchValue(model);
    var ele = document.getElementsByClassName('mat-tab-label') as HTMLCollectionOf<HTMLElement>;
    ele[0].click();
  }

  deleteUser(accountId) {
    this.confirmationDialogSrv.showDialog("Are you sure to delete?").subscribe(response => {
      if (response == "ok") {
        this.acctSvc.deleteAccount(accountId).subscribe((response) => {
          this.toastr.success("Deleted Successfully");
          this.clear();
        }, error => this.toastr.error(error));;
      }
    });
  }
}
