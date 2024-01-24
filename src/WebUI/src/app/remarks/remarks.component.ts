import { Component, OnDestroy, OnInit, ViewChild, ElementRef } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { fuseAnimations } from '@fuse/animations';
import { IRoleReportingTo } from '../models/RoleReportingTo';
import * as Xlsx from 'xlsx';
import { ToastrService } from 'ngx-toastr';
import * as _moment from 'moment';
import { RemarkService } from './remarks.service';
import { MatTabChangeEvent } from '@angular/material/tabs';
import * as _ from 'lodash';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import * as moment from 'moment';
import { filter } from 'rxjs/operators';
@Component({
  selector: 'app-remarks',
  templateUrl: './remarks.component.html',
  styleUrls: ['./remarks.component.scss'],
  animations: fuseAnimations
})
export class RemarksComponent implements OnInit {
  remarkForm: UntypedFormGroup;


  RemarksTypesDDl: any[] = [{ 'remarkTypeId':0 , 'remarkTypeDesc': '' },{ 'remarkTypeId': 1, 'remarkTypeDesc': 'Remittance' }, { 'remarkTypeId': 2, 'remarkTypeDesc': 'Traces' }];
  rowData: any[] = [];
  columnDef: any[] = [];

  //search
  searchByRemarkType: string;

  showListGrid: boolean;
  showButtons: boolean=true;

  constructor(private _formBuilder: UntypedFormBuilder, private toastr: ToastrService, private remarksService: RemarkService, private dialog: MatDialog) {
  }

  ngOnInit(): void {
    // Reactive Form
    this.remarkForm = this._formBuilder.group({
      remarkId:[''],
      description: ['', Validators.required],
      isRemittance: [true, Validators.required],     
    });

    this.columnDef = [{ 'header': 'Remark', 'field': 'description', 'type': 'label' },
    { 'header': 'Type', 'field': 'type', 'type': 'label' }  ,
    { 'header': '', 'field': '', 'type': 'button', 'activity': ['edit','delete'], 'maxWidth': 80 } 
    ];

    this.rowData = [];
  }

  clear() {
    this.remarkForm.reset();
    this.remarkForm.get('isRemittance').setValue(true);
  }

  save() {
    if (this.remarkForm.valid) {
      let model = this.remarkForm.value;
      let remarkId = this.remarkForm.get('remarkId').value;
      model.remarkId = (remarkId == "" || remarkId == null) ? 0 : remarkId;

      this.remarksService.save(model, model.remarkId).subscribe(response => {
        this.toastr.success("Remark Saved Successfully");
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
    this.getRemarkByIdAndNavigate(eve.remarkId);
  }

  deleteRow(eve) {
    this.remarksService.deleteRemark(eve.remarkId).subscribe(response => {
      this.toastr.success("Remark is Deleted successfully");
      this.search();
    });
  }

//   download() {
//     const ws: Xlsx.WorkSheet = Xlsx.utils.json_to_sheet(this.rowData);
//     const wb: Xlsx.WorkBook = Xlsx.utils.book_new();
//     Xlsx.utils.book_append_sheet(wb, ws, "Sheet1");
//     Xlsx.writeFile(wb, '.xls');
//   }

  search() {
    this.getRemarks();
  }

  getRemarks() {
    var filter="";    
    if(this.searchByRemarkType=="1" ||this.searchByRemarkType=="2")
    filter=this.searchByRemarkType=="1"?"true":"false";
    this.remarksService.getRemarks(filter).subscribe(response => {
        _.forEach(response, o => {
            o.type = o.isRemittance == true ? 'Remittance' : 'Traces';
          });
      this.rowData = response;
    });
  }

  getRemarkByIdAndNavigate(id: string) {
    this.remarkForm.reset();
    this.remarksService.getRemarkById(id).subscribe(response => {     
      this.remarkForm.patchValue(response);
    

      var ele = document.getElementsByClassName('mat-tab-label') as HTMLCollectionOf<HTMLElement>;
      ele[0].click();
    });
  }

  reset() {
    this.searchByRemarkType = "";
    this.search();
  }

   
}
