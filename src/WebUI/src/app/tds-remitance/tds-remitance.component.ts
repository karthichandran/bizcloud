import { Component, OnDestroy, OnInit, ElementRef, ViewChild } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { HttpEventType } from '@angular/common/http';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { MatStepper } from '@angular/material/stepper';
import { fuseAnimations } from '@fuse/animations';
import * as Xlsx from 'xlsx';
import { ToastrService } from 'ngx-toastr';
import * as _ from 'lodash';
import { TdsRemitanceService } from './tds-remitance.service';
import * as moment from 'moment';
//import { isNullOrUndefined, isUndefined } from 'util';
import * as fileSaver from 'file-saver';
import { ConfirmationDialogService } from '../core/confirmation-dialog/confirmation-dialog.service';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-tds-remitance',
  templateUrl: './tds-remitance.component.html',
  styleUrls: ['./tds-remitance.component.scss'],
  animations: fuseAnimations
})
export class TdsRemitanceComponent implements OnInit, OnDestroy {
  @ViewChild('stepper') private myStepper: MatStepper;
  debitAdviceForm: UntypedFormGroup;
  challanform: UntypedFormGroup;
  requestform: UntypedFormGroup;
  form16b: UntypedFormGroup;

  challanFile: any = {};
  form16File: any = {};
  debitAdviceFile:any={};
  challnFileName: string;
  f16FileName: string;
  showFileUpload: boolean;
  remitanceModel: any = {};
  transactionID: number;
  isTabDisable: boolean;
  rowData: any[] = [];
  columnDef: any[] = [];
  remitanceStatus: any[] = [];
  remitanceStatusDDl: any[] = [];
  sellerProperty: any[] = [];

  isChallanUpload: boolean;
  isF16Upload: boolean;
  isDebitAdviceUpload:boolean;
  //Search
  searchByPremises: string;
  searchByCustomer: string;
  searchByunitNo: string;
  searchBylotNo: string;
  searchByStatus: string;
  searchByAmount: string;
  

  constructor(private tdsService: TdsRemitanceService, private toastr: ToastrService, private _formBuilder: UntypedFormBuilder, private confirmationDialogSrv: ConfirmationDialogService,) {
  }

  ngOnInit(): void {  

 
    this.debitAdviceForm = this._formBuilder.group({
      paymentDate: ['',Validators.required],
      cinNo: ['', Validators.required],
      debitAdviceID:['']

    });

    this.challanform = this._formBuilder.group({
      challanDate: ['',Validators.required],
      challanID: ['', Validators.required],
      challanAckNo: ['', Validators.required],
      challanAmount: ['', Validators.required],
      customerName: [{ value: '', disabled: true }],
      premises: [{ value: '', disabled: true }],
      unitNo: [{ value: '', disabled: true }],
      lotNo: [{ value: '', disabled: true }]
    });

    this.requestform = this._formBuilder.group({
      customerPAN: [{ value: '', disabled: true }],
      dateOfBirth: [{ value: '', disabled: true }],
      f16BDateOfReq: ['', Validators.required],
      f16BRequestNo: ['', Validators.required]     
    });

    this.form16b = this._formBuilder.group({
      f16BCertificateNo: ['', Validators.required],
      f16CustName: [''],
      f16UpdateDate: [''],
      f16CreditedAmount:['']
    });

    this.columnDef = [
      { 'header': '', 'field': '', 'type': 'button', 'activity': ['edit'], 'width': 50},
      { 'header': 'Customer', 'field': 'customer', 'type': 'label', 'width': 250},
      { 'header': 'Property', 'field': 'propertyPremises', 'type': 'label', 'width': 250 },
      { 'header': 'Seller', 'field': 'seller', 'type': 'label', 'width': 250},
      { 'header': 'Unit No.', 'field': 'unitNo', 'type': 'label', 'width': 80 },
      { 'header': 'Lot No.', 'field': 'lotNo', 'type': 'label', 'width': 80},
      { 'header': 'Amount', 'field': 'amountShare', 'type': 'label', 'width': 100  },
      { 'header': 'GST', 'field': 'gstAmount', 'type': 'label', 'width': 80 },
      { 'header': 'TDS', 'field': 'tdsAmount', 'type': 'label', 'width': 80 },
      { 'header': 'TDS Interest', 'field': 'tdsInterest', 'type': 'label', 'width': 120 },
      { 'header': 'Late Fee', 'field': 'lateFee', 'type': 'label', 'width': 90 } ,
      { 'header': 'Gross Amount', 'field': 'grossShareAmount', 'type': 'label', 'width': 150 },
      { 'header': 'TDS By Seller', 'field': 'tdsBySeller', 'type': 'label', 'width': 140},
      { 'header': 'Date of Deduction', 'field': 'deductionDate', 'type': 'label', 'width': 170 },
      { 'header': 'Remittance Status', 'field': 'remittanceStatus', 'type': 'label', 'width': 170 }, 
      //{ 'header': 'Proceed to 26QB', 'field': '', 'type': 'button', 'activity': ['custom'], 'maxWidth': 140  }
    ];
  
    this.getRemitanceStatus();
    this.isTabDisable = true;
    this.showFileUpload = false;
  }

