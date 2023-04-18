/** Angular Imports */
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

/** rxjs Imports */
import { Observable } from 'rxjs';

/**
 * Accounting service.
 */
@Injectable({
  providedIn: 'root'
})
export class LotCreationService {

  /**
   * @param {HttpClient} http Http Client to send requests.
   */
  constructor(private http: HttpClient) { }


  saveLot(lot: any, isNewEntry: boolean): Observable<any> {
    if (!isNewEntry)
      return this.http.put('/lot/' + lot.id, { 'lotDto': lot });
    else {
      return this.http.post('/lot', { 'lotDto': lot });
    }
  }

 getLotByid(id: string): Observable<any> {
    return this.http.get(`/lot/${id}`);
  }

  getCusBillsByFilter(customerName: string, propertyId: string, premises: string, unitNo: string, paymentType: number): Observable<any> {
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

    return this.http.get('/CustomerBilling/getList', { params: params });
  }


  deleteLot(id: number): Observable<any> {
    return this.http.delete(`/lot/${id}`);
  }

  tabChanged(eve) {

  }
}
