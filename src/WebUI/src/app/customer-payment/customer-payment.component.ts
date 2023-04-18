import { Component, OnDestroy, OnInit, ViewChildren, QueryList, ElementRef, ViewChild, ViewEncapsulation } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormGroupDirective, ValidatorFn, AbstractControl, FormControl } from '@angular/forms';
import { fuseAnimations } from '@fuse/animations';
import * as _ from 'lodash';
import * as moment from 'moment';
import { PropertyService } from '../property/property.service';
import { TaxService } from '../tax/tax.service';
import { ClientPaymentService } from './customer-payment.service';
import { ToastrService } from 'ngx-toastr';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { MatStepper } from '@angular/material/stepper';
import * as fileSaver from 'file-saver';
//import { isUndefined } from 'util';
import { Observable, of, ReplaySubject, Subject } from 'rxjs';
import { ConfirmationDialogService } from '../core/confirmation-dialog/confirmation-dialog.service';
import * as Xlsx from 'xlsx';
import { HttpEventType } from '@angular/common/http';
import { MatSelect } from '@angular/material/select';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-customer-payment',
  templateUrl: './customer-payment.component.html',
  styleUrls: ['./customer-payment.component.scss'],
  animations: fuseAnimations,
  encapsulation: ViewEncapsulation.None

})
export class CustomerPaymentComponent implements OnInit {

  clientform: UntypedFormGroup;
  searchForm: UntypedFormGroup;
  @ViewChild('stepper') private myStepper: MatStepper;
  statusDDl: any[] = [{ 'id': '', 'description': '' }, { 'id': 1, 'description': 'Unit Cancelled' }, { 'id': 2, 'description': 'Unit Assigned' }, { 'id': 3, 'description': 'Block' }, { 'id': 4, 'description': 'Release' }];
  tdsCollectedBySellerDDl: any[] = [{ 'id': 0, 'description': 'No' }, { 'id': 1, 'description': 'Yes' }];


  statusId: any;
  customerAndPan: any = [];
  clientPaymentRoot: any[] = [];
  existingInstallments: any[] = [];
  baseInstallment: any = [];
  baseSellerProperty: any = [];
  propertyList: any[] = [];
  sellerProperty: any[] = [];
  natureOfPayment: any[] = [];
  natureOfPaymentSubSet: any[] = [];
  rowData: any[] = [];
  columnDef: any = [];
  sellerNames: any[] = [];
  remitanceStatusDDl: any[] = [];

  customersShareData: any[] = [];
  customersShareColdef: any[] = [];

  historyData: any[] = [];
  historyColDef: any[] = [];

  // Header Label
  showHeaderNames: boolean;

  //search
  propertyDDl: any[] = [];
  sellerList: any[] = [];
  searchNatureOfPayment: any[] = [];
  //searchByCustomerName: string;
  //searchByPan: string;
  //searchByPropertyID: string;
  //searchByPremises: string;
  //searchByMobile: string;
  //searchByRemark: string;
  //searchByStatusType: number;
  //searchByUnit: string;
  //searchByLot: string;

  showPaymentGrid: boolean;

  customerView: any[] = [];

  maxDate: any;

     //Property Filter for search
     public propertyFilterCtrlForSearch: FormControl = new FormControl();
     @ViewChild('PropertyFilterSelectForSearch', { static: true }) PropertyFilterSelectForSearch: MatSelect;
     /** Subject that emits when the component has been destroyed. */
     protected _onDestroyOnSearch = new Subject<void>();
     public filteredPropertyForSearch: ReplaySubject<any[]> = new ReplaySubject<any[]>();

  constructor(private _formBuilder: UntypedFormBuilder, private propertyService: PropertyService,
    private toastr: ToastrService, private taxService: TaxService, private clientPaymentservice: ClientPaymentService, private confirmationDialogSrv: ConfirmationDialogService) {
  }