  validateChallanform() {
    if (!this.challanform.valid) {
      Object.keys(this.challanform.controls).forEach(field => {
        const control = this.challanform.get(field);
        control.markAsTouched({ onlySelf: true });
      });
      this.toastr.error("Please fill the required fields");
      return false;
    }

    let model = this.challanform.value;
    if (model.challanID==0) {
      this.toastr.error("Challan Serial No. must not be 0");
      return false;
    }

    if (model.challanAckNo == 0) {
      this.toastr.error("Acknowledgement No. must not be 0");
      return false;
    }
    return true;
  }

  validateRequestForm() {
    if (!this.requestform.valid) {
      Object.keys(this.requestform.controls).forEach(field => {
        const control = this.requestform.get(field);
        control.markAsTouched({ onlySelf: true });
      });
      this.toastr.error("Please fill the required fields");
      return false;
    }

    let model = this.requestform.value;
    if (model.f16BRequestNo == 0) {
      this.toastr.error("Request No. must not be 0");
      return false;
    }
    return true;
  }

  validateForm16b() {
    if (!this.form16b.valid) {
      Object.keys(this.form16b.controls).forEach(field => {
        const control = this.form16b.get(field);
        control.markAsTouched({ onlySelf: true });
      });
      this.toastr.error("Please fill the required fields");
      return false;
    }
    let model = this.form16b.value;
    if (model.f16BCertificateNo == 0) {
      this.toastr.error("Certificate No. must not be 0");
      return false;
    }
    return true;
  }

  validateDebitAdvice() {
    if (!this.debitAdviceForm.valid) {
      Object.keys(this.debitAdviceForm.controls).forEach(field => {
        const control = this.debitAdviceForm.get(field);
        control.markAsTouched({ onlySelf: true });
      });
      this.toastr.error("Please fill the required fields");
      return false;
    }
    let model = this.debitAdviceForm.value;
    if (model.cinNo == 0) {
      this.toastr.error("CIN No. must not be 0");
      return false;
    }
    return true;
  }


  save(type: any): void {
    let remitmodel= this.remitanceModel;
    if (type == "challan") {
      if (!this.validateChallanform())
        return;
      let model = this.challanform.value;     
      remitmodel.challanDate = moment(model.challanDate).local().format("YYYY-MM-DD");
      remitmodel.challanID = model.challanID;
      remitmodel.challanAckNo = model.challanAckNo;
      remitmodel.challanAmount = model.challanAmount;
      remitmodel.f16BDateOfReq = "";
    }

    if (type == "request") {
      if (!this.validateRequestForm())
        return;
      let model = this.requestform.value;
      remitmodel.f16BDateOfReq = moment(model.f16BDateOfReq).local().format("YYYY-MM-DD");
      remitmodel.f16BRequestNo = model.f16BRequestNo;
    }

    if (type == "form16b") {
      if (!this.validateForm16b())
        return;
      let model = this.form16b.value;
      remitmodel.f16BCertificateNo = model.f16BCertificateNo;
      remitmodel.f16CustName = model.f16CustName;
      remitmodel.f16UpdateDate = moment(model.f16UpdateDate).local().format("YYYY-MM-DD");
      remitmodel.f16CreditedAmount = model.f16CreditedAmount;
    }

    if (type == "debitAdvice") {
      if (!this.validateDebitAdvice())
        return;
      let model = this.debitAdviceForm.value;
      remitmodel.cinNo = model.cinNo;
      remitmodel.paymentDate = moment(model.paymentDate).local().format("YYYY-MM-DD");
    }

    if (type == "debitAdvice")
      this.saveDebitAvice(remitmodel);
    else
      this.saveRemitance(remitmodel);
  }

