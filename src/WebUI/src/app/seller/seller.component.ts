import { Component, OnDestroy, OnInit, ViewChild, ElementRef } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormGroupDirective, ValidatorFn, AbstractControl} from '@angular/forms';
import { fuseAnimations } from '@fuse/animations';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { IRoleReportingTo } from '../models/RoleReportingTo';
import { SellerService } from './seller.service';
import { StatesService} from '../shared/services/states.service'
import * as Xlsx from 'xlsx';
import * as moment from 'moment';
import {StateDto } from '../ReProServices-api';
import { ToastrService } from 'ngx-toastr';
import { ConfirmationDialogService } from '../core/confirmation-dialog/confirmation-dialog.service';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-seller',
  templateUrl: './seller.component.html',
  styleUrls: ['./seller.component.scss'],
  animations: fuseAnimations
})
export class SellerComponent implements OnInit, OnDestroy {
  form: UntypedFormGroup;
  sellers: any[] = [];
  states: any[] = [];
  rowData: any[] = [];
  columnDef: any[] = [];
  searchBySellerName: string;
  searchBySellerPan: string;
  searchByMobile: string;
  showListGrid: boolean;
  isRadioButtonTouched: boolean = true;

  constructor(private _formBuilder: UntypedFormBuilder, private sellerService: SellerService, private statesService: StatesService, private toastr: ToastrService,
    private confirmationDialogSrv: ConfirmationDialogService) {

  }

  ngOnInit(): void {
    // Reactive Form
    this.form = this._formBuilder.group({
      sellerID:[''],
      sellerName: ['', Validators.required],
      addressPremises: [''],
      adressLine1: ['', Validators.required],
      addressLine2: [''],
      city: ['', Validators.required],
      stateID: ['', Validators.required],
      pinCode: ['', Validators.required],
      residency: ['resident', Validators.required],     
      pan: ['', [Validators.required,this.panValidator()]],
      emailID: ['', Validators.required],
      mobileNo: ['', Validators.required]
     
    });
    
    this.columnDef = [
      { 'header': 'Name', 'field': 'sellerName', 'type': 'label' },
      { 'header': 'Email', 'field': 'emailID', 'type': 'label' },
      { 'header': 'Pan', 'field': 'pan', 'type': 'label' },
      { 'header': 'Mobile', 'field': 'mobileNo', 'type': 'label' },
      { 'header': '', 'field': '', 'type': 'button', 'activity': ['edit'], 'maxWidth': 80 }
      //{ 'header': '', 'field': '', 'type': 'button', 'activity': ['delete'], 'maxWidth': 80 }    
    ];

    this.getAllStates();
  }
  IsduplicateSeller(): Observable<any> {
    return this.sellerService.getSellersByFilter('', this.form.value.pan, '');
  }

  save() {       
    if (this.form.valid) {
      let model = this.form.value;
      if (model.sellerID == "" || model.sellerID == null) {
        this.IsduplicateSeller().subscribe(res => {
          if (res == null || res == "") {
            this.saveSeller();
          }
          else
            this.toastr.error("Seller is already existm ");
        });
      }
      else
        this.saveSeller();
    }
    else {
      Object.keys(this.form.controls).forEach(field => {
        const control = this.form.get(field);
        control.markAsTouched({ onlySelf: true });
      });
    }
  }

  saveSeller() {
    let model = this.form.value;
    if (model.sellerID == "" || model.sellerID == null)
      model.sellerID = 0;
    if (this.form.value.residency == 'resident')
      model.isResident = true;
    else
      model.isResident = false;
    this.sellerService.saveSeller(model).subscribe((response) => {
      this.toastr.success("Saved Successfully");
      this.clear();
    }, error => this.toastr.error(error));
  }

  clear() {
    this.form.reset();
    this.form.patchValue({ 'residency': 'resident' });
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

  download() {
    const ws: Xlsx.WorkSheet = Xlsx.utils.json_to_sheet(this.rowData);
    const wb: Xlsx.WorkBook = Xlsx.utils.book_new();
    Xlsx.utils.book_append_sheet(wb, ws, "Sheet1");
    Xlsx.writeFile(wb, '.xls');
  }

  showSeller(model:any) {
    this.form.patchValue(model);
  }

  getAllSellers() {
    this.sellerService.getSellers().subscribe((response) => {
      this.rowData = response;
    });
  }

  getAllStates() {
    this.statesService.getStates().subscribe((response) => {
      this.states = response;
    });
  }

  addCoSeller() {
    var selle: any = [];
    this.sellerService.getSellerEntry("1").subscribe((response) => {
      selle = response;
    });

    if (this.form.valid) {
      this.sellers = [this.form.value, ...this.sellers];
    
      this.form.reset();

    }
    else {
      if (this.form.get("status").value)
        this.isRadioButtonTouched = true;
      else
        this.isRadioButtonTouched = false;

      Object.keys(this.form.controls).forEach(field => { 
        const control = this.form.get(field);          
        control.markAsTouched({ onlySelf: true });
      });
      //this.form.markAsTouched({ onlySelf: true });
     // this.form.markAllAsTouched();
    }
  }

  panValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      // if input field is empty return as valid else test
      const ret = (control.value !== '') ? new RegExp('^[A-Za-z]{5}[0-9]{4}[A-Za-z]$').test(control.value) : true;
      return !ret ? { 'invalidNumber': { value: control.value } } : null;
    };
  }


  tabChanged(eve: MatTabChangeEvent) {
    //console.log('tabChangeEvent => ', eve);
    //console.log('index => ', eve.index);
    if (eve.index == 1) {
      this.getAllSellers();
      this.showListGrid = true;
    }
    else
      this.showListGrid = false;
  }

  search() {
    
    this.sellerService.getSellersByFilter(this.searchBySellerName, this.searchBySellerPan, this.searchByMobile).subscribe((response) => {
        this.rowData = response;
      });   

  }

  reset() {
    this.searchByMobile = "";
    this.searchBySellerPan = "";
    this.searchBySellerName = "";
    this.search();
  }

  editRow(eve) {
    let model = eve;
    if (model.isResident)
      model.residency = 'resident';
    else
      model.residency = 'nonResident';
    this.form.patchValue(model);
    var ele = document.getElementsByClassName('mat-tab-label') as HTMLCollectionOf<HTMLElement>;
    ele[0].click();
  }

  deleteRow(eve) {
    this.confirmationDialogSrv.showDialog("Are you sure to delete?").subscribe(response => {
      if (response == "ok") {
        this.sellerService.deleteSeller(eve.sellerID);
      }
    });
  }
}
