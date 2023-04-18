import { Component, OnDestroy, OnInit, ViewChild, ElementRef } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormGroupDirective } from '@angular/forms';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { fuseAnimations } from '@fuse/animations';
import * as Xlsx from 'xlsx';
import { PropertyService } from '../../property/property.service';
import { ToastrService } from 'ngx-toastr';
import * as _ from 'lodash';
import { StatusReportService} from '../status-report/status-report.service';
import * as moment from 'moment';
import * as fileSaver from 'file-saver';

@Component({
  selector: 'status-report',
  templateUrl: './status-report.component.html',
  styleUrls: ['./status-report.component.scss'],
  animations: fuseAnimations
})
export class StatusReportComponent implements OnInit, OnDestroy {
  reportform: UntypedFormGroup;

  reportRowData: any[] = [];
  reportColumnDef: any[] = [];
  premisesDDl: any[] = []; 
  unitNoDDl: any[] = [];
  lotNoDDl: any[] = [];

  constructor(private _formBuilder: UntypedFormBuilder, private statusReportSvc: StatusReportService,  private propertySvc: PropertyService, private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.reportform = this._formBuilder.group({
      receiptType: ['1'],
      lotNo: [''],
      premisesId: [''],
      unitNo: [''],
      customerName: [''],
      statusId: [''],
      sellerId: ['']
    });

    this.reportColumnDef = [
      { 'header': 'Premises', 'field': 'premises', 'type': 'label','width':250 },
      { 'header': 'Client name', 'field': 'customerName', 'type': 'label', 'width': 250 },
      { 'header': 'Unit No', 'field': 'unitNo', 'type': 'label',  'width': 90 },
      { 'header': 'Lot', 'field': 'lotNo', 'type': 'label', 'width': 70},
      { 'header': 'Payment Receipt Date', 'field': 'ReceiptDate', 'type': 'label', 'width': 190},
      { 'header': 'Remittance of TDS Amount', 'field': 'tdsPaid', 'type': 'label', 'width': 220 },
      { 'header': 'Form 16B Requested', 'field': 'formRequested', 'type': 'label', 'width': 170 },
      { 'header': 'Form 16B Downloaded', 'field': 'formDownloaded', 'type': 'label', 'width': 180 },
      { 'header': 'Form 16B & Challan sent to Customer', 'field': 'emailDate', 'type': 'label', 'width': 300 }   
    ];    

    this.getProperties();
    this.getReportList();
    this.getLotNo();
  }

  getProperties() {
    this.propertySvc.getProperties().subscribe(response => {
      this.premisesDDl = response;
      this.premisesDDl.splice(0, 0, { 'propertyID': '', 'addressPremises': '' });
    });
  }
  getLotNo() {
    this.statusReportSvc.getLotNo().subscribe(response => {
      this.lotNoDDl = response;
      this.lotNoDDl.splice(0, 0, { 'lotNo': '' });
    });
  }

  getReportList() {
    var filters = this.reportform.value;
    
    this.statusReportSvc.getReportList(filters.customerName, filters.premisesId,  filters.unitNo, filters.lotNo).subscribe(response => {
      _.forEach(response, obj => {
        obj.receiptDate = obj.paymentReceiptDate==null?"": moment(obj.paymentReceiptDate).local().format("DD-MMM-YYYY");
        obj.tdsPaid = obj.remittanceOfTdsAmount==null ? "" : moment(obj.remittanceOfTdsAmount).local().format("DD-MMM-YYYY");
        obj.formRequested = obj.form16BRequested==null ? "" : moment(obj.form16BRequested).local().format("DD-MMM-YYYY");
        obj.formDownloaded = obj.form16BDownloaded==null ? "" : moment(obj.form16BDownloaded).local().format("DD-MMM-YYYY");
        obj.emailDate = obj.mailDate==null ? "" : moment(obj.mailDate).local().format("DD-MMM-YYYY");
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

    this.statusReportSvc.downloadtoExcel(filters.customerName, filters.premisesId, filters.unitNo, filters.lotNo).subscribe(response => {
      let blob: any = new Blob([response], { type: 'application/vnd.ms-excel' });
      fileSaver.saveAs(blob, 'StatusReport.xls');
    });
  }
   
  search() {
    this.getReportList();
  }

  reset() {
    this.reportform.reset();
  }


}
