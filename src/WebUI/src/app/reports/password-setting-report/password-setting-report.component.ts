import { Component, OnDestroy, OnInit, ViewChild, ElementRef } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormGroupDirective } from '@angular/forms';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { fuseAnimations } from '@fuse/animations';
import * as Xlsx from 'xlsx';
import { PropertyService } from '../../property/property.service';
import { ToastrService } from 'ngx-toastr';
import * as _ from 'lodash';
import { PasswordSettingReportService } from '../password-setting-report/password-setting-report.service';
import * as moment from 'moment';
import * as fileSaver from 'file-saver';

@Component({
  selector: 'password-setting-report',
  templateUrl: './password-setting-report.component.html',
  styleUrls: ['./password-setting-report.component.scss'],
  animations: fuseAnimations
})
export class PasswordSettingReportComponent implements OnInit, OnDestroy {
  reportform: UntypedFormGroup;

  reportRowData: any[] = [];
  reportColumnDef: any[] = [];
  premisesDDl: any[] = [];
  unitNoDDl: any[] = [];
  lotNoDDl: any[] = [];

  constructor(private _formBuilder: UntypedFormBuilder, private tracesReportSvc: PasswordSettingReportService,  private propertySvc: PropertyService, private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.reportform = this._formBuilder.group({
      lotNo: [''],
      premisesId: [''],
      unitNo: ['']
    });

    this.reportColumnDef = [
      { 'header': 'Lot No', 'field': 'lotNumber', 'type': 'label', 'width': 70 },
      { 'header': 'Unit No', 'field': 'unitNo', 'type': 'label', 'width': 70 },
      { 'header': 'Premises', 'field': 'propertyName', 'type': 'label', 'width': 150 },
      { 'header': 'Traces Pwd', 'field': 'hasTracesPassword', 'type': 'label', 'width': 120 },
      { 'header': 'PAN', 'field': 'pan', 'type': 'label', 'width': 120 },
      { 'header': 'D.O.B', 'field': 'date', 'type': 'label', 'width': 130 },
      { 'header': 'Name as per the TDS paid Challan', 'field': 'nameInChallan', 'type': 'label', 'width': 210 },
      { 'header': 'Name as per System', 'field': 'nameInSystem', 'type': 'label', 'width': 200 },
      { 'header': 'Challan Serial No', 'field': 'challanSerialNo', 'type': 'label', 'width': 160 },
      { 'header': 'Challan Total Amt', 'field': 'challanTotalAmount', 'type': 'label', 'width': 160 },
      { 'header': 'Address Premises', 'field': 'addressPremises', 'type': 'label', 'width': 250 },
      { 'header': 'Address 1', 'field': 'address1', 'type': 'label', 'width': 250 },
      { 'header': 'Address 2', 'field': 'address2', 'type': 'label', 'width': 250 },
      { 'header': 'City', 'field': 'city', 'type': 'label', 'width': 150 },
      { 'header': 'PinCode', 'field': 'pincode', 'type': 'label', 'width': 100 }
    ];

    this.getProperties();
   // this.getReportList();
    this.getLotNo();
  }

  getProperties() {
    this.propertySvc.getProperties().subscribe(response => {
      var orderedRes = _.orderBy(response, ['addressPremises'], ['asc']);
         
      this.premisesDDl = orderedRes;
      this.premisesDDl.splice(0, 0, { 'propertyID': '', 'addressPremises': '' });
    });
  }

  getLotNo() {
    this.tracesReportSvc.getLotNo().subscribe(response => {
      var orderedLot = _.orderBy(response, ['lotNo'], ['asc']);
      this.lotNoDDl = orderedLot;
      this.lotNoDDl.splice(0, 0, { 'lotNo': '' });
    });
  }

  getReportList() {
    var filters = this.reportform.value;

    this.tracesReportSvc.getReportList( filters.premisesId,  filters.unitNo, filters.lotNo).subscribe(response => {
      _.forEach(response, obj => {
        obj.date = obj.dateOfBirth == null ? "" : moment(obj.dateOfBirth).local().format("DD-MMM-YYYY");
      });
      this.reportRowData = response;
    });
  }

  /**
     * On destroy
     */
  ngOnDestroy(): void {
  }

  downloadExcel() {
    var filters = this.reportform.value;

    this.tracesReportSvc.downloadtoExcel( filters.premisesId,  filters.unitNo, filters.lotNo).subscribe(response => {
      let blob: any = new Blob([response], { type: 'application/vnd.ms-excel' });
      fileSaver.saveAs(blob, 'Password_Setting_Report.xls');
    });
  }

  search() {
    this.getReportList();
  }

  reset() {
    this.reportform.reset();
  }


}