  saveRemitance(model: any) {
    let isNew: boolean;
    if (model.remittanceID == 0)
      isNew = true;

    if (model.challanID != 0)
      model.remittanceStatusID = 2;
    // if (model.f16BRequestNo != 0 && !isNullOrUndefined(model.f16BRequestNo) && model.f16BRequestNo!="")
    if (model.f16BRequestNo != 0 && !(model.f16BRequestNo===undefined) &&  !(model.f16BRequestNo===null) && model.f16BRequestNo!="")
      model.remittanceStatusID = 3;
    // if (model.f16BCertificateNo != 0 && !isNullOrUndefined(model.f16BCertificateNo) && model.f16BCertificateNo != "")
    if (model.f16BCertificateNo != 0 && !(model.f16BCertificateNo===undefined) && !(model.f16BCertificateNo===null) && model.f16BCertificateNo != "")
      model.remittanceStatusID = 4;

    if (model.clientPaymentTransactionID == 0)
      model.clientPaymentTransactionID = this.transactionID;

    this.tdsService.save(model, isNew).subscribe(response => {
      this.toastr.success("Remittance changes are saved successfully");
      this.getRemitance(this.transactionID.toString());
    });
  }

  saveDebitAvice(model:any){
    let isNew: boolean;
    if (model.debitAdviceID == 0 || model.debitAdviceID==null || model.debitAdviceID==undefined)
      isNew = true;

    if (model.clientPaymentTransactionID == 0)
      model.clientPaymentTransactionID = this.transactionID;
    var da = {
      debitAdviceID:( model.debitAdviceID==null ||  model.debitAdviceID==undefined)?0: model.debitAdviceID,
      cinNo: model.cinNo,
      paymentDate: model.paymentDate,
      blobId: this.debitAdviceFile.blobID,
      clientPaymentTransactionID: model.clientPaymentTransactionID
    };

      this.tdsService.saveDebitAdvice(da, isNew).subscribe(response => {
        this.toastr.success("Debit Advice changes are saved successfully");
        this.getRemitance(this.transactionID.toString());
      });
  }
  /**
     * On destroy
     */
  ngOnDestroy(): void {
  }
     
  getRemitanceStatus() {
    this.tdsService.getRemitanceStatus().subscribe((response) => {
      this.remitanceStatus = response;
      this.remitanceStatusDDl = [];
      _.forEach(response, o => {
        this.remitanceStatusDDl.push({ 'id': o.remittanceStatusID, 'description': o.remittanceStatusText });       
      });
      this.remitanceStatusDDl.splice(0, 0, { 'id': '', 'description': '' }); 
    });
  }

  tabChanged(eve: MatTabChangeEvent) {
    //console.log('tabChangeEvent => ', eve);
    //console.log('index => ', eve.index);
    //if (eve.index == 1) {
    //  this.getAllProperties();
    //}
  }

  search(){   
    this.tdsService.getTdsRemitance(this.searchByCustomer, this.searchByPremises, this.searchByunitNo, this.searchBylotNo,this.searchByStatus,this.searchByAmount).subscribe((response) => {
      let statusList = this.remitanceStatus;
      function getStatus(statusId: any) {
        let statusObj = _.find(statusList, o => { return o.remittanceStatusID == statusId; });
        return statusObj.remittanceStatusText;
      }
      _.forEach(response, o => {
        o.customer = o.customerName + ' / ' + o.customerShare;
        o.seller = o.sellerName + ' / ' + o.sellerShare;
        o.tdsBySeller = o.tdsCollectedBySeller == true ? "Yes" : "No";
        o.deductionDate = moment(o.dateOfDeduction).format('DD-MMM-YYYY');
        o.status = getStatus(o.statusTypeID);
      });
        this.rowData = response;
      }); 
  } 

