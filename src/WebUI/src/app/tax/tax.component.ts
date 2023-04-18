import { Component, OnDestroy, OnInit, ViewChild, ElementRef } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { fuseAnimations } from '@fuse/animations';
import { IRoleReportingTo } from '../models/RoleReportingTo';
import * as Xlsx from 'xlsx';
import { ToastrService } from 'ngx-toastr';
import * as _moment from 'moment';
import { TaxService } from './tax.service';
import { MatTabChangeEvent } from '@angular/material/tabs';
import * as _ from 'lodash';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { CopyTaxDialogComponent} from './copy-tax-dialog/copy-tax-dialog.component';
import * as moment from 'moment';
@Component({
  selector: 'app-tax',
  templateUrl: './tax.component.html',
  styleUrls: ['./tax.component.scss'],
  animations: fuseAnimations
})
export class TaxComponent implements OnInit {
  taxForm: UntypedFormGroup;

  taxCode: any;

  taxTypes: any[] = [{ 'taxTypeId': 1, 'taxTypeDesc': 'GST' }, { 'taxTypeId': 2, 'taxTypeDesc': 'TDS' }];
  taxTypesDDl: any[] = [{ 'taxTypeId': '', 'taxTypeDesc': '' },{ 'taxTypeId': 1, 'taxTypeDesc': 'GST' }, { 'taxTypeId': 2, 'taxTypeDesc': 'TDS' }];
  rowData: any[] = [];
  columnDef: any[] = [];

  //search
  searchByTaxName: string;
  searchByTaxType: string;

  showListGrid: boolean;
  showButtons: boolean=true;

  constructor(private _formBuilder: UntypedFormBuilder, private toastr: ToastrService, private taxService: TaxService, private dialog: MatDialog) {
  }

  ngOnInit(): void {
    // Reactive Form
    this.taxForm = this._formBuilder.group({
      taxID:[''],
      taxCodeId: [{ value: '', disabled: true }],
      taxTypeId: [1, Validators.required],
      taxName: ['', Validators.required],
      taxValue: ['', Validators.required],
      effectiveStartDate: ['', Validators.required],
      effectiveEndDate: ['', Validators.required]
    });

    this.columnDef = [{ 'header': 'Name', 'field': 'taxName', 'type': 'label' },
    { 'header': 'Tax Value', 'field': 'taxValue', 'type': 'label' },
    { 'header': 'Type', 'field': 'taxType', 'type': 'label' },
      { 'header': 'Effective Date', 'field': 'effectiveDate', 'type': 'label' },
      { 'header': '', 'field': '', 'type': 'button', 'activity': [ 'edit'] } 
    ];

    this.rowData = [];
  }

  clear() {
    this.taxForm.reset();
  }

  save() {
    if (this.taxForm.valid) {
      let model = this.taxForm.value;
      let taxCodeId = this.taxForm.get('taxCodeId').value;
      model.taxCodeId = (taxCodeId == "" || taxCodeId == null) ? 0 : taxCodeId;
      if (model.taxID == "" || model.taxID == null)
        model.taxID = 0;

      model.effectiveStartDate = _moment(model.effectiveStartDate).local().format("YYYY-MM-DD");
      model.effectiveEndDate = _moment(model.effectiveEndDate).local().format("YYYY-MM-DD");
      this.taxService.saveTax(model, model.taxCodeId).subscribe(response => {
        this.toastr.success("Tax Saved Successfully");
        this.taxForm.get('taxTypeId').reset();// Tax Type resetting
        this.getTaxCodeById(response);
        
      });
     
    }
  }


  tabChanged(eve: MatTabChangeEvent) {
    if (eve.index == 1) {
      this.search();
      this.showListGrid = true;
    }
    else
      this.showListGrid = false;
  }

  editRow(eve) {
    this.getTaxCodeByIdAndNavigate(eve.taxID);
  }

  deleteRow(eve) {
    this.taxService.deleteTaxCode(eve.taxID).subscribe(response => {
      this.toastr.success("Tax Code is Deleted successfully");
      this.search();
    });
  }

  download() {
    const ws: Xlsx.WorkSheet = Xlsx.utils.json_to_sheet(this.rowData);
    const wb: Xlsx.WorkBook = Xlsx.utils.book_new();
    Xlsx.utils.book_append_sheet(wb, ws, "Sheet1");
    Xlsx.writeFile(wb, '.xls');
  }

  search() {
    this.getAllTaxCodes(this.searchByTaxName, this.searchByTaxType);
  }

  getAllTaxCodes(taxName: string, taxType: string) {
    this.taxService.getTaxCodesByQuery(taxName, taxType).subscribe(response => {
      _.forEach(response, o => {
        o.taxType = o.taxTypeId == 1 ? 'GST' : 'TDS';
        o.effectiveDate = _moment(o.effectiveStartDate).format("DD MMM YYYY") + " - " + _moment(o.effectiveEndDate).format("DD MMM YYYY");
      });
      let ordered = _.orderBy(response,[ 'taxCodeId','effectiveEndDate']);

      this.rowData = ordered;
    });
  }

  getTaxCodeByIdAndNavigate(id: string) {
    this.taxForm.reset();
    this.taxService.getTaxCodesByTaxId(id).subscribe(response => {     
      this.taxCode = response;     
      this.taxForm.patchValue(response);
      if (moment(response.effectiveEndDate) < moment()) {
        this.showButtons = false;
      }
      else
        this.showButtons = true;

      var ele = document.getElementsByClassName('mat-tab-label') as HTMLCollectionOf<HTMLElement>;
      ele[0].click();
    });
  }

  getTaxCodeById(id: string) {
    this.taxService.getTaxCodesByTaxId(id).subscribe(response => {
      this.taxCode = response;
      this.taxForm.patchValue(response);     
    });
  }

  reset() {
    this.searchByTaxName = "";
    this.searchByTaxType = "";
    this.search();
  }

  openDialog(): void {
    let model = _.cloneDeep(this.taxCode);
    model.effectiveStartDate = _moment().toDate();
    const dialogRef = this.dialog.open(CopyTaxDialogComponent, {
      hasBackdrop: false,
      maxHeight: 650,
      maxWidth: 1000,
      width: "800px",
      data: {
        'formData': model
      }
    });

    dialogRef.afterClosed().subscribe(() => {
      var ele = document.getElementsByClassName('mat-tab-label') as HTMLCollectionOf<HTMLElement>;
      ele[1].click();
      this.search();
      console.log('The dialog was closed');
    });
  }

  copy() {
    this.openDialog();
  }
}
