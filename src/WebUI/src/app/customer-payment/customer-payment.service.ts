/** Angular Imports */
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import * as moment from 'moment';
/** rxjs Imports */
import { Observable } from 'rxjs';

/**
 * Accounting service.
 */
@Injectable({
  providedIn: 'root'
})
export class ClientPaymentService {

  /**
   * @param {HttpClient} http Http Client to send requests.
   */
  constructor(private http: HttpClient) { }

  getNatureOfPayments(): Observable<any> {
    return this.http.get(`/NatureOfPayment`);
  }

  saveClientPayment(clientPayment: any, isNewEntry: boolean,installmendId:string): Observable<any> {
    if (isNewEntry)
      return this.http.post('/clientPayment', {'clientPaymentVM': clientPayment } );
    else {
      return this.http.put('/clientPayment/installmentID/' + installmendId, { 'clientPaymentVM': clientPayment, 'installmentID': installmendId});
    }
  }

  getClientPaymentByOwnershipId(id: string): Observable<any> {
    return this.http.get(`/clientPayment/${id}`);
  }

  deleteClientPayment(id: string): Observable<any> {
    return this.http.delete(`/clientPayment/installmentID/${id}`);
  }

  getCustomersByFilter(customerName: string, propertyId: string, premises: string, unitNo: string, lotNo: string, sellerId: string, remittanceStatusID: number, fromdate: string, todate: string, searchBynatureOfPaymentID: string): Observable<any> {
    let params = new HttpParams();
    if (customerName != "" && customerName != null)
      params = params.set("customerName", customerName);    
    if (propertyId != "" && propertyId != null)
      params = params.set("propertyID", propertyId);
    if (premises != "" && premises != null)
      params = params.set("premises", premises);
    if (unitNo != "" && unitNo != null)
      params = params.set("unitNo", unitNo);
    if (lotNo != "" && lotNo != null)
      params = params.set("LotNo", lotNo);
    if (remittanceStatusID != null)
      params = params.set("RemittanceStatusID", remittanceStatusID.toString());
    if (sellerId != "" && sellerId != null)
      params = params.set("SellerID", sellerId);
    if (fromdate != "" && fromdate != null) {
      let date = moment(fromdate).format("DD-MMM-YYYY");
      params = params.set("FromRevisedDate", date);
    }
    if (todate != "" && todate != null) {
      let date = moment(todate).format("DD-MMM-YYYY");
      params = params.set("ToRevisedDate", date);
    }
    if (searchBynatureOfPaymentID != "" && searchBynatureOfPaymentID != null) {
      params = params.set("NatureOfPaymentID", searchBynatureOfPaymentID);
    }

    return this.http.get('/clientPayment/paymentList', { params: params });
  }
  downloadExcelByFilter(customerName: string, propertyId: string, premises: string, unitNo: string, lotNo: string, sellerId: string, remittanceStatusID: number, fromdate: string, todate: string, searchBynatureOfPaymentID:string): Observable<any> {
    let params = new HttpParams();
    if (customerName != "" && customerName != null)
      params = params.set("customerName", customerName);
    if (propertyId != "" && propertyId != null)
      params = params.set("propertyID", propertyId);
    if (premises != "" && premises != null)
      params = params.set("premises", premises);
    if (unitNo != "" && unitNo != null)
      params = params.set("unitNo", unitNo);
    if (lotNo != "" && lotNo != null)
      params = params.set("LotNo", lotNo);
    if (  remittanceStatusID != null)
      params = params.set("RemittanceStatusID", remittanceStatusID.toString());
    if (sellerId != "" && sellerId != null)
      params = params.set("SellerID", sellerId);
    if (fromdate != "" && fromdate != null) {
      let date = moment(fromdate).format("DD-MMM-YYYY");
      params = params.set("FromRevisedDate", date);
    }
    if (todate != "" && todate != null) {
      let date = moment(todate).format("DD-MMM-YYYY");
      params = params.set("ToRevisedDate", date);
    }
    if (searchBynatureOfPaymentID != "" && searchBynatureOfPaymentID != null) {
      params = params.set("NatureOfPaymentID", searchBynatureOfPaymentID);
    }
    
    return this.http.get('/clientPayment/paymentList/getExcel', { params: params, responseType: 'blob' });
  }
  updateStatusAndremarks(model: any): Observable<any>{
    return this.http.put(`/customerProperty/status`, { 'custPropStatusObj': model });
  }

  getStatusList(): Observable<any>{
    return this.http.get(`/statusType`);
  }
  getSellerList(): Observable<any> {
    return this.http.get(`/seller/dropdown`);
  }
  getRemitanceStatus(): Observable<any> {
    return this.http.get('/RemittanceStatus');
  }
  uploadFile(formData: FormData, ): Observable<any> {
    return this.http.post('/clientPayment/uploadFile' , formData, { reportProgress: true, observe: 'events' });
  }
}