  ngOnInit() {
    this.clientform = this._formBuilder.group({
      installmentID: [''],
      clientPaymentID: [''],
      ownershipID: [''],
      dateOfPayment: ['', Validators.required],
      dateOfDeduction: ['', Validators.required],
      revisedDateOfPayment: ['', Validators.required],
      receiptNo: ['', Validators.required],
      lotNo: ['', Validators.required],
      tdsCollected: [''],
      tdsCollectedText: [{ value: '', disabled: true }],
      amountPaid: ['', Validators.required],
      natureOfPaymentID: ['1', Validators.required],
      notConsidered: ['']
    });

    this.searchForm = this._formBuilder.group({
      searchByCustomerName: [''],
      searchBypropertyID: [''],
      searchBypremises: [''],      
      searchByUnit: [''],
      searchByLot: [''],
      searchBySellerID: [''],
      searchByStatus: [''],
      fromDate: [''],
      toDate: [''],
      searchBynatureOfPaymentID:['']
    });

    this.rowData = [];
    this.columnDef = [
      { 'header': '', 'field': '', 'type': 'button', 'activity': ['edit'], 'maxWidth': 80 },
      { 'header': 'Client Name', 'field': 'clientName', 'type': 'labellist', 'minWidth': 400 },
      { 'header': 'PAN', 'field': 'panNumber', 'type': 'labellist' },
      { 'header': 'Premises', 'field': 'propertyPremises', 'type': 'label' },
      //{ 'header': 'Seller', 'field': 'sellerName', 'type': 'labellist' },
      { 'header': 'Unit No.', 'field': 'unitNo', 'type': 'label' },
      { 'header': 'Status', 'field': 'unitStatus', 'type': 'label' },
     
    ];

    this.getAllProperties();
    //this.getSellerProperty();
    this.getNatureOfPayments();
    this.getStatusList();
    this.getSellerList();
    this.getRemitanceStatus();

    this.customersShareColdef = [
      { 'header': 'Customer', 'field': 'customerNameAndPcnt', 'type': 'label', 'width': 250},
      { 'header': 'Seller', 'field': 'sellerNameAndPcnt', 'type': 'label', 'width': 250 },
      { 'header': 'Amount Shared', 'field': 'shareAmount', 'type': 'label', 'width': 120 },
      { 'header': 'GST', 'field': 'gst', 'type': 'label', 'width': 60 },     
      { 'header': 'TDS', 'field': 'tds', 'type': 'label', 'width': 60},
      { 'header': 'TDS Interest', 'field': 'tdsInterest', 'type': 'label', 'width': 120},
      { 'header': 'Gross Amount', 'field': 'grossShareAmount', 'type': 'label', 'width': 120 },
      { 'header': 'Late Fee', 'field': 'lateFee', 'type': 'label', 'width': 80 }

    ];

    this.historyColDef = [
      { 'header': 'Lot No', 'field': 'lotNo', 'type': 'label', 'width': 60},
      { 'header': 'Receipt No', 'field': 'receiptNo', 'type': 'label', 'width': 80},
      { 'header': 'Revised Date', 'field': 'revisedDateOfPayment', 'type': 'label', 'width': 80},
      { 'header': 'Paid Amount', 'field': 'amountPaid', 'type': 'label', 'width': 80},     
      { 'header': 'GST Rate', 'field': 'gstRate', 'type': 'label', 'width': 80},
      { 'header': 'GST', 'field': 'gstAmount', 'type': 'label', 'width': 60},
      { 'header': 'TDS Rate', 'field': 'tdsRate', 'type': 'label', 'width': 80 },
      { 'header': 'TDS', 'field': 'tdsAmount', 'type': 'label', 'width': 60},
      { 'header': 'TDS interest', 'field': 'tdsInterestAmount', 'type': 'label', 'width': 100 },
      { 'header': 'Gross Amount', 'field': 'grossAmount', 'type': 'label', 'width': 100 },
      { 'header': 'Late Fee', 'field': 'lateFee', 'type': 'label', 'width': 60 },
      { 'header': 'Nature of Payment', 'field': 'natureOfPayment', 'type': 'label', 'width': 120 }
    ];

    this.maxDate = moment().toDate();

    this.propertyFilterCtrlForSearch.valueChanges.pipe(takeUntil(this._onDestroyOnSearch))
    .subscribe(() => {
      this.filterPropertyForSearch();
    });
  }

  tabChanged(eve) {
    if (eve.index == 0) {
      this.rowData = [...this.rowData];
      this.showPaymentGrid = false;
      this.showHeaderNames = false;
    }
    else {
      this.showHeaderNames = true;
      this.showPaymentGrid = true;
    }

  }

  getRemitanceStatus() {
    this.clientPaymentservice.getRemitanceStatus().subscribe((response) => {     
      this.remitanceStatusDDl = [];
      _.forEach(response, o => {
        this.remitanceStatusDDl.push({ 'id': o.remittanceStatusID, 'description': o.remittanceStatusText });
      });
      this.remitanceStatusDDl.splice(0, 0, { 'id': '', 'description': '' });
    });
  }

