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
export class StatementOfAccountService {

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

  getReportList(customer: string, propertyId: string, unitNo: string): Observable<any> {
    let params = new HttpParams();
    if (customer != "" && customer != null)
      params = params.set("customerName", customer);
    if (propertyId != "" && propertyId != null)
      params = params.set("propertyId", propertyId);
    if (unitNo != "" && unitNo != null)
      params = params.set("unitNo", unitNo);
    
    return this.http.get('/statementOfAccount', { params: params });
    //return this.http.get('/statementOfAccount');
  }

  downloadtoExcel(customer: string, propertyId: string, unitNo: string, lot: string): Observable<any> {
    let params = new HttpParams();
    if (customer != "" && customer != null)
      params = params.set("customerName", customer);
    if (propertyId != "" && propertyId != null)
      params = params.set("propertyID", propertyId);
    if (unitNo != "" && unitNo != null)
      params = params.set("unitNo", unitNo);

    return this.http.get('/statementOfAccount/getExcel', { params: params, responseType: 'blob' });
  }  
}
