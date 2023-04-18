import { Component, OnDestroy, EventEmitter, OnInit, Input, Output, OnChanges, ElementRef, Renderer2, ViewChild, ViewEncapsulation, AfterViewInit} from '@angular/core';
import { DatatableComponent } from "@swimlane/ngx-datatable";
import { MatAutocompleteTrigger} from '@angular/material/autocomplete';
import * as _ from 'lodash';
import * as moment from 'moment';
import {ClientService } from '../../../../app/client/client.service';
//import { isUndefined } from 'util';
//import { isDefined } from '@angular/compiler/src/util';
import { ResizedEvent } from 'angular-resize-event';
import { Subscription } from 'rxjs';
import { ResizeService } from 'app/shared/services/resize.service';
@Component({
  selector: 'grid-client',
  templateUrl: './grid-client.component.html',
  styleUrls: ['./grid-client.component.scss'],
  encapsulation: ViewEncapsulation.None

})
export class GridClientComponent implements OnInit, OnChanges, AfterViewInit {

  paymentMethods: any[] = [{ 'paymentMethodID': 1, 'paymentMethod': 'Lumpsum' }, { 'paymentMethodID': 2, 'paymentMethod': 'Installment' }];
  statusDDl: any[] = [{ 'id': 1, 'description': 'Saved' }, { 'id': 2, 'description': 'Submitted' }, { 'id': 3, 'description': 'Cancelled' }, { 'id': 4, 'description': 'Assigned' }, { 'id': 5, 'description': 'Blocked' }, { 'id': 6, 'description': 'Released' }, { 'id': 7, 'description': 'Archive' }];
  groupData: any[] = [];
  remarks: any[] = [];
  containerWidth: number;

  test: any;
  editing: any = {};
  isedit: boolean = true;
  rowData: any[] = [];
  columnDefs: any[] = [];
  loadingIndicator: boolean;
  reorderable: boolean;
  triggerInterval: number = 0;
  initialContainerWidth: number = 0;

  @ViewChild('table', { static: false }) table: DatatableComponent;


  @Input() dataSet: any[];
  @Input() gridColDef: any[];
  @Input() rowSelection: string;
  @Output() selectedRows = new EventEmitter();
  @Output() action = new EventEmitter();
  private subcrption: Subscription;

  constructor(private clientService: ClientService, private _resizeService: ResizeService, private elemRef: ElementRef) {
    this.loadingIndicator = true;
    this.reorderable = true;
  }
  ngOnInit(): void {
    this.rowData = this.dataSet;
    this.columnDefs = this.gridColDef;
    this.loadingIndicator = false; 
    this.getRemarks();

    this.subcrption = this._resizeService.resize.subscribe(res => {
      this.onResized(res);
    });   
  }
  ngAfterViewInit(): void {
    this.initialContainerWidth = this.elemRef.nativeElement.parentElement.offsetWidth;
  }
  ngOnDestroy(): void {
    this.subcrption.unsubscribe();
  }

  getRemarks() {
    this.clientService.getRemarks().subscribe((response) => {
      this.remarks = response;
    });
  }


  ngOnChanges(changes: any): void {
    this.rowData = changes.dataSet.currentValue;
    this.rowData = [...this.rowData];
    if (this.table && this.table.recalculate) {
      this.table.recalculate();
      window.dispatchEvent(new Event('resize'));
    }
    // this.table.recalculate();
  }

  onSelect(eve) {
    var tet = eve;
    this.selectedRows.emit(eve);
  }

  updateValue(event, cell, rowIndex) {
    console.log('inline editing rowIndex', rowIndex);
    this.editing[rowIndex + cell] = false;
    this.rowData[rowIndex][cell] = event.value;
    this.rowData = [...this.rowData];
    this.editing[rowIndex + cell] = false;
    console.log('UPDATED!', this.rowData[rowIndex][cell]);
  }

  updateRemark(event: any,trigger, cell, rowIndex: any) {
    //let isOpen = isUndefined(trigger.autocomplete)?false: trigger.autocomplete.isOpen;
    let oldVal = this.rowData[rowIndex].remarks;
    let newVal = event.target.value;
    if (oldVal == newVal) {
      this.triggerInterval++;
      if (this.triggerInterval < 2) {
        return;
      }
      else
        this.triggerInterval = 0;
    } else
      this.triggerInterval = 0;

   // console.log("old :" + oldVal + " newVal :" + newVal + " isOPen :" + isOpen);

    this.editing[rowIndex + 'remarks'] = false;
    this.rowData[rowIndex][cell] = event.target.value;
    this.rowData = [...this.rowData];
    this.editing[rowIndex + cell] = false;
  }

  onCheckboxChange(rowIndex, field, val) {
    this.rowData[rowIndex][field] = val.checked;
    this.rowData = [...this.rowData];
  }

  getValue(value, options,id,description) {
    let opt = _.filter(options, function (o) {
      return o[id] == value;
    });
    return opt == "" ? "" : opt[0][description];
  }

  selectAllCheckbox(field, eve) {
    for (let i = 0; i < this.rowData.length; i++) {
      this.rowData[i][field] = eve.checked;
    }
    this.rowData = [...this.rowData];
  }
  showInline(field) {
    var data = {};
    _.forEach(this.editing, function (item, key) {
      data[key] = false;
    });
    this.editing = data;
    this.editing[field] = true;
  }

  toggleExpandRow(row) {
    this.rowData = row;
  }

  save(id: any,row:any) {
    let model = { 'id': id ,'action':'save','row':row};
    this.action.emit(model);
  }

  edit(row: any) {
    let id = row.ownershipID == "00000000-0000-0000-0000-000000000000" ? row.pan : row.ownershipID; 
  
    let model = { 'id': id, 'action': 'edit' };
    this.action.emit(model);
  }
  email(row: any) {
    let id = row.ownershipID == "00000000-0000-0000-0000-000000000000" ? row.pan : row.ownershipID; 
  
    let model = { 'id': id, 'action': 'email' };
    this.action.emit(model);
  }
  // onResized(eve: ResizedEvent) {
  //   // if (isUndefined(eve))
  //   if (eve===undefined)
  //     return;

  //   // if (isUndefined(eve.oldWidth))
  //   if (eve.oldWidth===undefined)
  //     return;

  //   if (eve.newWidth > eve.oldWidth) {
  //     let diff = eve.newWidth - eve.oldWidth - 84;
  //     this.containerWidth = this.initialContainerWidth + diff;
  //   }
  //   else {
  //     let diff = eve.oldWidth - eve.newWidth;
  //     this.containerWidth = this.containerWidth - diff + 64;
  //   }
  //   console.log("container width " + this.containerWidth);

  //   this.rowData = [...this.rowData];
  //   // if (isDefined(this.table)) {
  //   //   this.table.recalculate();
  //   // }
  //   if (!(this.table===undefined)) {
  //     this.table.recalculate();
  //   }
  // }

  onResized(eve: ResizedEvent) {
    if (eve===undefined)
      return;

    if (eve.oldRect===undefined)
      return;

    if (eve.newRect.width > eve.oldRect.width) {
      let diff = eve.newRect.width -  eve.oldRect.width - 84;
      this.containerWidth = this.initialContainerWidth + diff;
    }
    else {
      let diff = eve.oldRect.width -eve.newRect.width;
      this.containerWidth = this.containerWidth - diff + 64;
    }
    console.log("container width " + this.containerWidth);

    this.rowData = [...this.rowData];
    // if (isDefined(this.table)) {
    //   this.table.recalculate();
    // }
    if (!(this.table===undefined)) {
      this.table.recalculate();
    }
  }
 
}
