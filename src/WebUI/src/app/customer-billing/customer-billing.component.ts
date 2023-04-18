import { Component, OnDestroy, OnInit, ViewChildren, QueryList, ElementRef, ViewChild } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormGroupDirective, ValidatorFn, AbstractControl, FormControl } from '@angular/forms';
import { fuseAnimations } from '@fuse/animations';
import * as _ from 'lodash';
import * as moment from 'moment';
import { PropertyService } from '../property/property.service';
import { ToastrService } from 'ngx-toastr';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { CustomerBillingService } from './customer-billing.service';
import { ConfirmationDialogService } from '../core/confirmation-dialog/confirmation-dialog.service';
import { TaxService } from '../tax/tax.service';
import * as Xlsx from 'xlsx';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomerBillingDialogComponent } from './customer-billing-dialog/customer-billing-dialog.component';
import * as fileSaver from 'file-saver';

@Component({
  selector: 'app-customer-billing',
  templateUrl: './customer-billing.component.html',
  styleUrls: ['./customer-billing.component.scss'],
  animations: fuseAnimations
 
})
export class CustomerBillingComponent implements OnInit {
  customerBillingForm: UntypedFormGroup;
  billFilter: UntypedFormGroup;

  premises: any[] = [];
  payableBy: any[] = [{ 'id': 1, 'description': 'Seller' }, { 'id': 2, 'description': 'Customer' }];
  paymentMethods: any[] = [{ 'paymentMethodID': 1, 'paymentMethod': 'Lumpsum' }, { 'paymentMethodID': 2, 'paymentMethod': 'Installment' }];
  customers: any[] = [];
  units: any[] = [];
  gstCodes: any[] = [];
  customerBill: any = {};
  propertyDDl: any[] = [];
  propertyList: any[] = [];
  //grid
  rowData: any[] = [];
  columnDef: any[] = [];
  
  constructor(private _formBuilder: UntypedFormBuilder, private propertyService: PropertyService, private customerBillingService: CustomerBillingService,
    private toastr: ToastrService, private confirmationDialogSrv: ConfirmationDialogService, private taxService: TaxService, private dialog: MatDialog ) {
  }

  ngOnInit() {
  

    this.customerBillingForm = this._formBuilder.group({
      customerBillID: [''],
      ownershipID: [''],
      coOwner: ['no'],
      customerID: [''],
      customerName: [{ value: '', disabled: true }],
      unitNo: [{ value: '', disabled: true }],
      propertyPremises: [{ value: '', disabled: true }],
      propertyID: [''],
      payableBy: ['', Validators.required],
      payableByText: [{ value: '', disabled: true }],
      paymentMethodID: [''],
      paymentMethodText:[''],
      amount: ['', Validators.required],
      gstRate: ['', Validators.required],
      gstAmount: [{ value: '', disabled: true }],
      totalPayable: [{ value: '', disabled: true }],
      noOfInstallments:[''],
      costPerInstallment: [''],
      billDate:['']
    });

    this.billFilter = this._formBuilder.group({
      searchByCustomerName: [''],
      searchByPremises: [''],
      searchBypaymentType: [''],
      searchByUnit: [''],
      searchByPropertyID: [''],
      fromDate: [''],     
      toDate: ['']
    });

    this.getAllProperties();

    this.columnDef = [
      { 'header': '', 'field': '', 'type': 'button','activity':['edit'], 'maxWidth': 80 },
      { 'header': 'Invoice No.', 'field': 'customerBillID', 'type': 'label' },
      { 'header': 'Customer', 'field': 'customerName', 'type': 'label', 'minWidth': 300 },
      { 'header': 'Premises', 'field': 'propertyPremises', 'type': 'label', 'minWidth': 300 },
      { 'header': 'Unit No', 'field': 'unitNo', 'type': 'label' },
      { 'header': 'Amount', 'field': 'amount', 'type': 'label' },
      { 'header': 'GST Rate', 'field': 'gstRate', 'type': 'label' },
      { 'header': 'GST Amount', 'field': 'gstAmount', 'type': 'label' },
      { 'header': 'Total Payable', 'field': 'totalPayable', 'type': 'label' },
      { 'header': 'Bill Date', 'field': 'billDate', 'type': 'label' },
      { 'header': '', 'field': '', 'type': 'button','activity':['delete'], 'maxWidth': 80}    
    ];

    this.GetTaxCodes();
    this.search();
  }  