  reset() {
    this.searchByCustomer = "";
    this.searchByPremises = "";
    this.searchByunitNo = "";
    this.searchBylotNo = "";
    this.searchByAmount = "";
    this.searchByStatus=""
    this.search();
  }

  //customRow(eve) {
  //  this.tdsService.proceedForm26qb(eve.clientPaymentTransactionID).subscribe(response => {
  //    this.toastr.success("Form26QB is filled with data");
  //  });
  //}

  editRow(eve) {
    this.isTabDisable = false;
    this.transactionID = eve.clientPaymentTransactionID;
    let challanAmount = eve.tdsAmount + eve.tdsInterest + eve.lateFee;
    this.tdsService.getTdsRemitanceById(eve.clientPaymentTransactionID).subscribe(response => {
      this.remitanceModel = response;
      
      this.loadRemitanceDetails(response, challanAmount);
      var ele = document.getElementsByClassName('mat-tab-label') as HTMLCollectionOf<HTMLElement>;
      ele[1].click();
      if (this.myStepper.selectedIndex >= 1) {
        this.myStepper.reset();
      }
    });
  }

  getRemitance(transId:string) {
    this.tdsService.getTdsRemitanceById(transId).subscribe(response => {
      this.remitanceModel = response;    
      this.loadRemitanceDetails(response, response.challanAmount);
    });
  }

  loadRemitanceDetails(model: any, challanamount: number) {

    if(model.debitAdviceID==0){
      this.isDebitAdviceUpload=false;
    }
    else{
      this.getFiles(model.debitAdviceBlobId,'debitAdvice');
    }

    if (model.remittanceID == 0) {
      model.challanAmount = challanamount;
      model.challanDate = moment(moment().local().format("DD-MMM-YYYY")).format();
      this.showFileUpload = false;
      this.isChallanUpload = false;
      this.isF16Upload = false;
    }
    else {
      this.showFileUpload = true;
      this.getFiles(model.challanBlobID, "challan");
      this.getFiles(model.form16BlobID, "form16");
      
    }   
   
    this.challanform.patchValue(model);
    this.requestform.patchValue(model);
    this.requestform.get('dateOfBirth').setValue(moment(model.dateOfBirth).format("DDMMYYYY"));
    this.form16b.patchValue(model);
    this.debitAdviceForm.patchValue(model);
  }

  selectedstepperIndex(eve) {

  }

  Next(stepper: MatStepper, index: number) {
    if (index == 1) {
      stepper.next();
    }
    if (index == 2 && this.challanform.valid) {
      stepper.next();
    }
    if (index == 3 && this.requestform.valid) {
      stepper.next();
    }
  }

  uploadFile(event, fileType) {

    if (event.target.files && event.target.files.length > 0) {
      const file = event.target.files[0];
      let category: number;
      if (fileType == "challan") {
        this.challnFileName = file.name;
        category = 7;
      }
      if (fileType == "form16") {
        this.f16FileName = file.name;
        category = 6;
      }
    
      let formData = new FormData();
      formData.append(file.name, file);

      if(fileType=='debitAdvice'){
        
        this.tdsService.uploadDebitAdviceFile(formData).subscribe((event) => {
          if (event.body != undefined) {
            this.toastr.success("File Uploaded successfully");
            this.remitanceModel.debitAdviceBlobId = event.body;
            this.getFiles(this.remitanceModel.debitAdviceBlobId, 'debitAdvice');
          }
        }, null, () => {
            this.getRemitance(this.transactionID.toString());
        });

      }
      else{     
           
      this.tdsService.uploadFile(formData, this.remitanceModel.remittanceID, category).subscribe((event) => {
        if (event.type == HttpEventType.Sent) {
          this.toastr.success("File Uploaded successfully");          
        }       
      }, null, () => {
          this.getRemitance(this.transactionID.toString());
      });
    }
    }
  }

