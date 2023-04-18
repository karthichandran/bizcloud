import { Component, OnDestroy, OnInit, ViewChildren, QueryList, ElementRef, ViewChild } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormGroupDirective, ValidatorFn, AbstractControl, UntypedFormControl,FormControl } from '@angular/forms';
import { HttpEventType } from '@angular/common/http';
import { fuseAnimations } from '@fuse/animations';
import * as Xlsx from 'xlsx';
import { FusePerfectScrollbarDirective } from '@fuse/directives/fuse-perfect-scrollbar/fuse-perfect-scrollbar.directive';
import { ICustomerVM } from './CustomerDto';
import { PropertyService } from '../property/property.service';
import { ClientService } from './client.service';
import { StatesService } from '../shared/services/states.service';
import * as _ from 'lodash';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { MatStepper } from '@angular/material/stepper';
//import { isUndefined } from 'util';
import { ToastrService } from 'ngx-toastr';
import * as moment from 'moment';
import {TaxService } from '../tax/tax.service';
import { MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import * as _moment from 'moment';
import * as fileSaver from 'file-saver';
import { DomSanitizer } from "@angular/platform-browser";
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSelect } from '@angular/material/select';
import {CustomerBillingDialogComponent } from '../customer-billing/customer-billing-dialog/customer-billing-dialog.component';
import { ConfirmationDialogService } from '../core/confirmation-dialog/confirmation-dialog.service';
import { Subject } from 'rxjs';
import { ReplaySubject } from 'rxjs/internal/ReplaySubject';
import { takeUntil } from 'rxjs/operators';
@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.scss'],
  animations: fuseAnimations
})
export class ClientComponent implements OnInit, OnDestroy {
  customerform: UntypedFormGroup;
  propertyForm: UntypedFormGroup;
  //shareForm: FormGroup;

  clients: any[] = [];
  sellers: any[] = [];
  states: any[] = [];
  form16Options: any[] = [{ 'id': 1, 'description': 'Yes' }, { 'id': 0, 'description': 'No' }];
  paymentMethods: any[] = [{ 'paymentMethodID': 1, 'paymentMethod': 'Lumpsum' }, { 'paymentMethodID': 2, 'paymentMethod': 'Installment' }];
  statusDDl: any[] = [{ 'id': '', 'description': '' }, { 'id': 1, 'description': 'Saved' }, { 'id': 2, 'description': 'Submitted' }, { 'id': 3, 'description': 'Cancelled' }, { 'id': 4, 'description': 'Assigned' }, { 'id': 5, 'description': 'Blocked' }, { 'id': 6, 'description': 'Released' }, { 'id': 7, 'description': 'Archive' }, { 'id': 8, 'description': 'No IT Password' }];
  gstCode: any[] = [{ 'id': 1, 'description': '15%' }];
  tdsCode: any[] = [{ 'id': 1, 'description': '15%' }];
  rowData: any[] = [];
  customerData: any = [];
  customerColumnDef: any[] = [];
  customerListColumnDef: any[] = [];
  declaration = new UntypedFormControl();
  customerAlias = new UntypedFormControl();
  isRadioButtonTouched: boolean = true;
  showAddressClearBtn: boolean = false;
  propertyList: any[] = [];
  declarationFileName: string;
  statementFileName: string;
  costFileName: string;
  otherFileName: string;
  progress: number;
  message: string;
  fileModel: any[] = [];
  filesNameList: any[] = [];
  loadingIndicator: boolean = false;
  reorderable: boolean = true;
  currentClientId: number;
  panDoc: any = {};
  propertyDDl: any[] = [];
  activeProperty: any[] = [];
  isd2way: any = "+91";
  countDetails: any = {};

  inActiveProperty: string;
  showActiveProperty: boolean=true;

  // Header Label
  activeCustomer: string;
  activePremises: string;
  activeUnit: string;
  //search
  searchByCustomerName: string;
  searchByPan: string;
  searchByPropertyID: string;
  searchByPremises: string;
  searchByMobile: string;
  searchByRemark: string;
  searchByStatusType: number;
  searchByUnit: string;

  customerView: any[] = [];

  showListGird: boolean;

  @ViewChildren(FusePerfectScrollbarDirective)
  fuseScrollbarDirectives: QueryList<FusePerfectScrollbarDirective>;

    //Property Filter
    public propertyFilterCtrl: FormControl = new FormControl();
    @ViewChild('PropertyFilterSelect', { static: true }) PropertyFilterSelect: MatSelect;
    /** Subject that emits when the component has been destroyed. */
    protected _onDestroy = new Subject<void>();
    public filteredProperty: ReplaySubject<any[]> = new ReplaySubject<any[]>();

    //Property Filter for search
    public propertyFilterCtrlForSearch: FormControl = new FormControl();
    @ViewChild('PropertyFilterSelectForSearch', { static: true }) PropertyFilterSelectForSearch: MatSelect;
    /** Subject that emits when the component has been destroyed. */
    protected _onDestroyOnSearch = new Subject<void>();
    public filteredPropertyForSearch: ReplaySubject<any[]> = new ReplaySubject<any[]>();


