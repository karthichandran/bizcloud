import { Component, OnDestroy, OnInit, ViewChild, ElementRef } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormGroupDirective } from '@angular/forms';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { fuseAnimations } from '@fuse/animations';
import * as Xlsx from 'xlsx';
import { PropertyService } from '../../property/property.service';
import { ToastrService } from 'ngx-toastr';
import * as _ from 'lodash';
import { TaxPaymentReportService } from '../tax-payment-report/tax-payment-report.service';
import * as moment from 'moment';
import * as fileSaver from 'file-saver';

@Component({
  selector: 'tax-payment-report',
  templateUrl: './tax-payment-report.component.html',
  styleUrls: ['./tax-payment-report.component.scss'],
  animations: fuseAnimations
})
export class TaxPaymentReportComponent implements OnInit, OnDestroy {
  reportform: UntypedFormGroup;

  reportRowData: any[] = [];
  reportColumnDef: any[] = [];
  premisesDDl: any[] = [];
  unitNoDDl: any[] = [];
  lotNoDDl: any[] = [];

  constructor(private _formBuilder: UntypedFormBuilder, private taxPaymentSvc: TaxPaymentReportService,  private propertySvc: PropertyService, private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.reportform = this._formBuilder.group({
      lotNo: [''],
      premisesId: [''],
      unitNo: ['']
    });

    this.reportColumnDef = [
      { 'header': 'Lot No', 'field': 'lotNumber', 'type': 'label', 'width': 70 },
      { 'header': 'Property', 'field': 'propertyName', 'type': 'label', 'width': 250 },
      { 'header': 'Unit No', 'field': 'unitNo', 'type': 'label', 'width': 70 },
      { 'header': 'Customer Name', 'field': 'customerName', 'type': 'label', 'width': 150 },
      { 'header': 'Name as per the TDS paid Challan', 'field': 'nameInChallan', 'type': 'label', 'width': 210 },
      { 'header': 'Payment Date', 'field': 'date', 'type': 'label', 'width': 140 },
      { 'header': 'Challan No', 'field': 'challanSerialNo', 'type': 'label', 'width': 110 },
      { 'header': 'Challan I.T Amt', 'field': 'challanIncomeTaxAmount', 'type': 'label', 'width': 140 },
      { 'header': 'Challan Interest Amt', 'field': 'challanInterestAmount', 'type': 'label', 'width': 170 },
      { 'header': 'Challan Fee Amt', 'field': 'challanFeeAmount', 'type': 'label', 'width': 160 },
      { 'header': 'Challan Total Amt', 'field': 'challanTotalAmount', 'type': 'label', 'width': 200 }
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
    this.taxPaymentSvc.getLotNo().subscribe(response => {
      var orderedLot = _.orderBy(response, ['lotNo'], ['asc']);
      this.lotNoDDl = orderedLot;
      this.lotNoDDl.splice(0, 0, { 'lotNo': '' });
    });
  }

  getReportList() {
    var filters = this.reportform.value;

    this.taxPaymentSvc.getReportList( filters.premisesId,  filters.unitNo, filters.lotNo).subscribe(response => {
      _.forEach(response, obj => {
        obj.date = obj.challanPaymentDate == null ? "" : moment(obj.challanPaymentDate).local().format("DD-MMM-YYYY");
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

    this.taxPaymentSvc.downloadtoExcel( filters.premisesId,  filters.unitNo, filters.lotNo).subscribe(response => {
      let blob: any = new Blob([response], { type: 'application/vnd.ms-excel' });
      fileSaver.saveAs(blob, 'Tax_Payment_Report.xls');
    });
  }

  search() {
    this.getReportList();
  }

  reset() {
    this.reportform.reset();
  }


}
