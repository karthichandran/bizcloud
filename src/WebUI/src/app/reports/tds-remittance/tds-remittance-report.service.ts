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
export class TdsRemittanceReportService {

  /**
   * @param {HttpClient} http Http Client to send requests.
   */
  constructor(private http: HttpClient) { }

  getReportList(customer: string, propertyId: string, sellerId: string, unitNo: string, lot: string): Observable<any> {
    let params = new HttpParams();
    if (customer != "" && customer != null)
      params = params.set("customerName", customer);
    if (propertyId != "" && propertyId != null)
      params = params.set("propertyID", propertyId);
    if (sellerId != "" && sellerId != null)
      params = params.set("sellerID", sellerId);
    if (unitNo != "" && unitNo != null)
      params = params.set("unitNo", unitNo);
    if (lot != "" && lot != null)
      params = params.set("lot", lot);

    return this.http.get('/tdsremittance/report', { params: params });
  }

  downloadtoExcel(customer: string, propertyId: string, sellerId: string, unitNo: string, lot: string): Observable<any> {
    let params = new HttpParams();
    if (customer != "" && customer != null)
      params = params.set("customerName", customer);
    if (propertyId != "" && propertyId != null)
      params = params.set("propertyID", propertyId);
    if (sellerId != "" && sellerId != null)
      params = params.set("sellerID", sellerId);
    if (unitNo != "" && unitNo != null)
      params = params.set("unitNo", unitNo);
    if (lot != "" && lot != null)
      params = params.set("lot", lot);

    return this.http.get('/tdsremittance/getExcel', { params: params, responseType: 'blob' });
  }
}