  getNatureOfPayments() {
    this.clientPaymentservice.getNatureOfPayments().subscribe(response => {
      let groups = _.groupBy(response, o => {
        return o.natureOfPaymentID <= 2;
      });
      this.natureOfPayment = groups.true;
      this.natureOfPaymentSubSet = groups.false;
      this.searchNatureOfPayment = groups.true;
      this.searchNatureOfPayment.splice(0, 0, { 'natureOfPaymentID': '', 'natureOfPaymentText': '' });
    });
  }

  getAllProperties() {
    this.propertyService.getProperties().subscribe((response) => {
      this.propertyList = response;
      this.propertyDDl = response;
      this.propertyDDl.splice(0, 0, { 'propertyID': '', 'addressPremises': '' });
    });
  }

  getSellerProperty() {
    this.propertyService.getSellerProperties().subscribe(response => {
      this.sellerProperty = response;
    });
  }

  getStatusList() {
    this.clientPaymentservice.getStatusList().subscribe(response => {
      this.statusDDl = response;
    });
  }
  getSellerList() {
    this.clientPaymentservice.getSellerList().subscribe(response => {
      this.sellerList = response;
      this.sellerList.splice(0, 0, { 'sellerID': '', 'sellerName': '' });
    });
  }
  search() {
    if (!this.validateDate())
      return;
    let model = this.searchForm.value;
    this.clientPaymentservice.getCustomersByFilter(model.searchByCustomerName, model.searchBypropertyID, model.searchBypremises,
      model.searchByUnit, model.searchByLot, model.searchBySellerID, model.searchByStatus, model.fromDate, model.toDate, model.searchBynatureOfPaymentID).subscribe((response) => {
        this._loadCustomerList(response);
      })
  }

  reset() {
    this.searchForm.reset();    
    this.search();
  }

  _loadCustomerList(response) {
    let selProp = this.sellerProperty;
    let statusList = this.statusDDl;
    function sellerNames(propertyId: any) {
      let sellerNameObj: any = [];
      let sellerObj = _.find(selProp, o => { return o.propertyId == propertyId; });
      // if (!isUndefined(sellerObj)) {
        if (!(sellerObj===undefined)) {
        let splitOnbjs = sellerObj.sellerNames.toString().split(',');
        _.forEach(splitOnbjs, o => {
          sellerNameObj.push({ 'name': o });
        });
        return sellerNameObj;
      } else {
        return [{ 'name': '' }];
      }
    }

    this.customerView = _.cloneDeep(response.customersView);
    function getValue(val: number) {
      let model: any[] = [];
      let splitOnbjs = val.toString().split(',');
      _.forEach(splitOnbjs, obj => {
        model.push({ 'name': obj });
      });
      return model;
    }

    _.forEach(response.customersView, function (item) {
      item.clientName = getValue(item.customerName);
      item.panNumber = getValue(item.pan);
      // if (item.statusTypeID != 0)
      //   item.status = _.find(statusList, o => { return o.statusTypeID == item.statusTypeID }).status;
     // item.sellerName = sellerNames(item.propertyID)
    });

    //let invalidCustInx = _.findIndex(response.customersView, o => o.customerPropertyID == 0);   
    //response.customersView.splice(invalidCustInx, 1);
    let filteredRecord = _.takeWhile(response.customersView, o => { return o.customerPropertyID != 0});
    this.rowData = filteredRecord; 

  }

  editRow(eve) {
    this.getClientPaymentBase(eve.ownershipID);
    this.getCustomerAndPan(eve);
    this.sellerNames = eve.sellerName;
    var ele = document.getElementsByClassName('mat-tab-label') as HTMLCollectionOf<HTMLElement>;
    ele[1].click();
    if (this.myStepper.selectedIndex == 1) {
      this.myStepper.previous();
    }
  }

  getClientPaymentBase(ownershipId: string): Observable<any> {
    let obs = new Subject<string>();
    this.clientPaymentservice.getClientPaymentByOwnershipId(ownershipId).subscribe(response => {
      this.clientPaymentRoot = response;
      this.existingInstallments = response.existingInstallments;
      this.baseInstallment = response.installmentBaseObject;

      this.baseInstallment.dateOfPayment = "";
      this.baseInstallment.dateOfDeduction = "";
      this.baseSellerProperty = _.find(this.sellerProperty, o => { return o.propertyId == this.baseInstallment.propertyID; });
      this.processCustomerGrid(response.installmentBaseObject);
      if (response.existingInstallments.length > 0)
        this.processHistoryGrid(response.existingInstallments)
      else
        this.historyData = [];

      this.fillFormControls(this.baseInstallment, true);

      return obs.next("success");

    });
    return obs.asObservable();
  }

