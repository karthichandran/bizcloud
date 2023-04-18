import { Component, OnDestroy, OnInit, ViewChildren, QueryList, ElementRef, ViewChild } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormGroupDirective, ValidatorFn, AbstractControl, FormControl } from '@angular/forms';
import { fuseAnimations } from '@fuse/animations';
import * as _ from 'lodash';
import * as _moment from 'moment';
import { ToastrService } from 'ngx-toastr';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { LotCreationService } from './lot-creation.service';

@Component({
  selector: 'app-lot-creation',
  templateUrl: './lot-creation.component.html',
  styleUrls: ['./lot-creation.component.scss'],
  animations: fuseAnimations

})
export class LotCreationComponent implements OnInit {
  lotCreationForm: UntypedFormGroup;

  rowData: any[] = [];
  columnDef: any[] = [];

  constructor(private _formBuilder: UntypedFormBuilder, private toastr: ToastrService, private lotService: LotCreationService) {

  }

  ngOnInit() {
    this.lotCreationForm = this._formBuilder.group({
      lotId: [''],
      receivedDate: [''],
      receivedFrom: [''],
      noOfRecordsReceived: [''],
      noOfRecordsBlocked: [''],
      tdsPayment: [''],
      tdsPaymentWithCoowner: [''],
      tdsPaymentWithoutCoowner: [''],
      form16b: [''],
      form16bWithCoowner: [''],
      form16bWithoutCoowner: ['']      
    });

    this.rowData = [];
    this.columnDef = [
      { 'header': 'Lot No', 'field': 'lotNo', 'type': 'label'},
      { 'header': 'Received Date', 'field': 'dateReceived', 'type': 'label' },
      { 'header': 'No. Of Records', 'field': 'noOfRecords', 'type': 'label' },
      { 'header': 'Received from', 'field': 'receivedfrom', 'type': 'label' },
      { 'header': 'No. Of Records Blocked', 'field': 'noOfRecordsBlocked', 'type': 'label' },
      { 'header': '', 'field': '', 'type': 'button', 'activity': ['edit'], 'maxWidth': 70 }
    ];
  }

  save() {
  }

  clear() {
  }

  tabChanged(eve) { }
}
