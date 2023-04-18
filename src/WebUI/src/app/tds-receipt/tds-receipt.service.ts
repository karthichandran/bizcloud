/** Angular Imports */
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpEvent } from '@angular/common/http';
import { IPropertyVM} from '../property/property.model';
//import { PropertyDto } from '../ReProServices-api';


/** rxjs Imports */
import { Observable } from 'rxjs';

/**
 * Accounting service.
 */
@Injectable({
  providedIn: 'root'
})
export class TdsReceiptService {

  /**
   * @param {HttpClient} http Http Client to send requests.
   */
  constructor(private http: HttpClient) { }


  /**
   * @param {string} provisioningEntryId Provisioning entry ID of provisioning entry.
   * @returns {Observable<any>} Provisioning entry.
   */
  getPropertyEntry(propertyId: number): Observable<IPropertyVM> {
    return this.http.get(`/Property/${propertyId}`);
  }

  /**
   * @returns {Observable<any>} Loan products data.
   */
  getProperties(): Observable<any> {
    return this.http.get('/Property/dropdown');    
  }

  getModeOfReceipt(): Observable<any> {
    return this.http.get('/ModeOfReceipt');
  }

  getLotNo(): Observable<any> {
    return this.http.get('/clientpayment/lotNumbers');
  }
  getPropertiesByFilter(filter: string): Observable<any> {
    //let params = new HttpParams().set("paramName", filter).set("paramName2", paramValue2); 
    let params = new HttpParams().set("AddressPremises", filter); 
    return this.http.get('/Property', { params: params});
  }

  getReceipts(isTds: string, getPending: string, customer: string, propertyId: string, sellerId: string,unitNo: string, lot: string): Observable<any> {
    let params = new HttpParams();
    
    params = params.set("isTds", isTds);
    params = params.set("getPending", getPending);
    if (customer != "" && customer != null)
      params = params.set("customerName", customer);
    if (propertyId != "" && propertyId != null)
      params = params.set("propertyID", propertyId);
    if (unitNo != "" && unitNo != null)
      params = params.set("unitNo", unitNo);
    if (sellerId != "" && sellerId != null)
      params = params.set("sellerID", sellerId);
    if (lot != "" && lot != null)
      params = params.set("lotNo", lot);
    
    return this.http.get('/receipt', { params: params });
  }

  save(receiptModel: any, isNewEntry: boolean): Observable<any> {
    if (isNewEntry)
      return this.http.post('/receipt', { 'receiptList': receiptModel });
    else {
      return this.http.put('/receipt', { 'receiptList': receiptModel });
    }
  }

  sendMail(filter: string): Observable<any> {
    return this.http.get('/receipt/sendmail/' + filter);
  }
}
