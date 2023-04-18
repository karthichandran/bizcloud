import { Component, OnDestroy, OnInit, ViewChild, ElementRef } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormGroupDirective } from '@angular/forms';
import { fuseAnimations } from '@fuse/animations';
import * as Xlsx from 'xlsx';
import { SellerService } from '../seller/seller.service';
import { PropertyService} from '../property/property.service';
import { ToastrService } from 'ngx-toastr';
import { GridComponent} from '../../@fuse/components/grid/grid.component';
import * as _ from 'lodash';
import {TdsReceiptService } from '../tds-receipt/tds-receipt.service';
import * as moment from 'moment';

@Component({
  selector: 'app-tds-receipt',
  templateUrl: './tds-receipt.component.html',
  styleUrls: ['./tds-receipt.component.scss'],
  animations: fuseAnimations
})
export class TdsReceiptComponent implements OnInit, OnDestroy {
  @ViewChild(GridComponent) gridComp: GridComponent;
  receiptform: UntypedFormGroup;

  tdsRowData: any[] = [];
  tdsColumnDef: any[] = [];
  serviceFeeRowData: any[] = [];
  serviceColumnDef: any[] = [];
  premisesDDl: any[] = [];
  sellerDDl: any[] = [];
  modeOfReceiptDDl: any[] = [];
  lotNoDDl: any[] = [];
  totalTds: number;
  receipt: any;
  mode: any;
  refNo: any;

  statusType: any[] = [{ 'id': 1, 'description': 'Pending' }, { 'id': 2, 'description': 'Saved' }];
  constructor(private _formBuilder: UntypedFormBuilder, private tdsReceiptSvc: TdsReceiptService, private sellerService: SellerService, private propertySvc: PropertyService, private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.receiptform = this._formBuilder.group({
      receiptType: ['2'],
      lotNo: [''],
      premisesId: [''],
      unitNo: [''],
      customerName: [''],
      statusId: [1],
      sellerId:['']
    });

    this.tdsColumnDef = [
      { 'header': 'Lot', 'field': 'lotNo', 'type': 'label', 'width': 80 },
      { 'header': 'Client name', 'field': 'customerName', 'type': 'label', 'width': 250 },
      { 'header': 'Unit No', 'field': 'unitNo', 'type': 'label', 'width': 80 },
      { 'header': 'TDS', 'field': 'tds', 'type': 'label', 'width': 100 },
      { 'header': 'Date of Receipt', 'field': 'receipt_Date', 'type': 'date', 'width': 160 },
      { 'header': 'Mode of Receipt', 'field': 'modeOfReceiptID', 'type': 'selection', 'options': this.modeOfReceiptDDl, 'width': 160 },
      { 'header': 'Ref No', 'field': 'referenceNo', 'type': 'textbox', 'width': 130 },
      { 'header': 'TDS Received', 'field': 'tdsReceived', 'type': 'textbox', 'width': 160 },
      //{ 'header': 'Save', 'field': '', 'type': 'button', 'activity': ['save'],  'width': 80 },
      { 'header': 'Select All', 'field': 'isSelected', 'type': 'checkbox', 'checkall': true , 'width': 100 } 
     
    ];

    this.serviceColumnDef = [
      //{ 'header': 'Lot', 'field': 'lotNo', 'type': 'label'},
      { 'header': 'Client name', 'field': 'customerName', 'type': 'label', 'width': 250},
      { 'header': 'Unit No', 'field': 'unitNo', 'type': 'label', 'width': 80 },
      { 'header': 'Service Fee Payable', 'field': 'serviceFee', 'type': 'label', 'width': 180 },
      { 'header': 'GST Payable', 'field': 'gstPayable', 'type': 'label', 'width': 120 },
      { 'header': 'Total Payable', 'field': 'totalServiceFeeReceived', 'type': 'label', 'width': 120 },
      { 'header': 'TDS Interest', 'field': 'tdsInterest', 'type': 'label', 'width': 120 },
      { 'header': 'Late fee', 'field': 'lateFee', 'type': 'label', 'width': 100},
      { 'header': 'Date of Receipt', 'field': 'receipt_Date', 'type': 'date', 'width': 150},
      { 'header': 'Mode of Receipt', 'field': 'modeOfReceiptID', 'type': 'selection', 'options': this.modeOfReceiptDDl, 'width': 150},
      { 'header': 'Ref No', 'field': 'referenceNo', 'type': 'textbox', 'width': 100},
      { 'header': 'Total Service Received', 'field': 'totalServiceFeeReceived', 'type': 'textbox', 'width': 180 },
      { 'header': 'TDS Interest received', 'field': 'tdsInterestReceived', 'type': 'textbox', 'width': 170},
      { 'header': 'Late Fee Received', 'field': 'lateFeeReceived', 'type': 'textbox', 'width': 160},
      { 'header': 'Email Sent', 'field': 'emailStatus', 'type': 'label', 'width': 100 },
      //{ 'header': 'Save', 'field': '', 'type': 'button', 'activity': ['save'], 'width': 50},
      { 'header': 'Select All', 'field': 'isSelected', 'type': 'checkbox', 'checkall': true, 'width': 100} 
    ];
    
    this.getSellers();
    this.getProperties();
    this.getLotNo();
    this.getTdsReceipt();
    this.getModeOfReceipt();
  }