  clear() {
    this.customerBillingForm.reset();
  }

  tabChanged(eve: MatTabChangeEvent) {   
    if (eve.index == 0) {
      this.search();
    }
  }

  editRow(eve) {
    //this.getCusBillingById(eve.customerBillID);
    this.openDialog(eve.customerBillID);
  }

  deleteRow(eve) {
    this.confirmationDialogSrv.showDialog("Are you sure to delete?").subscribe(response => {
      if (response == "ok") {
        this.deleteCustomerBill(eve.customerBillID);
      }
    });
  }

  addNewBill(eve) {
    this.customerBillingService.getCusBillBaseModelByOwnershipId(eve.ownershipID).subscribe((response) => {
      this.fillModel(response);
      var ele = document.getElementsByClassName('mat-tab-label') as HTMLCollectionOf<HTMLElement>;
      ele[0].click();
    });
  }


  deleteCustomerBill(id:number) {
    this.customerBillingService.deleteCusBill(id).subscribe(response => {
      this.toastr.success("Cusastomer Bill delete successfull");
      this.search();
    });
}

  GetTaxCodes() {
    this.taxService.getTaxCodes().subscribe((response) => {
      let taxCodes = _.groupBy(response, function (item) {
        return item.taxTypeId == 1;
      });
      this.gstCodes = taxCodes.true;     
    });
  }

  getAllProperties() {
    this.propertyService.getProperties().subscribe((response) => {
      this.propertyList = response;
      this.propertyDDl = response;
      this.propertyDDl.splice(0, 0, { 'propertyID': '', 'addressPremises': '' });
    });
  }

  getCusBillingById(id:string) {
    this.customerBillingService.getCusBillByBillId(id).subscribe((response) => {
      this.fillModel(response);
      this.onChangePaymentMethod({ 'value': response.paymentMethodID});
      var ele = document.getElementsByClassName('mat-tab-label') as HTMLCollectionOf<HTMLElement>;
      ele[1].click();

    });
  }

  getUpdatedCusBillingById(id: string) {
    this.customerBillingService.getCusBillByBillId(id).subscribe((response) => {
      this.fillModel(response);
    });
  }
  fillModel(res: any) {
    
    this.customerBill = res;
    this.customerBill.coOwner = res.coOwner == true ? 'yes' : 'no';
    res.gstRate = res.gstRate == '0' ? 18 : this.getTaxValue(res.gstRate);
    res.gstAmount = res.gstAmount == 0 ? '' : res.gstAmount;
    res.totalPayable = res.totalPayable == 0 ? '' : res.totalPayable;
    this.customerBillingForm.patchValue(res);
  }

  getTaxValue(val: number) {
    let tax = val.toString().split('.');
    if (tax[1] == '00')
      return parseInt(tax[0]);
    else
      return val;   
  }

  search() {
    if(!this.validateDate())
    return;
    let model = this.billFilter.value;
    this.customerBillingService.getCusBillsByFilter(model.searchByCustomerName, model.searchByPropertyID,
      model.searchByPremises, model.searchByUnit, model.searchBypaymentType, model.fromDate, model.toDate).subscribe((response) => {
      _.forEach(response, o => {
        o.gstRate = o.gstRate + ' %';
        o.billDate = moment(o.billDate).format("DD-MMM-YYYY");
      });

        this.rowData=response;
      })
  }

  reset() {
    this.billFilter.reset();
    this.search();
  }

  save() {
    if (this.customerBillingForm.valid && this.validateField()) {
      let model = this.customerBillingForm.value;
      model.customerBillID = (model.customerBillID == "" || model.customerBillID == null) ? 0 : model.customerBillID;
      let isNew = model.customerBillID == 0 ? true : false;
      model.coOwner = model.coOwner == 'yes' ? true : false;
      model.gstRate = model.gstRate == '0' ? 18.00 : model.gstRate;
      model.billDate = moment(model.billDate).local().format("DD-MMM-YYYY");

      this.customerBillingService.saveCustomerBill(model, isNew).subscribe((response) => {
        this.toastr.success("Customer bill save successfull");
        this.getUpdatedCusBillingById(response.customerBillID);
      });
    }
    else {
      Object.keys(this.customerBillingForm.controls).forEach(field => {
        const control = this.customerBillingForm.get(field);
        control.markAsTouched({ onlySelf: true });
      });
    }
  }

