import { Component, OnDestroy, OnInit, ViewChild, ElementRef } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormGroupDirective } from '@angular/forms';
import { MatTabChangeEvent} from '@angular/material/tabs';
import { fuseAnimations } from '@fuse/animations';
import * as Xlsx from 'xlsx';
import { PropertyService } from '../property/property.service';
import { StatesService } from '../shared/services/states.service';
import {SellerService } from '../seller/seller.service';
import { IPropertyVM } from '../property/property.model';
import { ToastrService } from 'ngx-toastr';
import { GridComponent} from '../../@fuse/components/grid/grid.component';
import * as _ from 'lodash';
import { TaxService } from '../tax/tax.service';

@Component({
  selector: 'app-seller',
  templateUrl: './property.component.html',
  styleUrls: ['./property.component.scss'],
  animations: fuseAnimations
})
export class PropertyComponent implements OnInit, OnDestroy {
  @ViewChild(GridComponent) gridComp: GridComponent;

  form: UntypedFormGroup;
  sellers: any[] = [];
  sellersRawData = [];
  states: any[] = [];
  sellerList: any[] = [];
  rowData: any[] = [];
  columnDef: any[] = [];
  sellerData: any[] = [];
  sellerColumnDef: any[] = [];
  gstCode: any[] = [{ 'id': 1, 'description': '15%' }];
  tdsCode: any[] = [{ 'id': 1, 'description': '15%' }];
  typeOfProperty: any[] = [{ 'id': 1, 'description': 'Land' }, { 'id': 2, 'description': 'Building' }];
  searchOptions: any[] = [{ 'id': 1, 'description': 'Premises' }];
  isRadioButtonTouched: boolean = true;

  searchByPremises: string;
  searchByPan: string;
  searchBySellerName: string;

  showListGrid: boolean;
  showPercGrid: boolean;

  sellerProperty: any[] = [];
  constructor(private _formBuilder: UntypedFormBuilder, private propertyService: PropertyService, private statesService: StatesService, private sellerService: SellerService, private toastr: ToastrService, private taxService: TaxService) {

  }

  ngOnInit(): void {
    // Reactive Form
    this.form = this._formBuilder.group({
      propertyID: [''],
      propertyType: [2, Validators.required],
      addressPremises: [''],
      addressLine1: ['', Validators.required],
      addressLine2: [''],
      city: ['', Validators.required],
      stateID: ['', Validators.required],
      pinCode: ['', Validators.required],
      gstTaxCode: ['', Validators.required],
      tdsTaxCode: ['', Validators.required],
      tdsInterestRate: ['', Validators.required],
      lateFeePerDay: ['', Validators.required],     
      share: [''],
      propertyCode: [''],
      propertyShortName: [''],
      isActive:['true']
    });

    this.columnDef = [
      { 'header': 'Premises', 'field': 'addressPremises', 'type': 'label' },
      { 'header': 'Short Name', 'field':'propertyShortName','type':'label'},
      { 'header': 'Seller Name', 'field': 'sellerNames', 'type': 'labellist' },
      { 'header': 'Seller PAN', 'field': 'panNumbers', 'type': 'labellist' },
      { 'header': 'Is Active', 'field': 'isActive', 'type': 'label' }     
    ];

    this.sellerColumnDef = [{ 'header': 'Name', 'field': 'sellerID', 'type': 'selection','options':this.sellerList },
      { 'header': 'Share %', 'field': 'sellerShare', 'type': 'textbox' } 
      //{ 'header': 'Mark for Delete', 'field': 'markForDelete', 'type': 'checkbox' }
    ];
    this.getAllStates();
    this.getAllSellers();
    this.getTaxCodes();
    this.showListGrid = false;
    this.showPercGrid = true;
  }

  clear() {
    this.form.reset();
    this.sellerData = [];
    this.form.get('propertyType').setValue(2);
    this.form.get('isActive').setValue('true');
  }