  getProperties() {
    this.propertySvc.getProperties().subscribe(response => {
      this.premisesDDl = response;
      this.premisesDDl.splice(0, 0, { 'propertyID': '', 'addressPremises': '' });
    });
  }

  getSellers() {
    this.sellerService.getSellers().subscribe(response=> {
      this.sellerDDl = response;
      this.sellerDDl.splice(0, 0, { 'sellerID': '', 'sellerName': '' });
    });
  }
  getLotNo() {
    this.tdsReceiptSvc.getLotNo().subscribe(response => {
      this.lotNoDDl = response;
      this.lotNoDDl.splice(0, 0, { 'lotNo': '' });
    });
  }

  getModeOfReceipt() {
    this.tdsReceiptSvc.getModeOfReceipt().subscribe(response => {
      _.forEach(response, o => {
        o.id = o.modeOfReceiptID;
        o.description = o.modeOfReceiptText;
      });

      this.modeOfReceiptDDl = response;
      this.serviceColumnDef[8].options = this.modeOfReceiptDDl;
      this.tdsColumnDef[5].options = this.modeOfReceiptDDl;
    });
  }

  getTdsReceipt() {
    var filters = this.receiptform.value;
    var isTds = filters.receiptType == "2" ? "true" : "false";
    var isPending = filters.statusId == 1 ? "true" : "false";
    
    this.tdsReceiptSvc.getReceipts(isTds, isPending, filters.customerName, filters.premisesId, filters.sellerId, filters.unitNo, filters.lotNo).subscribe(response => {
      _.forEach(response, obj => {
        var t = moment(obj.dateOfReceipt).year();
        obj.receipt_Date = moment(obj.dateOfReceipt).year() > 2001 ? moment(obj.dateOfReceipt).local().format("DD-MMM-YYYY") : "";
      
        obj.isSelected = false;
        obj.tdsReceived = obj.tds;
        obj.tdsInterestReceived = obj.tdsInterest;
        obj.lateFeeReceived = obj.lateFee;
      });

      if (filters.receiptType == "2")
        this.tdsRowData = response;
      else {
        //_.forEach(response, o => {
        //  o.emailStatus = (o.emailSent != null) ? o.emailSent : false;
        //  if (o.receiptID == 0)
        //    o.disabled = true;
        //  else {
        //    if (o.emailStatus) {
        //      o.disabled = true;
        //    }
        //    else
        //    o.disabled = false;
        //  }
        //});
     
        this.serviceFeeRowData = response;
      }
    });
  }

  checkboxEve() {
    let total = 0;
    _.forEach(this.tdsRowData, o => {
      if (o.isSelected) {
        total += parseInt(o.tdsReceived);
        }
    });
    this.totalTds = total;
  }

  save(): void {
    var isTds = this.receiptform.value.receiptType == "2" ? true : false;
    let models: any = [];
    let gridData: any = [];
    if (isTds) {
      gridData = this.tdsRowData;
    } else {
      gridData = this.serviceFeeRowData;
    }

    _.forEach(gridData, o => {
      if (o.isSelected) {
        o.dateOfReceipt = moment(o.receipt_Date).local().format("DD-MMM-YYYY");
        o.receiptType = this.receiptform.value.receiptType;
        models.push(o);
      }
    });
        
    let isNew: boolean = this.receiptform.value.statusId == 1 ? true : false;   
    this.tdsReceiptSvc.save(models, isNew).subscribe(response => {
      this.toastr.success("Receipt changes are saved successfully");     
      this.getTdsReceipt();
    });
  }

  apply() {
    var isTds = this.receiptform.value.receiptType == "2" ? true : false;
    if (isTds) {
      _.forEach(this.tdsRowData, o => {
        if (o.isSelected) {
          o.receipt_Date = this.receipt;
          o.modeOfReceiptID = this.mode;
          o.referenceNo = this.refNo;
        }
        this.tdsRowData = [...this.tdsRowData];
      });
    }
    else {
      _.forEach(this.serviceFeeRowData, o => {
        if (o.isSelected) {
          o.receipt_Date = this.receipt;
          o.modeOfReceiptID = this.mode;
          o.referenceNo = this.refNo;
        }
        this.serviceFeeRowData = [...this.serviceFeeRowData];
      });
    }
  }
  /**
     * On destroy
     */
  ngOnDestroy(): void {
  }

  search() {
    this.getTdsReceipt();
  }
  reset() {
    this.receiptform.reset();
    this.receiptform.get("receiptType").setValue("2");
    this.receiptform.get("statusId").setValue(1);
  }

  sendMail() {
    let ids: string="";
    _.forEach(this.serviceFeeRowData, o => {
      if (o.isSelected && o.receiptID!=0)
        ids = ids + o.receiptID + ",";
    });
    if (ids == "") {
      this.toastr.warning("Please select records to send email");
      return;
    }
    ids = ids.substr(0, ids.length - 1);
  
    this.tdsReceiptSvc.sendMail(ids).subscribe(response => {
      if (response == "")
        this.toastr.success("Email sent successfully");
      else
        this.toastr.error("Failed to send some of emails")
    });
  }
}
