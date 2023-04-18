import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { fuseAnimations } from '@fuse/animations';
import { PropertyService } from '../property/property.service';
import { StatesService } from '../shared/services/states.service';
import { ToastrService } from 'ngx-toastr';
import { GridComponent } from '../../@fuse/components/grid/grid.component';
import * as _ from 'lodash';
import { TaxService } from '../tax/tax.service';
import { ProspectService } from './prospect.service';
import * as _moment from 'moment';
import { ConfirmationDialogService } from '../core/confirmation-dialog/confirmation-dialog.service';
@Component({
  selector: 'app-prospect',
  templateUrl: './prospect.component.html',
  styleUrls: ['./prospect.component.scss'],
  animations: fuseAnimations
})
export class ProspectComponent implements OnInit, OnDestroy {
  @ViewChild(GridComponent) gridComp: GridComponent;

  propertyForm: UntypedFormGroup;
  states: any[] = [];
  rowData: any[] = [];
  columnDef: any[] = [];
  gstCode: any[] = [{ 'id': 1, 'description': '15%' }];
  tdsCode: any[] = [{ 'id': 1, 'description': '15%' }];
  typeOfProperty: any[] = [{ 'id': 1, 'description': 'Land' }, { 'id': 2, 'description': 'Building' }];
  searchOptions: any[] = [{ 'id': 1, 'description': 'Premises' }];
  paymentMethods: any[] = [{ 'paymentMethodID': 1, 'paymentMethod': 'Lumpsum' }, { 'paymentMethodID': 2, 'paymentMethod': 'Installment' }];
  isRadioButtonTouched: boolean = true;

  customerRowData: any[] = [];
  customerColumnDef: any[] = [];

  propertyRowData: any[] = [];
  propertyColumnDef: any[] = [];

  searchByCustomer: string;
  searchByPremises: string;
  searchByPan: string;
  searchByUnitNo: string;

  showListGrid: boolean;
  showPercGrid: boolean;
  SelectRow: any;

  properties: any[] = [];
  constructor(private _formBuilder: UntypedFormBuilder, private propertyService: PropertyService, private statesService: StatesService, private toastr: ToastrService,
    private taxService: TaxService, private prospectServ: ProspectService, private confirmationDialogSrv: ConfirmationDialogService) {
  }

  ngOnInit(): void {
    // Reactive Form
    this.propertyForm = this._formBuilder.group({      
      customerShare: [''],     
      propertyId: [''],
      dateOfAgreement: [''],      
      statusTypeId: [''],
      premises: [''],
      paymentMethodId: [2, Validators.required],
      gstRateID: ['', Validators.required],
      tdsRateID: ['', Validators.required],
      totalUnitCost: [''],
      tdsCollected: ['yes', Validators.required],
      tdsCollectedBySeller: ['']
    });

    this.columnDef = [
      { 'header': '', 'field': '', 'type': 'button', 'activity': ['edit'], 'width': 50 },
      { 'header': 'Name', 'field': 'nameList', 'type': 'labellist' },
      { 'header': 'Premises', 'field': 'premises', 'type': 'label' },
      { 'header': 'Unit No', 'field': 'unitNo', 'type': 'label' },
      { 'header': 'Declaration Date', 'field': 'declarationDate', 'type': 'label' },
      { 'header': '', 'field': '', 'type': 'button', 'activity': ['delete'], 'width': 50 }
    ];

    this.customerColumnDef = [
      { 'header': 'Name', 'field': 'name', 'type': 'label' },
      { 'header': 'PAN', 'field': 'pan', 'type': 'label' },
      { 'header': 'Share', 'field': 'share', 'type': 'label' },
      { 'header': 'Phone No', 'field': 'mobileNo', 'type': 'label' },
      { 'header': 'Email', 'field': 'emailID', 'type': 'label' }
      
    ];

    this.propertyColumnDef = [
      { 'header': 'Premises', 'field': 'premises', 'type': 'label' },
      { 'header': 'Unit No', 'field': 'unitNo', 'type': 'label' },
      { 'header': 'Declaration Date', 'field': 'declarationDate', 'type': 'label' }
    ];

    //this.getAllStates();
    this.getTaxCodes();
    this.getProspectList();
    this.getProperty();
  }