  constructor(private _formBuilder: UntypedFormBuilder, private propertyService: PropertyService,
    private statesService: StatesService, private clientService: ClientService, private toastr: ToastrService,
    private taxService: TaxService, private sanitizer: DomSanitizer, private dialog: MatDialog, private confirmationDialogSrv: ConfirmationDialogService) {
  }

  ngOnInit(): void {
    // Reactive Form
    this.customerform = this._formBuilder.group({
      customerID: [''],
      name: ['', Validators.required],
      addressPremises: [''],
      adressLine1: [''],
      addressLine2: [''],
      city: [''],
      stateId: [''],
      //pinCode: ['',Validators.compose([this.pinCodeValidator(),  Validators.maxLength(10)])],
      pinCode: [''],
      pan: ['',Validators.compose( [Validators.required, this.panValidator(),Validators.maxLength(10)])],
      emailID: ['', Validators.email],
      mobileNo: ['', Validators.compose([Validators.required,,Validators.maxLength(15)])],
      dateOfBirth: ['', Validators.required],
      isTracesRegistered: [''],
      traces: ['no'],
      tracesPassword: [''],
      share: [''],
      form16b: ['yes'],    
      alternateNumber: [''],
      isd: ['+91'],
      isPanVerified: [''],
      onlyTDS: [''],
      incomeTaxPassword:['']
    });
    // Vertical Stepper form stepper
    this.propertyForm = this._formBuilder.group({
      customerPropertyId: [''],
      customerShare: [''],
      customerId: [''],
      propertyId: [''],
      dateOfSubmission: [''],
      remarks: [''],
      isShared: [''],
      statusTypeId: [''],
      premises: [''],
      addressLine1: [{ value: '', disabled: true }],
      addressLine2: [{ value: '', disabled: true }],
      city: [{ value: '', disabled: true }],
      stateID: [{ value: '', disabled: true }],
      pinCode: [{ value: '', disabled: true }],
      unitNo: ['', Validators.required],
      paymentMethodId: [2, Validators.required],
      gstRateID: ['', Validators.required],
      tdsRateID: ['', Validators.required],
      totalUnitCost: [''],
      dateOfAgreement: [''],
      tdsCollected: ['yes', Validators.required],
      tdsCollectedBySeller: [''],
      sellers:[''],
      stampDuty: ['']
    });

    this.customerColumnDef = [{ 'header': 'Name', 'field': 'name', 'type': 'label' },
      { 'header': 'Share %', 'field': 'share', 'type': 'textbox' },
      { 'header': 'Primary Contact', 'field': 'isPrimaryOwner', 'type': 'checkbox', 'checkall': false, 'isMultySelect': false }
    ];
     
    this.getAllProperties();
    this.getAllStates();
    this.GetTaxCodes();  
    
    //initiate property filter
    //this.filteredProperty.next(this.propertyList.slice());
    this.propertyFilterCtrl.valueChanges.pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterProperty();
      });

      this.propertyFilterCtrlForSearch.valueChanges.pipe(takeUntil(this._onDestroyOnSearch))
      .subscribe(() => {
        this.filterPropertyForSearch();
      });
  }

  clear() {
    this.customerform.reset();
    this.propertyForm.reset();
    this.clients = [];
    this.filesNameList = [];
    this.customerData = [];
    this.customerform.get('traces').setValue("no");
    this.customerform.get('form16b').setValue("yes");
    this.customerform.get('isd').setValue("+91");
    this.propertyForm.get('tdsCollected').setValue("yes");
    this.panDoc = {};
    this.clearDocuments();
    this.clearHeadercustomerInfo();
    this.showAddressClearBtn = false;
    this.customerAlias.reset();
    this.showActiveProperty = true;
  }

  clearHeadercustomerInfo() {
    this.activeCustomer = "";
    this.activePremises = "";
    this.activeUnit = "";
  }

  /**
     * On destroy
     */
  ngOnDestroy(): void {
  }

  //selectedRows(eve) {
  //   this.customerform.patchValue(eve.selected[0]);
  //}

  GetTaxCodes() {
    this.taxService.getTaxCodes().subscribe((response) => {
      let taxCodes = _.groupBy(response, function (item) {
        return item.taxTypeId == 1;
      });
      this.gstCode = taxCodes.true;
      this.tdsCode = taxCodes.false;
    });
  }

  download() {
    const ws: Xlsx.WorkSheet = Xlsx.utils.json_to_sheet(this.rowData);
    const wb: Xlsx.WorkBook = Xlsx.utils.book_new();
    Xlsx.utils.book_append_sheet(wb, ws, "Sheet1");
    Xlsx.writeFile(wb, '.xls');
  }

  showClient(eve, model: any) {
    this.currentClientId = model.customerID;    

    if (model.isTracesRegistered)
      model.traces = "yes";
    else
      model.traces = "no";

    if (model.allowForm16B)
      model.form16b = "yes";
    else
      model.form16b = "no";
   
    model.pinCode = isNaN(model.pinCode) ? model.pinCode.trim() : model.pinCode;
    this.customerform.patchValue(model);
    this.customerform.get('alternateNumber').clearValidators();
    this.loadPanDocument(model.pan);
    this.removeRestriction();

    //Show active labels
    if (model.customerProperty!=null && model.customerProperty.length > 0) {
      this.activeCustomer = model.name;
      this.activePremises = _.find(this.propertyList, o => { return o.propertyID == model.customerProperty[0].propertyId }).addressPremises;
      this.activeUnit = model.customerProperty[0].unitNo;
    }
  } 

  addCoClient() {
    var isPanValid = this.customerform.get('isPanVerified').value;
    if (isPanValid == "" || isPanValid == null || isPanValid == false) {
      this.toastr.error("Please select Pan verified.");
      return;
    }

    this.removeRestriction();
    this.customerform.get('alternateNumber').clearValidators();
    var invalidList = _.filter(this.customerform.controls, function (item) {
      return item.validator != null && item.value == "";
    })

    if (this.customerform.valid && invalidList.length == 0) {
      this.showAddressClearBtn = true;

      let unSavedCustomer = _.filter(this.clients, function (item) { return item.customerID == 0 || item.customerID == null || item.customerID == ""; });
      let cusID = this.customerform.value.customerID;
      if (unSavedCustomer.length > 0 || cusID == 0 || cusID == null || cusID == "") {
        this.toastr.error("Please save the customer details before adding new customer");
        return;
      }
      
     // this.clients.push(_.clone(this.customerform.value));
          
      let client = this.customerform.value;
      client.customerID = 0;
      client.name = '';      
      client.mobileNo = '';
      client.emailID = '';
      client.pan = '';
      client.dateOfBirth = '';
      client.form16b = 'yes';
      client.tracesPassword = "";
      //client.customerAlias = "";
      client.alternateNumber = "";  
      client.isd = "+91";
      client.isPanVerified = false;
      client.incomeTaxPassword="";
      this.customerform.reset();
      this.customerform.patchValue(client);

      //To Reset control validators
      var formcontrl = this.customerform;
      _.forEach(['name', 'addressPremises', 'mobileNo', 'emailID', 'pan', 'dateOfBirth'], function (item) {
        let control = formcontrl.get(item);
        control.setErrors(null);
      });

      this.panDoc = {};
    }
    else {
      let client = this.customerform.value;
      this.customerform.reset();
      this.customerform.patchValue(client);
    }
    //Add restriction for aviod duplicates
   // this.addRestriction();
  }

  finishVerticalStepper(): void {
    alert('You have finished the vertical stepper!');
  }

  panValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      // if input field is empty return as valid else test
      const ret = (control.value !== '') ? new RegExp('^[A-Za-z]{5}[0-9]{4}[A-Za-z]$').test(control.value) : true;
      return !ret ? { 'invalidNumber': { value: control.value } } : null;
    };
  }

  pinCodeValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      // if input field is empty return as valid else test
      const ret = (control.value !== '') ? new RegExp('^[0-9]*$').test(control.value) : true;
      return !ret ? { 'invalidNumber': { value: control.value } } : null;
    };
  }

  addRestriction() {
    const validators = [Validators.required, Validators.minLength(3) ];
    this.customerform.addControl('completed', new UntypedFormControl('', validators));
    this.customerform.updateValueAndValidity();
  }
  removeRestriction() {
    this.customerform.removeControl('completed');
  //  this.customerform.updateValueAndValidity();
  }

  clearValidator() {
    this.customerform.get("alternateNumber").clearValidators();
    this.customerform.get("addressPremises").clearValidators();
    this.customerform.get("adressLine1").clearValidators();
    this.customerform.get("addressLine2").clearValidators();
    this.customerform.get("pinCode").clearValidators();
    this.customerform.get("isTracesRegistered").clearValidators();
    this.customerform.get("tracesPassword").clearValidators();
    this.customerform.get("traces").clearValidators();
    this.customerform.get("share").clearValidators();
    this.customerform.get("isd").clearValidators();
    this.customerform.get("customerID").clearValidators();
    this.customerform.get("isPanVerified").clearValidators();
    this.customerform.get("incomeTaxPassword").clearValidators();
  }
  saveCustomer(): void {
   // this.removeRestriction();
    this.clearValidator();
    if (this.customerform.valid) {
      let isNewEntry = true;
      var invalidList = _.filter(this.customerform.controls, function (item,key) {
        return item.validator != null && item.value == "";
      })
      if (invalidList.length == 0) {
        let currentCustomer = this.customerform.value;

        if (currentCustomer.traces == "yes") {
          if (currentCustomer.tracesPassword == "") {
            this.toastr.error("Please enter the Traces password");
            return;
          }
        }
       
        if (currentCustomer.isPanVerified == "" || currentCustomer.isPanVerified == undefined || currentCustomer.isPanVerified == false) {
          this.toastr.error("Please select Pan verified.");
          return;
        }

        let _custID = currentCustomer.customerID;
        if (_custID == "" || _custID == 0 || _custID == null) {
          let custIndex = _.findIndex(this.clients, function (item) { return item.pan == currentCustomer.pan; });
          if (custIndex == -1)
            this.clients.push(_.clone(this.customerform.value));
          else
            this.clients[custIndex] = this.customerform.value;
        }
        else {
          let custIndex = _.findIndex(this.clients, function (item) { return item.customerID == currentCustomer.customerID; });

          let oldModel = this.clients[custIndex];

          if (oldModel.customerProperty != null) {
            let oldCustomerProperty = _.clone(oldModel.customerProperty);
            this.clients[custIndex] = currentCustomer;
            this.clients[custIndex].customerProperty = oldCustomerProperty;
          } else
            this.clients[custIndex] = currentCustomer;
          //_.forEach(this.clients, function (item) {
          //  if (item.customerID == currentCustomer.customerID)
          //    item = [...item ,currentCustomer,];
          //});
        }

      }
      else {
        Object.keys(this.customerform.controls).forEach(field => {
          const control = this.customerform.get(field);
          control.markAsTouched({ onlySelf: true });
        });
        this.toastr.error("Please fill the all manditory fields");
        return;
      }

      // let startDate = new Date(this.customerform.value);
      let customerVM: ICustomerVM = {};
      let models = _.map(this.clients, function (item) {
        if (item.customerID == "" || item.customerID == 0 || item.customerID == null)
          item.customerID = 0;
        else
          isNewEntry = false;
        if (item.traces == "yes")
          item.isTracesRegistered = true;
        else
          item.isTracesRegistered = false;

        if (item.form16b == 'yes')
          item.allowform16b = true;
        else
          item.allowform16b = false;;

       // item.dateOfBirth = moment(item.dateOfBirth).local().format();
        item.dateOfBirth = moment(item.dateOfBirth).local().format("YYYY-MM-DD");

        //if (item.isPanValid == "yes")
        //  item.isPanVerified = true;
        //else
        //  item.isPanVerified = false;
        return item;
      });
      customerVM.customers = models;
      this.clientService.saveCustomer(customerVM, isNewEntry).subscribe((res) => {
        let newCusInx = _.findIndex(res.customers, function (item) {
          return item.customerProperty == null;
        });
        let existCusInx = _.findIndex(res.customers, function (item) {
          return (item.customerProperty != null && item.customerProperty.length > 0);
        });
        if (newCusInx > -1 && existCusInx > -1) {
          this.clients = res.customers;
          this.saveNewCustomer(res.customers, newCusInx);
        } else {
          this.toastr.success("Customer details saved successfully");
          this.clients = res.customers;
          this.showClient('', this.clients[0]);
        }
        //this.customerform.reset();        
        this.showAddressClearBtn = false;

      }, (err) => {
        // this.addRestriction();
      });
    }
    else {
      Object.keys(this.customerform.controls).forEach(field => {
        const control = this.customerform.get(field);
        control.markAsTouched({ onlySelf: true });
      });
    }
  }

  saveNewCustomer(customers: any, inx: number) {

    let cusToTake = (inx > 0) ? 0 : 1;

    let existCus = _.cloneDeep(customers[inx]);
    let existingProp =_.cloneDeep( customers[cusToTake].customerProperty);
    let newCusID = customers[inx].customerID;

    existCus.customerProperty = existingProp;
    existCus.customerProperty[0].customerPropertyId = 0;
    existCus.customerProperty[0].customerId = newCusID;
    existCus.customerProperty[0].customerShare = 0;
    existCus.customerProperty[0].isShared = customers.length > 1 ? true : false;

    let customerVM: ICustomerVM = {};
    customerVM.customers = [existCus];
    this.clientService.saveCustomerProperty(customerVM, 0).subscribe((res) => {
      this.clients[inx] = res.customers[0];
      this.toastr.success("Customer details saved successfully");
      this.showClient('', this.clients[0]);
    });

  }

  saveProperty(actionFrom: string): void {
    let toast = this.toastr;
    let ispanValid: boolean=true;
    _.forEach(this.clients, function (item) {
      if (item.isPanVerified == "" || item.isPanVerified == undefined || item.isPanVerified == false) {
        toast.error("Please select Pan verified.");
        ispanValid = false;
      }
    });
    if (!ispanValid)
      return;

    if (this.clients.length > 0) {
      let sharedCustomer = this.clients.length > 1 ? true : false;

      if (this.propertyForm.valid) {

        let propertyModel = this.propertyForm.value;
        let customerPropertyID = (propertyModel.customerPropertyId == "" || propertyModel.customerPropertyId == null) ? 0 : propertyModel.customerPropertyId;

        let declarationDate :any;
        if (this.declaration.value != null && this.declaration.value != "") {
          declarationDate = moment(this.declaration.value).local().format("YYYY-MM-DD");          
        }
        else {
         // declarationDate = moment().local().format("YYYY-MM-DD"); 
         // declarationDate = moment().local().format();
        }       

        let custAlias = this.customerAlias.value;
        if (actionFrom == "share") {
          if (custAlias == null || custAlias == "") {
            toast.error("Please fill the Owner alias.");
            return;
          }
        }

        if (customerPropertyID == 0) {
          _.forEach(this.clients, function (item) {
            let models = _.clone(propertyModel);
            models.customerPropertyId = 0;
            models.customerId = item.customerID;
            if (models.tdsCollected == "yes")
              models.tdsCollectedBySeller = true;
            else
              models.tdsCollectedBySeller = false;
            models.statusTypeId = 1;
            models.customerShare = 0;
            models.dateOfSubmission = declarationDate;
            models.isShared = sharedCustomer;
            models.customerAlias = custAlias;
            item.customerProperty = [];
            item.customerProperty[0] = models;
            models = {};
          });
        }
        else {
          var ownershipId = this.clients[0].customerProperty[0].ownershipID;
          var statusTypeId = this.clients[0].customerProperty[0].statusTypeId;
          _.forEach(this.clients, function (item) {
            if (item.customerProperty.length == 0) {
              let models = _.clone(propertyModel);
              models.customerPropertyId = 0;
              models.customerId = item.customerID;
              if (models.tdsCollected == "yes")
                models.tdsCollectedBySeller = true;
              else
                models.tdsCollectedBySeller = false;
              models.statusTypeId = statusTypeId;
              models.customerShare = 0;
              models.dateOfSubmission = declarationDate;
              models.isShared = sharedCustomer;
              models.customerAlias = custAlias;
              models.ownershipID = ownershipId;
              item.customerProperty = [];
              item.customerProperty[0] = models;
              models = {};
            }
          });

            propertyModel.dateOfSubmission = declarationDate;
         
          _.forEach(this.clients, function (item) {
           
            if (propertyModel.tdsCollected == "yes" )
              item.customerProperty[0].tdsCollectedBySeller = true;
            else
              item.customerProperty[0].tdsCollectedBySeller = false;

            item.customerProperty[0].propertyId = propertyModel.propertyId;
            item.customerProperty[0].unitNo = propertyModel.unitNo;
            item.customerProperty[0].paymentMethodId = propertyModel.paymentMethodId;
            item.customerProperty[0].gstRateID = propertyModel.gstRateID;
            item.customerProperty[0].tdsRateID = propertyModel.tdsRateID;
            item.customerProperty[0].totalUnitCost = propertyModel.totalUnitCost;
            item.customerProperty[0].dateOfAgreement = (propertyModel.dateOfAgreement == "" || propertyModel.dateOfAgreement==null )?'' :moment(propertyModel.dateOfAgreement).local().format("YYYY-MM-DD");
            item.customerProperty[0].dateOfSubmission = moment(propertyModel.dateOfSubmission).local().format("YYYY-MM-DD");
            item.customerProperty[0].customerAlias = custAlias;
            item.customerProperty[0].isShared = sharedCustomer;        
            item.customerProperty[0].stampDuty = propertyModel.stampDuty;
          });
        }

        this.UpdateCustomerShare();
        let customerVM: ICustomerVM = {};
        customerVM.customers = this.clients;

        this.clientService.saveCustomerProperty(customerVM, customerPropertyID).subscribe((res) => {
          this.toastr.success("Customer details saved successfully");
          if (actionFrom == "share") {
            var ele = document.getElementsByClassName('mat-step-label') as HTMLCollectionOf<HTMLElement>;
            ele[0].click();
            this.clear();
           
            return;
          }

          this.clients = res.customers;
          
          this.propertyForm.patchValue(res.customers[0].customerProperty[0]);
          if (res.customers[0].customerProperty[0].tdsCollectedBySeller)
            this.propertyForm.get('tdsCollected').setValue('yes');
          else
            this.propertyForm.get('tdsCollected').setValue('no');
          let date = res.customers[0].customerProperty[0].dateOfSubmission;
          if (moment(date).year() > 2000)
            this.declaration.setValue(date);

          this.showClient('', this.clients[0]);
          // Save send files to server
         // this.`();
        });

      } else {
        this.toastr.error("Please fill the required fields");
      }
    }
    else {
      this.toastr.error("No customers added");
    }
  }

  UpdateCustomerShare() {
    if (this.customerData.length > 0) {

      _.forEach(this.customerData, (cus) => {
        if (cus.share != "") {
          let cusInx = _.findIndex(this.clients, function (item) { return item.customerID == cus.customerID; });
          if (cusInx != null) {
            this.clients[cusInx].customerProperty[0].customerShare = parseFloat(cus.share);
            this.clients[cusInx].customerProperty[0].isPrimaryOwner = cus.isPrimaryOwner;
           // this.clients[cusInx].customerProperty[0].allowForm16B = cus.form16b == "0" ? false : true;
          }
        }

      });
    }
    else {

      if (this.clients.length == 1) {
        this.clients[0].customerProperty[0].customerShare = 100;
      } else {
        _.forEach(this.clients, function (item) {
          if (item.customerProperty[0].customerShare == "" || item.customerProperty[0].customerShare==null)
          item.customerProperty[0].customerShare = 0;
        });
      }
    }
  }

  submitCustomer(): void {
    if (this.validateSharePercentageAndDeclarationDate()) {
      this.saveProperty('share');
    }
  }

  validateSharePercentageAndDeclarationDate(): boolean {

    if (!this.declaration.valid) {
      this.declaration.markAsTouched();
      return false;;
    }

    let share: number = 0;
    let toastr = this.toastr;
    let isShareValid: boolean = true;
    _.forEach(this.customerData, function (item) {
      let val = parseFloat(item.share);
      if (isNaN(val) || val==0) {
        isShareValid= false;
      }
      share += val;
    });
    if (!isShareValid) {
      toastr.error("Please enter valid share value");
      return false;
    }
    if (share != 100 && share > 0) {
      this.toastr.error("Sum of share % must be equal to 100");
      return false;
    }

    let noOfPRimaryOwner = 0;
    _.forEach(this.customerData, function (item) {      
      if (item.isPrimaryOwner) {
        noOfPRimaryOwner++;
      }      
    });
    if (noOfPRimaryOwner != 1) {
      this.toastr.error("Please Select the primary owner");
      return false;
    }
    return true;
  }

  clearAddress(): void {
    this.showAddressClearBtn = false;
    let client = this.customerform.value;
    this.customerform.reset();
    client.addressPremises = '';
    client.adressLine1 = '';
    client.addressLine2 = '';
    client.city = '';
    client.stateId = '';
    client.pinCode = '';
    this.customerform.patchValue(client);
    Object.keys(this.customerform.controls).forEach(field => {
      const control = this.customerform.get(field);
      control.setErrors(null);
    });

  }

  getAllProperties() {
    this.propertyService.getProperties().subscribe((response) => {
      this.propertyList = response;
      this.activeProperty = _.filter(response, o => { return o.isActive == null || o.isActive == true; });
    });
  }

  getAllStates() {
    this.statesService.getStates().subscribe((response) => {
      this.states = response;
    });
  }

  panUploadClick(uploadBtn: Element) {
    if (this.panDoc.fileName===undefined)
      uploadBtn.dispatchEvent(new MouseEvent('click'));
    else
      this.toastr.warning("Please delete the current document then Upload");
  }

  uploadFile(event,fileType) {

    if (event.target.files && event.target.files.length > 0) {
      const file = event.target.files[0];
      let categoryId = 0;
      if (fileType == "declaration") { 
        this.declarationFileName = file.name;
        categoryId = 1;
    }
    if (fileType == "statement") {
      this.statementFileName = file.name;
      categoryId = 2;
    }
    if (fileType == "cost") {
      this.costFileName = file.name;
      categoryId = 3;
    }
      if (fileType == "other") {
        this.otherFileName = file.name;
        categoryId = 4;
      }
      this.fileModel = [{ 'file': file, 'name': file.name, 'categoryId': categoryId}];
      this.saveFiles(event);  

    }
  }
  uploadPan(event) {
    if (event.target.files && event.target.files.length > 0) {
      const files = event.target.files[0];
      let formData = new FormData();
      formData.append(files.name, files);     

      let pan = this.customerform.get('pan').value;
      this.clientService.uploadPan(formData, pan,5).subscribe((eve) => {
        if (eve.type == HttpEventType.Sent) {
          this.toastr.success("File Uploaded successfully");
        }       
      },
        (err) => { },
        () => {
          event.target.value = "";
          this.loadPanDocument(pan);
        }
      );
    }
  }

  saveFiles(eve) {
    if (this.fileModel.length == 0)
      return;

    let formData = new FormData();
    _.forEach(this.fileModel, function (item) {
      formData.append(item.name, item.file);
    });

    let guid = this.clients[0].customerProperty[0].ownershipID;
    this.clientService.uploadFile(formData, guid, this.fileModel[0].categoryId).subscribe((event) => {
     
      if (event.type == HttpEventType.Sent) {
        this.toastr.success("File Uploaded successfully");
        this.loadFilesList();
      }
      //if (event.type === HttpEventType.UploadProgress)
      //  this.progress = Math.round(100 * event.loaded / event.total);
      //else if (event.type === HttpEventType.Response) {
      //  this.message = 'Upload success.';
      //}
    },
      (err) => { },
      () => {
        eve.target.value = "";
        this.loadFilesList();
      }
      
    );
  }

  selectedProperty(id:any) {
    var prop = _.filter(this.propertyList, function (item) {
      return item.propertyID == id;
    });

    if (prop[0].isActive == null || prop[0].isActive == true) {
      this.showActiveProperty = true;
    } else {
      this.showActiveProperty = false;
      this.inActiveProperty = prop[0].addressPremises;
    }

      this.propertyForm.patchValue(prop[0]);
    if (this.clients[0].customerProperty==null || this.clients[0].customerProperty.length==0) {
      this.propertyForm.get('gstRateID').setValue(prop[0].gstTaxCode);
      this.propertyForm.get('tdsRateID').setValue(prop[0].tdsTaxCode);
    }

    this.clientService.getSellerName(id).subscribe(res => {
      this.propertyForm.get('sellers').setValue(res[0].sellerNames);
    });
  }

  selectedState(eve) {
    if (eve.value == 37) {
      this.customerform.get('pinCode').setValue("999999");
    }
  }

  tabChanged(eve: MatTabChangeEvent) {
    if (eve.index == 1) {
      this.clearHeadercustomerInfo();
     // this.search();
      this.propertyDDl = _.clone(this.propertyList);
      this.propertyDDl.splice(0, 0, { 'propertyID': '', 'addressPremises': '' });
      this.showListGird = true;
    }
    else
      this.showListGird = false;;
  }


  _loadCustomerList(response) {
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
      item.dateOfSubmission = moment(item.dateOfSubmission).year() > 2000 ? moment(item.dateOfSubmission).format('DD-MMM-YYYY') : ''
    });     

    this.rowData = response.customersView;
  }

  selectedRows(eve) {
    let model = eve.selected[0];
    if (model.isResident)
      model.residency = 'resident';
    else
      model.residency = 'nonResident';
    this.customerform.patchValue(model);
    var ele = document.getElementsByClassName('mat-tab-label') as HTMLCollectionOf<HTMLElement>;
    ele[0].click();
  }

  onSelect(eve) {
  }

  action(eve) {
    if (eve.action == 'edit') {
      this.clear();
      this.loadCustomerAndPropertyById(eve.id);
      var ele = document.getElementsByClassName('mat-tab-label') as HTMLCollectionOf<HTMLElement>;
      ele[0].click();
    }
    //  else if (eve.action == 'email') {
    //   this.clientService.groupMail(eve.id).subscribe((res) => {
    //     this.toastr.success("Sent mail successfully");
    //   });
    // }
    else {
      let row = eve.row;
      let model: any = {};    
     
      model.ownershipID = row.ownershipID;
      model.statusTypeID = row.statusTypeID;
      model.remarks = row.remarks
      this.clientService.updateStatusAndremarks(model).subscribe(response => {
        let customer = _.find(this.customerView, o => { return o.ownershipID == row.ownershipID });

        let isStatusChanged = !(row.statusTypeID == customer.statusTypeID);
        if (row.statusTypeID == 2 && isStatusChanged)
          this.openDialog(row.ownershipID);
        else {
          this.toastr.success("Customer details saved successfully");
          this.search();
        }
      });   
      
    }
  }

  loadCustomerAndPropertyById(id: string) {
    this.clearDocuments();
    if (id.length == 10)
      this.clientService.getCustomerByPan(id).subscribe((response) => { this.loadCustomer(response); });
    else
      this.clientService.getCustomerByID(id).subscribe((response) => { this.loadCustomerAndProperty(response); });
  }

  clearDocuments() {
    this.declarationFileName = "";
    this.statementFileName = "";
    this.costFileName = "";
    this.otherFileName = "";
   
  }
  loadCustomerByPan(id: string) {
    if (id.length != 10) {
      this.toastr.warning("Customer is not available on this pan number");
      return;
    }
    let existInx = _.findIndex(this.clients, o => {
      return o.pan == id;
    });

    if (existInx > -1) {
      return;
    }

    this.clientService.getCustomerByPan(id).subscribe((response) => {     
      if (response != null) {
        response['pinCode'] = response['pinCode'].trim();
        this.customerform.reset();
        this.clients.push(response);
        this.showClient('', response);
      }
    });
  }

  loadCustomer(response: any) {
    this.customerform.reset();
    this.clients= [response] ;
    this.propertyForm.reset();
    var ele = document.getElementsByClassName('mat-step-label') as HTMLCollectionOf<HTMLElement>;
    ele[0].click();
    this.showClient('', this.clients[0]);
    if (this.clients[0].customerProperty == null || this.clients[0].customerProperty.length == 0) {
      this.propertyForm.get('tdsCollected').setValue("yes");
    }
   // this.removeRestriction();
  }

  loadCustomerAndProperty(response: any) {
    this.customerform.reset();
    this.clients = response.customers;
    this.propertyForm.patchValue(response.customers[0].customerProperty[0]);
    this.selectedProperty(response.customers[0].customerProperty[0].propertyId);
    this.propertyForm.patchValue(response.customers[0].customerProperty[0]);
    if (response.customers[0].customerProperty[0].tdsCollectedBySeller)
      this.propertyForm.get('tdsCollected').setValue('yes');
    else
      this.propertyForm.get('tdsCollected').setValue('no');
    this.propertyForm.get('sellers').setValue(response.customers[0].sellers);

    this.customerAlias.setValue(response.customers[0].customerProperty[0].customerAlias);
    let date = response.customers[0].customerProperty[0].dateOfSubmission;
    if(moment(date).year()>2000)
    this.declaration.setValue(date);
    var ele = document.getElementsByClassName('mat-step-label') as HTMLCollectionOf<HTMLElement>;
    ele[0].click();
    this.showClient('', this.clients[0]);
    this.removeRestriction();
  }

  loadCustomerShare() {  
    let data = [];
    _.forEach(this.clients, function (item) {
      if (item.customerProperty.length > 0) {
        let model = { name: '', share: '', customerID: 0, isPrimaryOwner:false};
        model.name = item.name;
        model.customerID = item.customerID;
        model.share = item.customerProperty[0].customerShare;
        model.isPrimaryOwner = item.customerProperty[0].isPrimaryOwner;
        data = [model, ...data];
      }
    });

    let sortedData = _.sortBy(data, o => { return o.customerID; });
    this.customerData = sortedData;
  }

  selectedstepperIndex(eve) {
    if (eve.selectedIndex == 2) {
      this.loadCustomerShare();
      this.loadFilesList();
      this.filesNameList = [];
    }    
  }

  loadFilesList() {
    if (this.clients[0].customerProperty.length > 0) {
      this.clientService.getUploadedFiles(this.clients[0].customerProperty[0].ownershipID).subscribe((response) => {
        this.filesNameList = response;
      });
    }
  }

  loadPanDocument(id:string) {   
    this.clientService.getUploadedPan(id).subscribe((response) => {
      if (response != null)
        this.panDoc = response;
      else {
        this.panDoc = {};
      }
      });   
  }

  downloadFile(blobId:any,name:string,status:any) {

    this.clientService.downloadFiles(blobId).subscribe((response) => {

      let fileType = name.split('.')[1];
      let blobType = "";

      if (fileType == 'pdf') {
        blobType = 'application/pdf'
      }
      else if (fileType == 'jpg' || fileType == 'jpeg' || fileType == 'png' ) {
        blobType = 'image/' + fileType;
      }
      else if (fileType == 'xls')
      {
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


       // let blob: any = new Blob([response], { type: blobType });
      let blob: any = new Blob([response], { type: response.type });

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

  downloadExcel() {   
    this.clientService.downloadExcelByFilter(this.searchByCustomerName, this.searchByPan, this.searchByPropertyID,
      this.searchByPremises, this.searchByUnit, this.searchByRemark, this.searchByStatusType).subscribe((response) => {
        let blob: any = new Blob([response], { type: 'application/vnd.ms-excel' });
        fileSaver.saveAs(blob, 'Clients.xls');
      });
  }

  deleteFile(id: string,type:string) {
    this.clientService.deleteFile(id).subscribe(() => {
      this.toastr.success("FIle is deleted successfully");
      if (type == "pan")
        this.loadPanDocument(this.customerform.get('pan').value);
      else
        this.loadFilesList();
    });
  }

  search() {
    this.clientService.getCustomersByFilter(this.searchByCustomerName, this.searchByPan, this.searchByPropertyID,
      this.searchByPremises, this.searchByUnit, this.searchByRemark, this.searchByStatusType).subscribe((response) => {
        this._loadCustomerList(response);
      });
    this.clientService.getCustomerCount().subscribe(res => {
      this.countDetails = res;
    });
  }

  reset() {
    this.searchByCustomerName = "";
    this.searchByPan = "";
    this.searchByPremises = "";
    this.searchByUnit = "";
    this.searchByRemark = "";
    this.searchByStatusType = 0;
    this.searchByPropertyID = "";
    this.search();
  }

  NavigateToProperty(stepper: MatStepper) {
    if (this.clients.length == 0) {
      this.toastr.error("Please add/save new customer.");
      return;
    }

    var isValid = _.findIndex(this.clients, function (item) { return item.customerID == 0 || item.customerID == null || item.customerID == "" });

    if (isValid > -1) {
      this.toastr.warning("Please save customers");
      return;
    }
     
    if (this.customerform.get('customerID').value==0) {
      this.toastr.warning("Please save customers");
      return;
    }

    stepper.next();
  }

  NavigateToShare(stepper: MatStepper) {
    if (this.propertyForm.get('propertyId').value == 0 || this.propertyForm.get('propertyId').value == "" || this.propertyForm.get('propertyId').value == null) {
      this.toastr.error("Please fill and save property details.");
      return;
    }

    if (this.clients[0].customerProperty==null || this.clients[0].customerProperty.length==0) {
      this.toastr.error("Please save property details.");
      return;
    }
    
    stepper.next();
  }

  openDialog(id:string): void {
    const dialogRef = this.dialog.open(CustomerBillingDialogComponent, {
      hasBackdrop: false,
      maxHeight: 650,
      maxWidth: 1000,
      width:"800px",
      data: {
        'premises': this.propertyList, 'paymentMethods': this.paymentMethods, 'gstRate': this.gstCode, 'ownershipId': id,'isNew':true
      }
     
    });

    dialogRef.afterClosed().subscribe(() => {
      this.search();
      console.log('The dialog was closed');     
    });
  }
  deleteCustomer(id: number) {
    var curCus = _.find(this.clients, o => {return o.customerID==id });
    this.confirmationDialogSrv.showDialog("Are you sure to delete this customer?").subscribe(response => {
      if (response == "ok") {
        let ownershipid: string;
        if (curCus.customerProperty != null)
          ownershipid = curCus.customerProperty[0].ownershipID;
        this.clientService.deleteCustomer(id, ownershipid).subscribe(res => {
          this.toastr.success("Customer is deleted successfully");
          if (this.clients.length > 1) {
            if (this.clients[0].customerProperty != null) {
              let guid = this.clients[0].customerProperty[0].ownershipID;
              this.loadCustomerAndPropertyById(guid);
            }
            else {
              let inx = _.findIndex(this.clients, o => {
                return o.customerID == id
              });
              this.clients.splice(inx, 1);
              this.customerform.reset();
              this.customerform.get("form16b").setValue("yes");
              this.customerform.get("traces").setValue("no");
              this.customerform.get("isd").setValue("+91");
            }
          } else {
            this.clear();
          }
        });
      }
    });
  }

  UpdateTraces(isChecked) {
    if (isChecked) {
      this.customerform.get('traces').setValue("no");
    }

  }

  UpdateOnlyTDS() {
    this.customerform.get('onlyTDS').setValue(false);
  }

   //property Filter functionality for active property
   protected filterProperty() {
    if (!this.activeProperty) {
      return;
    }
    // get the search keyword
    let search = this.propertyFilterCtrl.value;
    if (!search) {
      this.filteredProperty.next(this.activeProperty.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    // filter the banks
    this.filteredProperty.next(this.filterProFun(search));
  }

  filterProFun(search) {
    var list = this.activeProperty.filter(prop => prop.addressPremises.toLowerCase().indexOf(search) > -1);
    return list;
  }
  // --- end property filter

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
