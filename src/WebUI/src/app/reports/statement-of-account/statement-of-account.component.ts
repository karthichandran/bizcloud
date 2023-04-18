import { Component, OnDestroy, OnInit, ViewChild, ElementRef } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormGroupDirective } from '@angular/forms';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { fuseAnimations } from '@fuse/animations';
import * as Xlsx from 'xlsx';
import { PropertyService } from '../../property/property.service';
import { ToastrService } from 'ngx-toastr';
import * as _ from 'lodash';
import {StatementOfAccountService  } from './statement-of-account.service' ;
import * as moment from 'moment';
import * as fileSaver from 'file-saver';

@Component({
  selector: 'statement-of-account',
  templateUrl: './statement-of-account.component.html',
  styleUrls: ['./statement-of-account.component.scss'],
  animations: fuseAnimations
})
export class StatementOfAccountComponent implements OnInit, OnDestroy {
  reportform: UntypedFormGroup;

  reportRowData: any[] = [];
  reportColumnDef: any[] = [];
  premisesDDl: any[] = [];
  unitNoDDl: any[] = [];

  payableTds: number=0;
  payableInterest: number=0;
  payableLateFee: number=0;
  payableServiceFee: number=0;
  payableTotal: number=0;

  receivedTds: number=0;
  receivedInterest: number=0;
  receivedLateFee: number=0;
  receivedServiceFee: number=0;
  receivedTotal: number=0;

  remittedTds: number=0;
  remittedInterest: number=0;
  remittedLateFee: number=0;
  remittedTotal: number=0;

  balanceTds: number=0;
  balanceInterest: number=0;
  balanceLateFee: number=0;
  balanceServiceFee: number=0;
  balanceTotal: number=0;


  constructor(private _formBuilder: UntypedFormBuilder, private statementAccountReportSvc: StatementOfAccountService, private propertySvc: PropertyService, private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.reportform = this._formBuilder.group({
      premisesId: [''],
      unitNo: [''],
      customerName: ['']
    });

    this.reportColumnDef = [
      { 'header': 'Unit No', 'field': 'unitNo', 'type': 'label', 'headerClass': 'header-yellow-color', 'headerFontClass':'header-font-color','width': 100},
      { 'header': 'Date of Payment', 'field': 'paymentDate', 'type': 'label', 'headerClass': 'header-yellow-color', 'headerFontClass': 'header-font-color', 'width': 140},
      { 'header': 'Amount Paid', 'field': 'payableAmountPaid', 'type': 'label', 'headerClass': 'header-yellow-color', 'headerFontClass': 'header-font-color', 'width': 110 },
      { 'header': 'Gross Amount', 'field': 'payableGrossAmount', 'type': 'label', 'headerClass': 'header-yellow-color', 'headerFontClass': 'header-font-color', 'width': 140 },
      { 'header': 'TDS', 'field': 'payableTds', 'type': 'label', 'headerClass': 'header-yellow-color', 'headerFontClass': 'header-font-color', 'width': 100},
      { 'header': 'Interest', 'field': 'payableInterest', 'type': 'label', 'headerClass': 'header-yellow-color', 'headerFontClass': 'header-font-color', 'width': 100},
      { 'header': 'Late Fee', 'field': 'payableLateFee', 'type': 'label', 'headerClass': 'header-yellow-color', 'headerFontClass': 'header-font-color', 'width': 100 },
      { 'header': 'Service Charge', 'field': 'payableServiceFee', 'type': 'label', 'headerClass': 'header-yellow-color', 'headerFontClass': 'header-font-color', 'width': 130},
      { 'header': 'Date', 'field': 'dateOfReceived', 'type': 'label', 'headerClass': 'header-lightsalmon-color', 'headerFontClass': 'header-font-color', 'width': 140},
      { 'header': 'Total Amount Received', 'field': 'receivedTotalAmount', 'type': 'label', 'headerClass': 'header-lightsalmon-color', 'headerFontClass': 'header-font-color', 'width': 180},
      { 'header': 'TDS', 'field': 'receivedTds', 'type': 'label', 'headerClass': 'header-lightsalmon-color', 'headerFontClass': 'header-font-color', 'width': 100},
      { 'header': 'Interest', 'field': 'receivedInterest', 'type': 'label', 'headerClass': 'header-lightsalmon-color', 'headerFontClass': 'header-font-color', 'width': 90},
      { 'header': 'Late Fee', 'field': 'receivedLateFee', 'type': 'label', 'headerClass': 'header-lightsalmon-color', 'headerFontClass': 'header-font-color', 'width': 90},
      { 'header': 'Service Charge', 'field': 'receivedServiceCharge', 'type': 'label', 'headerClass': 'header-lightsalmon-color', 'headerFontClass': 'header-font-color','width': 150},
      { 'header': 'Date', 'field': 'dateOfRemitted', 'type': 'label', 'headerClass': 'header-green-color', 'headerFontClass': 'header-font-color', 'width': 140},
      { 'header': 'TDS', 'field': 'remittedTds', 'type': 'label', 'headerClass': 'header-green-color', 'headerFontClass': 'header-font-color', 'width': 100},
      { 'header': 'Interest', 'field': 'remittedInterest', 'type': 'label', 'headerClass': 'header-green-color', 'headerFontClass': 'header-font-color', 'width': 100 },
      { 'header': 'Late Fee', 'field': 'remittedLateFee', 'type': 'label', 'headerClass': 'header-green-color', 'headerFontClass': 'header-font-color', 'width': 100 }
    ];

    this.getProperties();
   // this.getReportList();
  }