  fillFormControls(model: any, isNew: boolean) {
    model.tdsCollectedText = model.tdsCollectedBySeller ? "Yes" : "No";

    model.tdsCollected = (model.tdsCollectedBySeller) ? 1 : 0;
    if (parseInt(model.natureOfPaymentID) == 0)
      model.natureOfPaymentID = 1;
    if (model.natureOfPaymentID > 1) {
      model.notConsidered = model.natureOfPaymentID > 2 ? model.natureOfPaymentID : 3;
      model.natureOfPaymentID = 2;
    }
    this.clientform.patchValue(model);
    if (isNew)
      this.clientform.get('receiptNo').enable({ onlySelf: true });
    else
      this.clientform.get('receiptNo').disable({ onlySelf: true });
  }

  getCustomerAndPan(res) {
    this.customerAndPan = [];
    for (let i = 0; i < res.clientName.length; i++) {
      this.customerAndPan.push({ 'name': res.clientName[i].name, 'pan': res.panNumber[i].name });
    }
  }

  save(action) {
    if (action == 'status') {
      let model: any = {};
      // model.installmentBaseObject = this.baseInstallment;
      //  model.existingInstallments = {};
      //this.savePayment(model,false);
      if (this.baseInstallment.statusTypeID == "" || this.baseInstallment.remarks == "") {
        this.toastr.error("Please fill the Remarks and Status")
        return;
      }
      model.ownershipID = this.baseInstallment.ownershipID;
      model.statusTypeID = this.baseInstallment.statusTypeID;
      model.remarks = this.baseInstallment.remarks;
      this.clientPaymentservice.updateStatusAndremarks(model).subscribe(response => {
        this.getClientPaymentBase(this.baseInstallment.ownershipID);
      });

    }
    else {
      if (this.validate()) {

        let model = this.clientform.value;
        this.baseInstallment.dateOfPayment = moment(model.dateOfPayment).local().format("YYYY-MM-DD");
        this.baseInstallment.dateOfDeduction = moment(model.dateOfDeduction).local().format("YYYY-MM-DD");
        this.baseInstallment.revisedDateOfPayment = moment(model.revisedDateOfPayment).local().format("YYYY-MM-DD");
        // this.baseInstallment.receiptNo = isUndefined(model.receiptNo) ? this.clientform.get('receiptNo').value : model.receiptNo;
        this.baseInstallment.receiptNo = (model.receiptNo===undefined) ? this.clientform.get('receiptNo').value : model.receiptNo;
        this.baseInstallment.lotNo = model.lotNo;
        this.baseInstallment.natureOfPaymentID = model.natureOfPaymentID > 1 ? model.notConsidered : model.natureOfPaymentID;
        this.baseInstallment.amountPaid = model.amountPaid;
        this.baseInstallment.clientPaymentID = model.clientPaymentID;


        let baseModel: any = {};
        baseModel.installmentBaseObject = this.baseInstallment;
        this.savePayment(baseModel, true);
      }

    }
  }

  validate() {
    if (!this.clientform.valid) {
      Object.keys(this.clientform.controls).forEach(field => {
        const control = this.clientform.get(field);
        control.markAsTouched({ onlySelf: true });
      });
      this.toastr.error("Please fill the required fields");
      return false;
    }

    let model = this.clientform.value;
    if (model.dateOfPayment == null || model.dateOfPayment == "") {
      this.toastr.error("Please enter the date of payment");
      return false;
    }
    if (model.dateOfDeduction == null || model.dateOfDeduction == "") {
      this.toastr.error("Please enter the date of Deduction");
      return false;
    }
    if (model.revisedDateOfPayment == null || model.revisedDateOfPayment == "") {
      this.toastr.error("Please enter the Revised date of payment");
      return false;
    }
    if (model.receiptNo == 0 || model.receiptNo == "") {
      this.toastr.error("Please enter the receipt no");
      return false;
    }

    if (model.amountPaid == 0 || model.amountPaid == "") {
      this.toastr.error("Please enter the amount");
      return false;
    }
    return true;
  }