  validateField() {
    let payableBy = this.customerBillingForm.get('payableBy').value;
    let amount = this.customerBillingForm.get('amount').value;
    let gst = this.customerBillingForm.get('gstRate').value;
    if (payableBy == 0) {
      this.toastr.error('Please select the payable');
      return false;
    }

    if (amount == 0) {
      this.toastr.error('Please enter the amount');
      return false;
    }

    if (gst == 0) {
      this.toastr.error('Please select the GST Rate');
      return false;
    }
    return true;
  }

  onChangePaymentMethod(eve) {
    let noOfInstallmentsControl = this.customerBillingForm.get('noOfInstallments');
    let costPerInstallmentControl = this.customerBillingForm.get('costPerInstallment')
    if (eve.value == 1) {
      noOfInstallmentsControl.disable({ onlySelf: true });
      costPerInstallmentControl.disable({ onlySelf: true });
      noOfInstallmentsControl.clearValidators();
      noOfInstallmentsControl.updateValueAndValidity();
      costPerInstallmentControl.clearValidators();
      costPerInstallmentControl.updateValueAndValidity();
     
    }
    else {
      noOfInstallmentsControl.enable({ onlySelf: true });
      costPerInstallmentControl.enable({ onlySelf: true });
      noOfInstallmentsControl.setValidators([Validators.required]);
      noOfInstallmentsControl.updateValueAndValidity();
      costPerInstallmentControl.setValidators([Validators.required]);
      costPerInstallmentControl.updateValueAndValidity();
    }
  }

  onChangeInstallmentAndCost() {
    let noOfInstallment: number = this.customerBillingForm.get('noOfInstallments').value;
    let costPerInstallment: number = this.customerBillingForm.get('costPerInstallment').value;
    if (noOfInstallment == 0 || costPerInstallment == 0)
      return;
    let amount = noOfInstallment * costPerInstallment;
    this.customerBillingForm.get('amount').setValue(amount);
  }

  validateDate() {
    let fromDate = this.billFilter.value.fromDate;
    let toDate = this.billFilter.value.toDate;
    if (fromDate != "" && toDate != "") {
      if (moment(fromDate) > moment(toDate)) {
        this.toastr.error("From Date should be less than To Date");
        return false;
      }       
    }
    return true;
  }

  download() {
    if (!this.validateDate())
      return;
    let model = this.billFilter.value;
    this.customerBillingService.downloadBillsByFilter(model.searchByCustomerName, model.searchByPropertyID,
      model.searchByPremises, model.searchByUnit, model.searchBypaymentType, model.fromDate, model.toDate).subscribe((response) => {
        let blob: any = new Blob([response], { type: 'application/vnd.ms-excel' });
        fileSaver.saveAs(blob, 'CustomerBilling.xls');
      })
    //let reportData: any[] = [];
    //_.forEach(this.rowData, o => {
    //  reportData.push({
    //    'Bill Date': o.billDate,
    //    'Customer Name': o.customerName,
    //    'PAN': o.pan,
    //    'Property Premises': o.propertyPremises,
    //    'Unit No': o.unitNo,
    //    'is Co-Owner': o.coOwner,
    //    'Payment Method': o.paymentMethodText,
    //    'Payable By': o.payableByText,
    //    'No.of Installments': o.noOfInstallments,
    //    'Charge Per Installment': o.costPerInstallment,
    //    'Amount': o.amount,
    //    'GST Rate': o.gstRate,
    //    'GST Amount': o.gstAmount,
    //    'Total Payable': o.totalPayable
    //  });
    //});

    //const ws: Xlsx.WorkSheet = Xlsx.utils.json_to_sheet(reportData);
    //const wb: Xlsx.WorkBook = Xlsx.utils.book_new();
    //Xlsx.utils.book_append_sheet(wb, ws, "Sheet1");
    //Xlsx.writeFile(wb, 'CustomerBilling.xls');
  }

  openDialog(id: string): void {
    const dialogRef = this.dialog.open(CustomerBillingDialogComponent, {
      hasBackdrop: false,
      maxHeight: 650,
      maxWidth: 1000,
      width: "800px",
      data: {
        'premises': this.propertyList, 'paymentMethods': this.paymentMethods, 'gstRate': this.gstCodes, 'ownershipId': id,'isNew':false
      }

    });

    dialogRef.afterClosed().subscribe(() => {
      this.search();
      console.log('The dialog was closed');
    });
  }
}
