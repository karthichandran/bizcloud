import { Component, OnDestroy, OnInit, ViewChild, ElementRef } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormGroupDirective } from '@angular/forms';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { fuseAnimations } from '@fuse/animations';
import * as Xlsx from 'xlsx';
import { PropertyService } from '../../property/property.service';
import { ToastrService } from 'ngx-toastr';
import * as _ from 'lodash';
import { LotSummaryReportService} from '../lot-summary/lot-summary-report.service';
import { SellerService } from '../../seller/seller.service';
import * as fileSaver from 'file-saver';

@Component({
  selector: 'seller-compliance-report',
  templateUrl: './lot-summary-report.component.html',
  styleUrls: ['./lot-summary-report.component.scss'],
  animations: fuseAnimations
})
export class LotSummaryReportComponent implements OnInit, OnDestroy {
  reportform: UntypedFormGroup;

  reportRowData: any[] = [];
  reportColumnDef: any[] = [];
  premisesDDl: any[] = [];
  unitNoDDl: any[] = [];
  sellerDDl: any[] = [];
  lotNoDDl: any[] = [];

  constructor(private _formBuilder: UntypedFormBuilder, private lotSummarySvc: LotSummaryReportService, private sellerSvc: SellerService, private propertySvc: PropertyService, private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.reportform = this._formBuilder.group({
      lotNo: [''],
      premisesId: [''],
      unitNo: [''],
      customerName: [''],
      sellerId: ['']
    });

    this.reportColumnDef = [
      { 'header': 'Lot No', 'field': 'lotNo', 'type': 'label', 'width': 80 },      
      { 'header': 'Total No of Units', 'field': 'totalPayments', 'type': 'label', 'width': 140 },
      { 'header': 'No.of Units Considered', 'field': 'paymentsConsidered', 'type': 'label', 'width': 180 },     
      { 'header': 'No.of Units Not Considered', 'field': 'paymentsNotConsidered', 'type': 'label', 'width': 230 },
      //{ 'header': 'Total Payments', 'field': 'transactionsCount', 'type': 'label', 'width': 140 },
      { 'header': 'No. of Payments Considered', 'field': 'transactionConsidered', 'type': 'label', 'width': 240 },
      { 'header': 'No.of Payments Not Considered', 'field': 'transactionNotConsidered', 'type': 'label', 'width': 240 },
      { 'header': 'No.of TDS payments to be done', 'field': 'transWithTdsPending', 'type': 'label', 'width': 250 },
      { 'header': 'No.of TDS payments Completed', 'field': 'transWithTdsPaid', 'type': 'label', 'width': 250 },
      { 'header': 'No.of payments with Co-Owners', 'field': 'transWithCoOwner', 'type': 'label', 'width': 260  },
      { 'header': 'No.of payments without Co-Owners', 'field': 'transWithNoCoOwner', 'type': 'label', 'width': 280 },
      { 'header': 'No.of Records Form 16B generated', 'field': 'transWithF16BGenerated', 'type': 'label', 'width': 280  },
      { 'header': 'No.of Records Form 16B generated(with Co-owners)', 'field': 'transWithF16BGeneratedWithSharedOwnership', 'type': 'label', 'width': 410  },
      { 'header': 'No.of Records Form 16B generated(without Co-owners)', 'field': 'transWithF16BGeneratedWithNotSharedOwnership', 'type': 'label','width':430 }
    ];

    //this.getProperties();
    //this.getReportList();
    //this.getSellers();
    //this.getLotNo();
    this.getReportList();
  }

  getProperties() {
    this.propertySvc.getProperties().subscribe(response => {
      this.premisesDDl = response;
      this.premisesDDl.splice(0, 0, { 'propertyID': '', 'addressPremises': '' });
    });
  }

  getSellers() {
    this.sellerSvc.getSellers().subscribe(response => {
      this.sellerDDl = response;
      this.sellerDDl.splice(0, 0, { 'sellerID': '', 'sellerName': '' });
    });
  }

  getLotNo() {
    this.lotSummarySvc.getLotNo().subscribe(response => {
      this.lotNoDDl = response;
      this.lotNoDDl.splice(0, 0, { 'lotNo': '' });
    });
  }

  getReportList() {
  
    this.lotSummarySvc.getReportList().subscribe(response => {     
      this.reportRowData = response;
    });
  }

  /**
     * On destroy
     */
  ngOnDestroy(): void {
  }

  downloadExcel() {
   

    this.lotSummarySvc.downloadtoExcel().subscribe(response => {
      let blob: any = new Blob([response], { type: 'application/vnd.ms-excel' });
      fileSaver.saveAs(blob, 'LotSummaryReport.xls');
    });
  }

  search() {
    this.getReportList();
  }

  reset() {
    this.reportform.reset();
  }


}
