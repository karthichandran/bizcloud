/** Angular Imports */
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpEvent } from '@angular/common/http';
//import { PropertyDto } from '../ReProServices-api';


/** rxjs Imports */
import { Observable } from 'rxjs';

/**
 * Accounting service.
 */
@Injectable({
  providedIn: 'root'
})
export class StatusReportService {

  /**
   * @param {HttpClient} http Http Client to send requests.
   */
  constructor(private http: HttpClient) { }


  /**
   * @returns {Observable<any>} Loan products data.
   */
  getProperties(): Observable<any> {
    return this.http.get('/Property/dropdown');
  }


  getPropertiesByFilter(filter: string): Observable<any> {
    //let params = new HttpParams().set("paramName", filter).set("paramName2", paramValue2); 
    let params = new HttpParams().set("AddressPremises", filter);
    return this.http.get('/Property', { params: params });
  }

  getLotNo(): Observable<any> {
    return this.http.get('/clientpayment/lotNumbers');
  }

  getReportList( customer: string, propertyId: string, unitNo: string, lot: string): Observable<any> {
    let params = new HttpParams();
    if (customer != "" && customer != null)
      params = params.set("customerName", customer);
    if (propertyId != "" && propertyId != null)
      params = params.set("propertyID", propertyId);
    if (unitNo != "" && unitNo != null)
      params = params.set("unitNo", unitNo);
    if (lot != "" && lot != null)
      params = params.set("lotNo", lot);

    return this.http.get('/statusReport', { params: params });
  }

  downloadtoExcel(customer: string, propertyId: string, unitNo: string, lot: string): Observable<any> {
    let params = new HttpParams();
    if (customer != "" && customer != null)
      params = params.set("customerName", customer);
    if (propertyId != "" && propertyId != null)
      params = params.set("propertyID", propertyId);
    if (unitNo != "" && unitNo != null)
      params = params.set("unitNo", unitNo);
    if (lot != "" && lot != null)
      params = params.set("lotNo", lot);

    return this.http.get('/statusReport/getExcel', { params: params, responseType: 'blob' });
  }

  save(receiptModel: any, isNewEntry: boolean): Observable<any> {
    if (isNewEntry)
      return this.http.post('/receipt', { 'receiptDto': receiptModel });
    else {
      return this.http.put('/receipt', { 'receiptDto': receiptModel });
    }
  }
}