  clear() {
    this.propertyForm.reset();
    this.propertyForm.get('propertyType').setValue(2);
  }

  save(): void {
    if (this.propertyForm.valid) {
      let model = this.propertyForm.value;
      model.prospectPropertyID = this.SelectRow.prospectPropertyID;
      if (model.tdsCollected == "yes")
        model.tdsCollectedBySeller = true;
      else
        model.tdsCollectedBySeller = false;
      this.prospectServ.MoveProspectToCustomer(model).subscribe(res => {
        this.toastr.success("Process is completed.");
        this.getProspectList();
        var ele = document.getElementsByClassName('mat-tab-label') as HTMLCollectionOf<HTMLElement>;
        ele[0].click();
      });
    }
    else {
      this.toastr.error("Please fill the required fields")
    }
  }

  GetTaxCodes() {
    this.taxService.getTaxCodes().subscribe((response) => {
      let taxCodes = _.groupBy(response, function (item) {
        return item.taxTypeId == 1;
      });
      this.gstCode = taxCodes.true;
      this.tdsCode = taxCodes.false;
    });
  }
  getProspectList() {
    function getNameList(name: string) {
      let names = name.split(',');
      let nameList = [];
      _.forEach(names, obj => {
        nameList.push({ 'name': obj });
      });
      return nameList;
    }

    this.prospectServ.getProspectList(this.searchByPremises,this.searchByCustomer,this.searchByPan,this.searchByUnitNo).subscribe(res => {
      _.forEach(res, o => {
        o.nameList = getNameList(o.name);
        o.declarationDate = _moment(o.declarationDate).local().format('DD-MMM-YYYY');   
      });
      this.rowData = res;
    });

  }
 
  /**
     * On destroy
     */
  ngOnDestroy(): void {
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

  tabChanged(eve: MatTabChangeEvent) {
    //console.log('tabChangeEvent => ', eve);
    //console.log('index => ', eve.index);
    if (eve.index == 0) {
      this.search();     
    }
    else {     
    }
  }

  search() {
    this.getProspectList();
  }

  reset() {
    this.searchByCustomer = "";
    this.searchByPremises = "";
    this.searchByPan = "";
    this.searchByUnitNo = "";
    this.search();
  }

  editRow(eve) {
    this.SelectRow = eve;
    this.getSelectedProspect(eve.prospectPropertyID);
    var ele = document.getElementsByClassName('mat-tab-label') as HTMLCollectionOf<HTMLElement>;
    ele[1].click();
  }

  getSelectedProspect(id: number) {
    this.prospectServ.getProspectPropertyByID(id).subscribe(res => {
      this.customerRowData = res.prospectDto;
      var propDto = res.prospectPropertyDto;
      var prop = _.find(this.properties, o => o.propertyID == propDto.propertyID);
      let rows = { 'premises': prop.addressPremises, 'unitNo': propDto.unitNo, 'declarationDate': _moment(propDto.declarationDate).local().format('DD-MMM-YYYY')};
      this.propertyRowData = [rows];
      this.propertyForm.get("gstRateID").setValue(prop.gstTaxCode);
      this.propertyForm.get("tdsRateID").setValue(prop.tdsTaxCode);
    });
  }

  getProperty() {
    this.propertyService.getProperties().subscribe(res => {
      this.properties = res;
      this.properties.splice(0, 0, { 'propertyID': '', 'addressPremises': '' });
    });
  }
  deleteRow(eve) {
    this.deleteProspectAndProperty(eve.prospectPropertyID);
  }

  deleteProspectAndProperty(id: number) {
    this.confirmationDialogSrv.showDialog("Are you sure to delete?").subscribe(response => {
      if (response == "ok") {
        this.prospectServ.deleteProspectAndProperty(id).subscribe(res => {
          this.toastr.success("Presale Customer is deleted successfully");
          this.search();
        });
      }
    });
  }
}