  getProperties() {
    this.propertySvc.getProperties().subscribe(response => {
      this.premisesDDl = response;
      this.premisesDDl.splice(0, 0, { 'propertyID': '', 'addressPremises': '' });
    });
  } 

  getReportList() {
    var filters = this.reportform.value;

    this.statementAccountReportSvc.getReportList(filters.customerName, filters.premisesId, filters.unitNo).subscribe(response => {
      _.forEach(response, obj => {
        obj.paymentDate = obj.payableDateOfPayment == null ? "" : moment(obj.payableDateOfPayment).local().format("DD-MMM-YYYY");
        obj.dateOfReceived = obj.receivedDate == null ? "" : moment(obj.receivedDate).local().format("DD-MMM-YYYY");
        obj.dateOfRemitted = obj.remittedDate == null ? "" : moment(obj.remittedDate).local().format("DD-MMM-YYYY");
      });
      //let grouped = _.groupBy(response, o => o.ownershipID);

      let ordered = _.orderBy(response, o => o.ownershipID);
      
      this.reportRowData = ordered;
      this.calculateSummary(ordered);
    });
  }

  calculateSummary(report:any) {
    let payTds = 0;
    let payInterest = 0;
    let payLateFee = 0;
    let payServiceFee = 0;
    let recTds = 0;
    let recInterest = 0;
    let recServiceFee = 0;
    let recLateFee = 0;
    let remTds = 0;
    let remInterest = 0;
    let remLateFee = 0;
   
    _.forEach(report, function (obj) {
      payTds = payTds + obj.payableTds;
      payInterest = payInterest + obj.payableInterest;
      payLateFee = payLateFee + obj.payableLateFee;
      payServiceFee = payServiceFee + obj.payableServiceFee;

      recTds = recTds + obj.receivedTds;
      recInterest = recInterest + obj.receivedInterest;
      recServiceFee = recServiceFee + obj.receivedServiceCharge;
      recLateFee = recLateFee + obj.receivedLateFee;

      remTds = remTds + obj.remittedTds;
      remInterest = remInterest + obj.remittedInterest;
      remLateFee = remLateFee + obj.remittedLateFee;
    });

    this.payableTds =parseFloat(payTds.toFixed(2));
    this.payableInterest = parseFloat(payInterest.toFixed(2));
    this.payableLateFee = parseFloat(payLateFee.toFixed(2));
    this.payableServiceFee = parseFloat(payServiceFee.toFixed(2));
    this.receivedTds = parseFloat(recTds.toFixed(2));
    this.receivedInterest = parseFloat(recInterest.toFixed(2));
    this.receivedServiceFee = parseFloat(recServiceFee.toFixed(2));
    this.receivedLateFee = parseFloat(recLateFee.toFixed(2));
    this.remittedTds = parseFloat(remTds.toFixed(2));
    this.remittedInterest = parseFloat(remInterest.toFixed(2));
    this.remittedLateFee = parseFloat(remLateFee.toFixed(2));   

    this.balanceTds = parseFloat((this.payableTds - this.remittedTds).toFixed(2));
    this.balanceInterest = parseFloat((this.payableInterest - this.remittedInterest).toFixed(2));
    this.balanceLateFee = parseFloat((this.payableLateFee - this.remittedLateFee).toFixed(2));
    this.balanceServiceFee = parseFloat((this.payableServiceFee - this.receivedServiceFee).toFixed(2));

    this.payableTotal = parseFloat((this.payableTds + this.payableInterest).toFixed(2));
    this.receivedTotal = parseFloat((this.receivedTds + this.receivedInterest).toFixed(2));
    this.remittedTotal = parseFloat((this.remittedTds + this.remittedInterest).toFixed(2));
    this.balanceTotal = parseFloat((this.balanceTds + this.balanceInterest).toFixed(2));
  }
  /**
     * On destroy
     */
  ngOnDestroy(): void {
  }

  downloadExcel() {
    var filters = this.reportform.value;

    this.statementAccountReportSvc.downloadtoExcel(filters.customerName, filters.premisesId, filters.unitNo, filters.lotNo).subscribe(response => {
      let blob: any = new Blob([response], { type: 'application/vnd.ms-excel' });
      fileSaver.saveAs(blob, 'StatementOfAccountReport.xls');
    });
  }

  search() {
    this.getReportList();
  }

  reset() {
    this.reportform.reset();
  }


}
