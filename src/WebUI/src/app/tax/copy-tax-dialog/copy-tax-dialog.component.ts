import { Component, OnInit, Inject, ViewEncapsulation } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormGroupDirective, ValidatorFn, AbstractControl, FormControl } from '@angular/forms';
import * as _ from 'lodash';
import { ToastrService } from 'ngx-toastr';
import * as _moment from 'moment';
import { TaxService } from '../tax.service';
@Component({
  selector: 'app-copy-tax-dialog',
  templateUrl: './copy-tax-dialog.component.html'
})
export class CopyTaxDialogComponent implements OnInit {
  copyTaxForm: UntypedFormGroup;
  taxCodeData: any = {};

  constructor(public dialogRef: MatDialogRef<CopyTaxDialogComponent>, private _formBuilder: UntypedFormBuilder, @Inject(MAT_DIALOG_DATA) data, private toastr: ToastrService, private taxService: TaxService) {
    this.taxCodeData = data.formData;
  }

  ngOnInit() {
    this.copyTaxForm = this._formBuilder.group({
      taxID: [''],
      taxCodeId: [{ value: '', disabled: true }],
      taxTypeId: [1, Validators.required],
      taxName: ['', Validators.required],
      taxValue: ['', Validators.required],
      effectiveStartDate: ['', Validators.required],
      effectiveEndDate: ['', Validators.required]
    });
    this.copyTaxForm.patchValue(this.taxCodeData);
  }

  save() {
    if (this.copyTaxForm.valid) {
      let model = this.copyTaxForm.value;
      model.taxCodeId = this.taxCodeData.taxCodeId;

      model.effectiveStartDate = _moment(model.effectiveStartDate).local().format("YYYY-MM-DD");
      model.effectiveEndDate = _moment(model.effectiveEndDate).local().format("YYYY-MM-DD");
      this.taxService.copyTax(model).subscribe(response => {
        this.toastr.success("Tax Saved Successfully");
        this.dialogRef.close();
      });
    }
    else {
      Object.keys(this.copyTaxForm.controls).forEach(field => {
        const control = this.copyTaxForm.get(field);
        control.markAsTouched({ onlySelf: true });
      });
    }
  }

  close() {
    this.dialogRef.close();
  }

}