  save(): void {
    if (this.form.valid && this.validatesellerAndSharePercentage()) {
      let model = this.form.value;
      let propertyModel: IPropertyVM = {};
      propertyModel.propertyDto = model;

      let sellersList = this.sellerData;
      _.forEach(sellersList, o => {
        if (sellersList.length > 1)
          o.isSharedSeller = true;
        else
          o.isSharedSeller = false;
        });
     
      propertyModel.sellerProperties = sellersList;
      
      this.propertyService.saveProperty(propertyModel).subscribe((response) => {
        this.toastr.success("Saved Successfully");
        this.clear();
      }, error => this.toastr.error(error));
    } else {
      Object.keys(this.form.controls).forEach(field => {
        const control = this.form.get(field);
        control.markAsTouched({ onlySelf: true });
      });
    }
  }

  validatesellerAndSharePercentage(): boolean {

    if (this.sellerData.length == 0) {
      this.toastr.error("Minimum one seller must be added");
      return false;
    }
    
    let group = _.groupBy(this.sellerData, function (item) { return item.sellerID });
    if (Object.keys(group).length !== this.sellerData.length) {
      this.toastr.error("duplicating sellers are not allowed.");
      return false;
    }

    let share: number = 0;
    let toastr = this.toastr;
    let isShareValid: boolean = true;
    _.forEach(this.sellerData, function (item) {
      let val = parseFloat(item.sellerShare);
      if (isNaN(val)) {
        isShareValid = false;
      }
      if (!item.markForDelete)
      share += val;
    });
    if (!isShareValid) {
      toastr.error("Please enter valid share value");
      return false;
    }
    if (share != 100 && share > 0)
      this.toastr.error("Sum of share % must be equal to 100");
    return share == 100 || share == 0;
  }
  /**
     * On destroy
     */
  ngOnDestroy(): void {
  }

  selectedRows(eve) {   
    this.form.patchValue(eve.selected[0]);
    var ele = document.getElementsByClassName('mat-tab-label') as HTMLCollectionOf<HTMLElement>;
    ele[0].click();
    this.getPropertyById(eve.selected[0].propertyId);   
  }  
   
  getAllSellers() {
    this.sellerService.getSellers().subscribe((response) => {
      this.sellersRawData = response;
      this.sellerList = _.map(response, function (item) {
        return{ 'id': item.sellerID, 'description': item.sellerName };
      })

      this.sellerColumnDef[0].options = this.sellerList;
    });
  }

  formatRowData(response: any) {
    function splitData(data: string) {
      let splitData = data.split(',');

      let concatdata: any[] = [];
      _.forEach(splitData, o => {
        concatdata.push({ 'name': o });
      });
      return concatdata;
    }
    _.forEach(response, o => {
      o.sellerNames = splitData(o.sellerNames);
      o.panNumbers = splitData(o.panNumbers);
      o.isActive = o.isActive == null ? true : o.isActive;
    })
    this.rowData = response;
  }

  getPropertyById(id:number) {
    this.propertyService.getPropertyEntry(id).subscribe((response) => {
      this.form.patchValue(response.propertyDto);
      if (response.propertyDto.isActive == null) {
        this.form.get('isActive').setValue('true');
      }
      this.sellerData = response.sellerProperties;    
    });
  }

  getAllStates() {
    this.statesService.getStates().subscribe((response) => {
      this.states = response;
    });
  }

  getTaxCodes() {
    this.taxService.getTaxCodes().subscribe((response) => {
      let taxCodes = _.groupBy(response, function (item) {
        return item.taxTypeId == 1;
      });
      this.gstCode = taxCodes.true;
      this.tdsCode = taxCodes.false;
    });
  }
 
  addCoSeller() {
    this.sellerData = [{ 'sellerID': '', 'sellerShare': '' }, ...this.sellerData];
    this.gridComp.showInline("0sellerID");    
  }

  tabChanged(eve: MatTabChangeEvent) {
    //console.log('tabChangeEvent => ', eve);
    //console.log('index => ', eve.index);
    if (eve.index == 1) {
      this.search();
      this.showListGrid = true;
      this.showPercGrid = false;
    }
    else {
      this.showListGrid = false;
      this.showPercGrid = true;
    }
  }

  search() {   
    this.propertyService.getSellerPropertiesByFilter(this.searchByPremises,this.searchByPan,this.searchBySellerName).subscribe((response) => {
       this.formatRowData(response)
      });
  } 

  reset() {
    this.searchByPremises = "";
    this.searchByPan = "";
    this.searchBySellerName = "";
    this.search();
  }  
}
