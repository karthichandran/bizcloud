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
export class TaxPaymentReportService {

  /**
   * @param {HttpClient} http Http Client to send requests.
   */
  constructor(private http: HttpClient) { }

  getReportList( propertyId: string,  unitNo: string, lot: string): Observable<any> {
    let params = new HttpParams();   
    if (propertyId != "" && propertyId != null)
      params = params.set("propertyId", propertyId);    
    if (unitNo != "" && unitNo != null)
      params = params.set("unitNo", unitNo);
    if (lot != "" && lot != null)
      params = params.set("lotNo", lot);

    return this.http.get('/taxPaymentReport', { params: params });
  }

  downloadtoExcel( propertyId: string, unitNo: string, lot: string): Observable<any> {
    let params = new HttpParams();
    if (propertyId != "" && propertyId != null)
      params = params.set("propertyId", propertyId);
    if (unitNo != "" && unitNo != null)
      params = params.set("unitNo", unitNo);
    if (lot != "" && lot != null)
      params = params.set("lotNo", lot);

    return this.http.get('/taxPaymentReport/getExcel', { params: params, responseType: 'blob' });
  }

  getLotNo(): Observable<any> {
    return this.http.get('/clientpayment/lotNumbers');
  }
}