  savePayment(model: any, reload: boolean) {
    let isNew: boolean = model.installmentBaseObject.clientPaymentID == 0 ? true : false;
    if (!isNew) {
      model.existingInstallments = this.existingInstallments;
      _.forEach(model.existingInstallments, o => {
        if (o.installmentID == this.clientform.value.installmentID) {
          o.dateOfPayment = model.installmentBaseObject.dateOfPayment;
          o.dateOfDeduction = model.installmentBaseObject.dateOfDeduction;
          o.revisedDateOfPayment = model.installmentBaseObject.revisedDateOfPayment;
          o.receiptNo = model.installmentBaseObject.receiptNo;
          o.lotNo = model.installmentBaseObject.lotNo;
          o.natureOfPaymentID = model.installmentBaseObject.natureOfPaymentID;
          o.amountPaid = model.installmentBaseObject.amountPaid;
        }
      });
      model.installmentBaseObject = {};
    }
    this.clientPaymentservice.saveClientPayment(model, isNew, this.clientform.value.installmentID).subscribe(respons => {
      this.toastr.success('saved successfully');
      if (reload) {
        this.reloadInstallment(this.clientform.value.ownershipID, this.clientform.value.installmentID);
      }
    }, (e) => {
      // this.toastr.error(e.error.error);
    });
  }

  reloadInstallment(ownershipId: string, installmentID: string) {
    this.getClientPaymentBase(ownershipId).subscribe(response => {
      let model = _.find(this.existingInstallments, o => { return o.installmentList[0].installmentID == installmentID });
      //if (model.natureOfPaymentID > 1) {
      //  model.notConsidered = model.natureOfPaymentID;
      //  model.natureOfPaymentID = 2;
      //}
      let cloneModel = _.cloneDeep(model);
      this.fillFormControls(cloneModel, false);
      this.processCustomerGrid(cloneModel);
    });
  }

  Next(stepper: MatStepper) {
    stepper.next();
  }

  selectedstepperIndex(eve) {
  }

  processCustomerGrid(res: any) {
    let dataRows = _.cloneDeep(res.installmentList);

    _.forEach(dataRows, o => {
      o.customerNameAndPcnt = o.customerName + ' (' + o.customerShare + ' %)';
      o.sellerNameAndPcnt = o.sellerName + ' (' + o.sellerShare + ' %)';
    });

    this.customersShareData = dataRows;
  }

  processHistoryGrid(res: any) {
    let dataRows = [];
    _.forEach(res, o => {
      dataRows.push({
        'installmentID': o.installmentID,
        'receiptNo': o.receiptNo,
        'revisedDateOfPayment': moment(o.revisedDateOfPayment).format('DD-MMM-YYYY'),
        'amountPaid': o.roundoffAdjustment == 0 || o.roundoffAdjustment == o.amountPaid ? o.amountPaid : (o.amountPaid + o.roundoffAdjustment) == 0 ? o.amountPaid : o.amountPaid + " (" + o.roundoffAdjustment + ")",
        'grossAmount': o.grossAmount,
        'tdsAmount': o.tdsAmount,
        'gstAmount': o.gstAmount,
        'tdsRate':o.tdsRate+" %",
        'gstRate': o.gstRate + " %",
        'tdsInterestAmount': o.tdsInterestAmount,
        'natureOfPayment': o.natureOfPayment,
        'lateFee': o.lateFee,
        'lotNo':o.lotNo
      });
    });
    this.historyData = dataRows;
  }

  selectedHistoryRows(eve) {
    let model = _.find(this.existingInstallments, o => { return o.installmentList[0].installmentID == eve.selected[0].installmentID });
    // if (!isUndefined(model)) {
      if (!(model===undefined)) {
      let cloneModel = _.cloneDeep(model);
      this.fillFormControls(cloneModel, false);
      this.processCustomerGrid(cloneModel);
    }
  }

  AddNewPayment() {
    if (this.validate()) {
      if (this.clientform.value.clientPaymentID == 0) {
        this.toastr.error("Please save before adding new payment");
        return;
      }
      this.clientform.reset();
      this.fillFormControls(this.baseInstallment, true);
      this.processCustomerGrid(this.baseInstallment);
      // this.getClientPaymentBase(this.baseInstallment.ownershipID);
      this.OnNatureOfPayChanged({ 'value': 1 });
    }
  }

