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
export class CustomerBillingService {

  /**
   * @param {HttpClient} http Http Client to send requests.
   */
  constructor(private http: HttpClient) { }


  saveCustomerBill(customerBill: any, isNewEntry: boolean): Observable<any> {
    if (!isNewEntry)
      return this.http.put('/CustomerBilling/' + customerBill.ownershipID, { 'customerBillingDto': customerBill });
    else {
      return this.http.post('/CustomerBilling', { 'customerBillingDto': customerBill });
    }
  }

  getCusBillBaseModelByOwnershipId(id:string): Observable<any> {
    return this.http.get(`/CustomerBilling/getBaseModel/${id}`);
  }

  getCusBillByOwnershipId(id: string): Observable<any> {
    return this.http.get(`/CustomerBilling/${id}`);
  }

  getCusBillByBillId(id: string): Observable<any> {
    return this.http.get(`/CustomerBilling/CustomerBillID/${id}`);
  }

  getCusBillsByFilter(customerName: string,propertyId:string, premises: string, unitNo: string, paymentType: number,fromDate:string,toDate:string): Observable<any> {
    let params = new HttpParams();
    if (customerName != "" && customerName != null)
      params = params.set("CustomerName", customerName);
    if (propertyId != "" && propertyId != null)
      params = params.set("PropertyID", propertyId);
    if (premises != "" && premises != null)
      params = params.set("Premises", premises);
    if (unitNo != "" && unitNo != null)
      params = params.set("unitNo", unitNo);
    if (paymentType != 0 && paymentType != null)
      params = params.set("paymentStatus", paymentType.toString());
    if (fromDate != "" && fromDate != null) {
      let date = moment(fromDate).format("DD-MMM-YYYY");
      params = params.set("fromDate", date);
    }
    if (toDate != "" && toDate != null) {
      let date = moment(toDate).format("DD-MMM-YYYY");
      params = params.set("toDate", date);
    }
    return this.http.get('/CustomerBilling/getList', { params: params });
  }

  downloadBillsByFilter(customerName: string, propertyId: string, premises: string, unitNo: string, paymentType: number, fromDate: string, toDate: string): Observable<any> {
    let params = new HttpParams();
    if (customerName != "" && customerName != null)
      params = params.set("CustomerName", customerName);
    if (propertyId != "" && propertyId != null)
      params = params.set("PropertyID", propertyId);
    if (premises != "" && premises != null)
      params = params.set("Premises", premises);
    if (unitNo != "" && unitNo != null)
      params = params.set("unitNo", unitNo);
    if (paymentType != 0 && paymentType != null)
      params = params.set("paymentStatus", paymentType.toString());
    if (fromDate != "" && fromDate != null) {
      let date = moment(fromDate).format("DD-MMM-YYYY");
      params = params.set("fromDate", date);
    }
    if (toDate != "" && toDate != null) {
      let date = moment(toDate).format("DD-MMM-YYYY");
      params = params.set("toDate", date);
    }
    return this.http.get('/CustomerBilling/getExcel', { params: params,responseType:'blob' });
  }

  deleteCusBill(id: number): Observable<any> {
    return this.http.delete(`/CustomerBilling/${id}`);
  }
  
}
