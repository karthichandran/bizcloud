import { Component, OnDestroy, OnInit, ViewChild, ElementRef } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormGroupDirective,FormControl } from '@angular/forms';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { fuseAnimations } from '@fuse/animations';
import * as Xlsx from 'xlsx';
import { PropertyService } from '../../property/property.service';
import { ToastrService } from 'ngx-toastr';
import * as _ from 'lodash';
import { SellerComplianceReportService } from '../seller-compliance/seller-compliance-report.service';
import { SellerService} from '../../seller/seller.service';
import * as moment from 'moment';
import * as fileSaver from 'file-saver';
import { MatSelect } from '@angular/material/select';
import { ReplaySubject, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'seller-compliance-report',
  templateUrl: './seller-compliance-report.component.html',
  styleUrls: ['./seller-compliance-report.component.scss'],
  animations: fuseAnimations
})
export class SellerComplianceReportComponent implements OnInit, OnDestroy {
  reportform: UntypedFormGroup;

  reportRowData: any[] = [];
  reportColumnDef: any[] = [];
  premisesDDl: any[] = [];
  unitNoDDl: any[] = [];
  sellerDDl: any[] = [];
  lotNoDDl: any[] = [];
  activeProperty: any[] = [];
//Property Filter
public propertyFilterCtrl: FormControl = new FormControl();
@ViewChild('PropertyFilterSelect', { static: true }) PropertyFilterSelect: MatSelect;
/** Subject that emits when the component has been destroyed. */
protected _onDestroy = new Subject<void>();
public filteredProperty: ReplaySubject<any[]> = new ReplaySubject<any[]>();

  constructor(private _formBuilder: UntypedFormBuilder, private sellerReportSvc: SellerComplianceReportService,private sellerSvc:SellerService, private propertySvc: PropertyService, private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.reportform = this._formBuilder.group({
      lotNo: [''],
      premisesId: [''],
      unitNo: [''],
      customerName: [''],
      sellerId: ['']
    });

    this.reportColumnDef = [
      { 'header': 'Seller Name', 'field': 'sellerName', 'type': 'label','width':250 },
      { 'header': 'Premises', 'field': 'premises', 'type': 'label', 'width': 250 },
      { 'header': 'Client name', 'field': 'customerName', 'type': 'label', 'width': 250 },
      { 'header': 'Unit No', 'field': 'unitNo', 'type': 'label', 'width': 70  },
      { 'header': 'TDS Certificate Date', 'field': 'certificateDate', 'type': 'label', 'width': 180 },
      { 'header': 'TDS Certificate No', 'field': 'tdsCertificateNo', 'type': 'label', 'width': 160},
      { 'header': 'Amount', 'field': 'amount', 'type': 'label', 'width': 100 },
      { 'header': 'Form 16B File Name', 'field': 'form16BFileName', 'type': 'label', 'width': 250 }
    ];

    this.getProperties();
    this.getReportList();
    this.getSellers();
    this.getLotNo();
    this.propertyFilterCtrl.valueChanges.pipe(takeUntil(this._onDestroy))
    .subscribe(() => {
      this.filterProperty();
    });
  }

  getProperties() {
    this.propertySvc.getProperties().subscribe(response => {
      this.premisesDDl = response;
      this.premisesDDl.splice(0, 0, { 'propertyID': '', 'addressPremises': '' });
      this.activeProperty = _.filter(response, o => { return o.isActive == null || o.isActive == true; });
    });
  }

  getSellers() {
    this.sellerSvc.getSellers().subscribe(response => {
      this.sellerDDl = response;
      this.sellerDDl.splice(0, 0, { 'sellerID': '', 'sellerName': '' });
    });
  }

  getLotNo() {
    this.sellerReportSvc.getLotNo().subscribe(response => {
      this.lotNoDDl = response;
      this.lotNoDDl.splice(0, 0, { 'lotNo': '' });
    });
  }

  getReportList() {
    var filters = this.reportform.value;

    this.sellerReportSvc.getReportList(filters.customerName, filters.premisesId,filters.sellerId, filters.unitNo, filters.lotNo).subscribe(response => {
      _.forEach(response, obj => {
        obj.certificateDate = obj.tdsCertificateDate == null ? "" : moment(obj.tdsCertificateDate).local().format("DD-MMM-YYYY");
        obj.receiptDate = obj.paymentReceiptDate == null ? "" : moment(obj.paymentReceiptDate).local().format("DD-MMM-YYYY");
        obj.tdsPaid = obj.remittanceOfTdsAmount == null ? "" : moment(obj.remittanceOfTdsAmount).local().format("DD-MMM-YYYY");
        obj.formRequested = obj.form16BRequested == null ? "" : moment(obj.form16BRequested).local().format("DD-MMM-YYYY");
        obj.formDownloaded = obj.form16BDownloaded == null ? "" : moment(obj.form16BDownloaded).local().format("DD-MMM-YYYY");
        obj.emailDate = obj.mailDate == null ? "" : moment(obj.mailDate).local().format("DD-MMM-YYYY");
      });
      this.reportRowData = response;
    });
  }

  /**
     * On destroy
     */
  ngOnDestroy(): void {
  }

  downloadExcel() {
    var filters = this.reportform.value;

    this.sellerReportSvc.downloadtoExcel(filters.customerName, filters.premisesId, filters.sellerId, filters.unitNo, filters.lotNo).subscribe(response => {
      let blob: any = new Blob([response], { type: 'application/vnd.ms-excel' });
      fileSaver.saveAs(blob, 'SellerComplianceReport.xls');
    });
  }

  search() {
    this.getReportList();
  }

  reset() {
    this.reportform.reset();
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
}
