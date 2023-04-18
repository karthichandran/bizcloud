import { Component, OnDestroy, EventEmitter, OnInit, Input, Output, OnChanges, ElementRef, Renderer2, ViewChild, ViewEncapsulation,AfterViewInit} from '@angular/core';
import { DatatableComponent } from "@swimlane/ngx-datatable";
import * as _ from 'lodash';
import {ResizeService } from 'app/shared/services/resize.service';
//import { isDefined } from '@angular/compiler/src/util';
import { ResizedEvent } from 'angular-resize-event';
//import { isUndefined } from 'util';
import {  Subscription } from 'rxjs';
@Component({
  selector: 'grid-group',
  templateUrl: './grid-group.component.html',
  styleUrls: ['./grid-group.component.scss'],
  encapsulation: ViewEncapsulation.None
  
})
export class GridGroupComponent implements OnInit,OnChanges,AfterViewInit{
  editing:any = {};
  isedit: boolean = true;
  footerHeight: number;
  rowData: any[] = [];
  columnDefs: any[] = []; 
  loadingIndicator: boolean;
  reorderable: boolean;
  scrollH: boolean;
  containerWidth: number;

  initialContainerWidth: number = 0;

  @ViewChild('table', { static: false }) table: DatatableComponent;

  @Input() dataSet: any[];
  @Input() gridColDef: any[];
  @Input() rowSelection: string;
  @Input() showFooter: boolean;
  @Input() scrollbarH: boolean;
  @Output() selectedRows: EventEmitter<any>= new EventEmitter();
  @Output() deleteRow: EventEmitter<any> = new EventEmitter();
  @Output() editRow: EventEmitter<any> = new EventEmitter();
  @Output() addRow: EventEmitter<any> = new EventEmitter();
  @Output() customRow: EventEmitter<any> = new EventEmitter();
  @Output() saveRow: EventEmitter<any> = new EventEmitter();
  @Output() checkboxEve: EventEmitter<any> = new EventEmitter();
  private subcrption: Subscription;

  constructor(private _resizeService:ResizeService,private elemRef:ElementRef) {
    this.loadingIndicator = true;
    this.reorderable = true;
  }
  ngOnInit(): void {
    this.footerHeight = this.showFooter ? 56 : 0;
    this.rowData = this.dataSet;
    this.columnDefs = this.gridColDef;   
    this.loadingIndicator = false;
    this.scrollH = this.scrollbarH;    
  
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
    //this.rowData[rowIndex][cell] = event.target.value;
    this.rowData[rowIndex][cell] = event.value;
    this.rowData = [...this.rowData];
    console.log('UPDATED!', this.rowData[rowIndex][cell]);
  }

  onCheckboxChange(rowIndex, col, val) {
    // if (isDefined(col.isMultySelect)&& col.isMultySelect==false) {
    //   _.forEach(this.rowData, o => {
    //     o[col.field] = false;
    //   });
    // }
    if (!(col.isMultySelect===undefined) && col.isMultySelect==false) {
      _.forEach(this.rowData, o => {
        o[col.field] = false;
      });
    }

    this.rowData[rowIndex][col.field] = val.checked;
    this.rowData = [...this.rowData];

    this.checkboxEve.emit();
  }

  getValue(value,options) {
   let opt= _.filter(options, function (o) {
      return o.id == value;
   });
    return opt == "" ? "" : opt[0].description;
  }

  selectAllCheckbox(field, eve) {
    for (let i = 0; i < this.rowData.length; i++) {
      this.rowData[i][field] = eve.checked;
    }
    this.rowData = [...this.rowData];
    this.checkboxEve.emit();
  }
  showInline(field) {
    var data = {};
     _.forEach(this.editing, function (item,key) {
       data[key] = false;
     });
    this.editing = data;
    this.editing[field] = true;
  }

  toggleExpandRow(row) {
    this.rowData = row;
  }

  deleteAction( row: any) {
    this.deleteRow.emit(row);
  }

  editAction( row: any) {
    this.editRow.emit(row);
  }

  addAction(row: any) {
    this.addRow.emit(row);
  }

  customAction(row: any) {
    this.customRow.emit(row);
  }
  saveAction(row: any) {
    this.saveRow.emit(row);
  }

  
  onResized(eve: ResizedEvent) {
    // if (isUndefined(eve))
    if (eve===undefined)
      return;

    // if (isUndefined(eve.oldWidth))
    if (eve.oldRect===undefined)
      return;


    if (eve.newRect.width >eve.oldRect.width) {
      let diff = eve.newRect.width - eve.oldRect.width -84;
      this.containerWidth = this.initialContainerWidth + diff;     
    }
    else {
      let diff = eve.oldRect.width- eve.newRect.width ;
      this.containerWidth = this.containerWidth - diff+64;
    }
    console.log("container width " + this.containerWidth);
        
    this.rowData = [...this.rowData];
    if (!(this.table===undefined)) {
      this.table.recalculate();
    }

  }

  sortable(colm: any) {
    let columninx = _.findIndex(this.columnDefs, o => { return o.field == colm; });
    if (this.columnDefs[columninx].sort===undefined) {
      this.columnDefs[columninx]['sort'] = 'desc';
    }

    if ( this.columnDefs[columninx].sort == 'desc') {
      this.rowData = _.orderBy(this.rowData, [colm], ['asc']);
      this.columnDefs[columninx].sort = 'asc';
    }
    else {
      this.rowData = _.orderBy(this.rowData, [colm], ['desc']);
      this.columnDefs[columninx].sort = 'desc';
    }
  }

  toggleExpandGroup(group) {
    console.log('Toggled Expand Group!', group);
    this.table.groupHeader.toggleExpandGroup(group);
  }

  onDetailToggle(event) {
    console.log('Detail Toggled', event);
  }
}