  getFiles(blobId: string, fileType: string) {
    // if (isUndefined(blobId) || blobId == null) {
      if ((blobId===undefined) || blobId == null) {
      if (fileType == "challan") {
        this.challanFile = {};
        this.isChallanUpload = false;
      }
       if(fileType=="form16"){
        this.form16File = {};
        this.isF16Upload = false;
      }
      if(fileType=="debitAdvice"){
        this.debitAdviceFile={};
        this.isDebitAdviceUpload=false;
      }
      return;
    }

    this.tdsService.getUploadedFiles(blobId).subscribe(response => {
      // if (isUndefined(response.fileName)) {
        if ( response==null  || response.fileName===undefined ) {
        if (fileType == "challan") {
          this.isChallanUpload = false;
        }
        else if(fileType=="form16"){
          this.isF16Upload = false;
        }
        else if(fileType=="debitAdvice"){
          this.isDebitAdviceUpload=false;
        }
        return;
      }
      if (fileType == "challan") {
        this.challanFile = response;
        this.isChallanUpload = true;
      }
      else  if(fileType=="form16"){
        this.form16File = response;
        this.isF16Upload = true;
      }
      else if(fileType=="debitAdvice"){
        this.debitAdviceFile=response;
        this.isDebitAdviceUpload=true;
      }
    });
  }

  downloadFile(blobId: any, name: string, status: any) {

    this.tdsService.downloadFiles(blobId).subscribe((response) => {

      let fileType = name.split('.')[1];
      let blobType = "";

      if (fileType == 'pdf') {
        blobType = 'application/pdf'
      }
      else if (fileType == 'jpg' || fileType == 'jpeg' || fileType == 'png') {
        blobType = 'image/' + fileType;
      }
      else if (fileType == 'xls') {
        blobType = 'application/vnd.ms-excel';
      }
      else if (fileType == 'xlsx') {
        blobType = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';
      }
      else if (fileType == 'docx') {
        blobType = 'application/vnd.openxmlformats-officedocument.wordprocessingml.document';
      }
      else if (fileType == 'ods') {
        blobType = 'application/vnd.oasis.opendocument.spreadsheet';
      }
      else if (fileType == 'xls') {
        blobType = 'application/msword';
      }


      let blob: any = new Blob([response], { type: blobType });

      //This will open file in new browser tab

      if (status == 'view') {
        const url = window.URL.createObjectURL(blob);
        window.open(url);
      } else {
        // window.location.href = response.url;
        fileSaver.saveAs(blob, name);
      }
    });
  }

  deleteFile(id: string, fileType:string) {
    this.tdsService.deleteFile(id).subscribe(() => {
      
      if (fileType == "challan") {
        this.challanFile = {};
        this.isChallanUpload = false;
      }
      else if(fileType=="form16"){
        this.form16File = {};
        this.isF16Upload = false;
      }
      else{
        this.debitAdviceFile={};
        this.isDebitAdviceUpload=false;
      }
    });
  }

  deleteRemittance() {
    this.confirmationDialogSrv.showDialog("Are you sure to delete this Remittance ?").subscribe(response => {
      if (response == "ok") {
        this.tdsService.deleteRemittance(this.remitanceModel.remittanceID).subscribe(response => {
          this.toastr.success("Record is deleted successfully");
          var ele = document.getElementsByClassName('mat-tab-label') as HTMLCollectionOf<HTMLElement>;
          ele[0].click();
          this.search();
          this.isTabDisable = true;

        });
      }
    });
  }

  deletedebitAdvice() {
    if (this.remitanceModel.debitAdviceID == undefined || this.remitanceModel.debitAdviceID == 0)
      return;

    this.confirmationDialogSrv.showDialog("Are you sure to delete this Debit Advice?").subscribe(response => {
      if (response == "ok") {
        if (this.remitanceModel.debitAdviceID != undefined) {

          this.tdsService.deleteDebitAdvice(this.transactionID.toString()).subscribe(response => {
            if (this.debitAdviceFile.blobID != null && this.debitAdviceFile.blobID != 0 && this.debitAdviceFile.blobID != undefined) {
              this.tdsService.deleteFile(this.remitanceModel.debitAdviceBlobId).subscribe(response => {
                this.resetAfterDelete();
              });
            }
            else {
              this.resetAfterDelete();
            }
            
          });
        }
       
      }
    });

  }

  resetAfterDelete(){
    this.toastr.success("Record is deleted successfully");
    this.debitAdviceForm.reset();
    this.isDebitAdviceUpload=false;
    this.getRemitance(this.transactionID.toString());
  }
}
