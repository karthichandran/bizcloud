import { Component, OnInit, Inject, ViewEncapsulation} from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormGroupDirective, ValidatorFn, AbstractControl, FormControl } from '@angular/forms';
import * as _ from 'lodash';
import { CustomerBillingService} from '../customer-billing.service';
import { ToastrService } from 'ngx-toastr';
import * as moment from 'moment';
@Component({
  selector: 'app-customer-billing-dialog',
  templateUrl: './customer-billing-dialog.component.html',
  styleUrls: ['./customer-billing-dialog.component.scss']
})
export class CustomerBillingDialogComponent implements OnInit{
  customerBillingForm: UntypedFormGroup;

  premises: any[] = [];
  payableBy: any[] = [{ 'id': 1, 'description': 'Seller' }, { 'id': 2, 'description': 'Customer' }];
  paymentMethods: any[] = [{ 'paymentMethodID': 1, 'paymentMethod': 'Lumpsum' }, { 'paymentMethodID': 2, 'paymentMethod': 'Installment' }];
  customers: any[] = [];
  units: any[] = [];
  gstCodes: any[] = [];
  ownershipId: string;
  customerBill: any = {};
  isNew: boolean;

  constructor(public dialogRef: MatDialogRef<CustomerBillingDialogComponent>, private _formBuilder: UntypedFormBuilder, @Inject(MAT_DIALOG_DATA) data, private customerBillingService: CustomerBillingService, private toastr: ToastrService) {
    this.premises = data.premises;
    this.paymentMethods = data.paymentMethods;
    this.gstCodes = data.gstRate;
    this.ownershipId = data.ownershipId;
    this.isNew = data.isNew;
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
      payableBy: [2, Validators.required],
      payableByText: [{ value: '', disabled: true }],
      paymentMethodID: [''],
      amount: ['', Validators.required],
      gstRate: ['', Validators.required],
      gstAmount: [{ value: '', disabled: true }],
      totalPayable: [{ value: '', disabled: true }],
      noOfInstallments: [''],
      costPerInstallment: [''],
      billDate:['']
    });
    this.getCustomerBill(this.ownershipId);
  }

  getCustomerBill(id: string) {
    if(this.isNew)
      this.customerBillingService.getCusBillBaseModelByOwnershipId(id).subscribe((response) => {
        if (response.customerBillID != 0) {
          this.toastr.warning("Customer Bill Already Exist. Modify as Necessary");
         
        }
      this.fillModel(response);
      this.onChangePaymentMethod({ 'value': response.paymentMethodID });
    });
    else
    this.customerBillingService.getCusBillByBillId(id).subscribe((response) => {
      this.fillModel(response);
      this.onChangePaymentMethod({ 'value': response.paymentMethodID });  
    });
  }

 

  save() {
    if (this.customerBillingForm.valid && this.validateField()) {
      let model = this.customerBillingForm.value;
      model.customerBillID = (model.customerBillID == "" || model.customerBillID == null) ? 0 : model.customerBillID;
      let isNew = model.customerBillID == 0 ? true : false;
      model.coOwner = model.coOwner == 'yes' ? true : false;
      model.gstRate = model.gstRate == '0' ? 18 : model.gstRate;   
      model.billDate = moment(model.billDate).local().format("DD-MMM-YYYY");

      this.customerBillingService.saveCustomerBill(model, isNew).subscribe((response) => {
        this.toastr.success("Customer billing Is saved successfully");
        this.getCusBillingById(response.customerBillID);
      });
    }
    else {
      Object.keys(this.customerBillingForm.controls).forEach(field => {
        const control = this.customerBillingForm.get(field);
        control.markAsTouched({ onlySelf: true });
      });
    }
  }

  getCusBillingById(id: string) {
    this.customerBillingService.getCusBillByBillId(id).subscribe((response) => {
      this.fillModel(response);   

      
    });
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

  fillModel(res: any) {
    this.customerBill = res;
    this.customerBill.coOwner = res.coOwner == true ? 'yes' : 'no';
    res.gstRate = res.gstRate == '0' ? 18 : this.getTaxValue(res.gstRate);
    res.gstAmount = res.gstAmount == 0 ? '' : res.gstAmount;
    res.totalPayable = res.totalPayable == 0 ? '' : res.totalPayable;
    res.payableBy = res.payableBy == 0 ? 2 : res.payableBy;
    this.customerBillingForm.patchValue(res);
  }

  getTaxValue(val: number) {
    let tax = val.toString().split('.');
    if (tax[1] == '00')
      return parseInt(tax[0]);
    else
      return val;
  }

  close() {
    this.dialogRef.close();
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
  
}