  onPaymentDateChanged(eve) {
    let paymentdate = eve.target.value;
    if (moment(paymentdate).isValid()) {
      let paymentDate = moment(paymentdate);
      let dueDate = paymentDate.add('1', 'month').endOf('month');
      let currentDay = moment();
      if (currentDay <= dueDate) {
        this.clientform.get('dateOfDeduction').setValue(moment(paymentdate).format('YYYY-MM-DD'));
      } else {
        this.clientform.get('dateOfDeduction').setValue(moment().format('YYYY-MM-DD'));
      }     
    }
    else {
      this.toastr.error("Please enter the valid payment date.")
    }
  }

  assignRevisedDate(eve) {
    let paymentdate = eve.target.value;
    this.clientform.get('revisedDateOfPayment').setValue(moment(paymentdate).format('YYYY-MM-DD'));
    this.onPaymentDateChanged(eve);
  }

  deletePayment() {
    this.confirmationDialogSrv.showDialog("Are you sure to delete Receipt No. " + this.clientform.get('receiptNo').value + " ?").subscribe(response => {
      if (response == "ok") {
        this.deleteClientPayment(this.clientform.value.installmentID);
      }
    });
  }

  deleteClientPayment(installmentID: string) {
    this.clientPaymentservice.deleteClientPayment(installmentID).subscribe(response => {
      this.toastr.success("Client payment is deleted successfully");
      this.getClientPaymentBase(this.clientform.value.ownershipID);
    });
  }

  OnNatureOfPayChanged(eve) {
    if (eve.value == 2) {
      const validators = [Validators.required];
      this.clientform.get('notConsidered').setValidators(validators);
      this.clientform.get('notConsidered').updateValueAndValidity();
      //  this.clientform.get('notConsidered').markAsTouched();
    } else {
      this.clientform.get('notConsidered').clearValidators();
      this.clientform.get('notConsidered').updateValueAndValidity();
    }
  }

  downloadExcel() {
    if (!this.validateDate())
      return;
    let model = this.searchForm.value;
   
    this.clientPaymentservice.downloadExcelByFilter(model.searchByCustomerName, model.searchBypropertyID, model.searchBypremises,
      model.searchByUnit, model.searchByLot, model.searchBySellerID, model.searchByStatus, model.fromDate, model.toDate, model.searchBynatureOfPaymentID).subscribe((response) => {
        let blob: any = new Blob([response], { type: 'application/vnd.ms-excel' });
        fileSaver.saveAs(blob, 'ClientPayment.xls');
      });
  }

  validateDate() {
    let fromDate = this.searchForm.value.fromDate;
    let toDate = this.searchForm.value.toDate;
    if (fromDate != "" && toDate != "") {
      if (moment(fromDate) > moment(toDate)) {
        this.toastr.error("FromDate should be less than ToDate");
        return false;
      }
    }
    return true;
  }

  ImportFile(eve) {

    const file = eve.target.files[0];
    let formData = new FormData();
    formData.append(file.name, file);
    var isError:boolean = false;
    this.clientPaymentservice.uploadFile(formData).subscribe(event => {
     
      if (event.type == HttpEventType.Sent) {
       
      }
    },
      (err) => {
        isError = true;
        eve.target.value = "";    
      },
      () => {
        if (!isError)
          this.toastr.success("File Uploaded successfully");
        eve.target.value = "";       
      });

    //const reader: FileReader = new FileReader();
    //let data;
    //reader.onload = (e: any) => {
    //  /* read workbook */
    //  const bstr: string = e.target.result;
    //  const wb: Xlsx.WorkBook = Xlsx.read(bstr, { type: 'binary' });

    //  //For multiple sheets
    //  for (var i = 0; i < wb.SheetNames.length; i++) {
    //    const wsname: string = wb.SheetNames[i];
    //    const ws: Xlsx.WorkSheet = wb.Sheets[wsname];

    //    /* save data */
    //    data = Xlsx.utils.sheet_to_json(ws, { header: 1 });
    //    console.log(data);
    //  }
    //};
    //reader.readAsBinaryString(files[0]);

  }

//property Filter functionality
protected filterPropertyForSearch() {
  if (!this.propertyDDl) {
    return;
  }
  // get the search keyword
  let search = this.propertyFilterCtrlForSearch.value;
  if (!search) {
    this.filteredPropertyForSearch.next(this.propertyDDl.slice());
    return;
  } else {
    search = search.toLowerCase();
  }
  // filter the banks
  this.filteredPropertyForSearch.next(this.filterProFunForSearch(search));
}

filterProFunForSearch(search) {
  var list = this.propertyDDl.filter(prop => prop.addressPremises.toLowerCase().indexOf(search) > -1);
  return list;
}
}
