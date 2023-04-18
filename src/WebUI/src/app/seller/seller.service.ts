/** Angular Imports */
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { SellerDto } from '../ReProServices-api';

/** rxjs Imports */
import { Observable } from 'rxjs';

/**
 * Accounting service.
 */
@Injectable({
  providedIn: 'root'
})
export class SellerService {

  /**
   * @param {HttpClient} http Http Client to send requests.
   */
  constructor(private http: HttpClient) { }

  
  /**
   * @param {string} provisioningEntryId Provisioning entry ID of provisioning entry.
   * @returns {Observable<any>} Provisioning entry.
   */
  getSellerEntry(sellerId: string): Observable<SellerDto> {
    return this.http.get<SellerDto>(`/Seller/${sellerId}`);
  }
 
  /**
   * @returns {Observable<any>} Loan products data.
   */
  getSellers(): Observable<any> {
    return this.http.get('/Seller/dropdown');
  }
  getSellersByFilter(sellerName: string, pan: string, mobile: string): Observable<any> {
    let params = new HttpParams();
    if (sellerName != "" && sellerName!=null)
      params =params.set("SellerName", sellerName)
    if (pan != "" && pan != null)
      params =params.set("PAN", pan)
    if (mobile != "" && mobile != null)
      params =params.set("MobileNo", mobile);
    
    return this.http.get('/Seller', { params: params });
  }

  saveSeller(seller: SellerDto): Observable<any> {

    if(seller.sellerID>0)
      return this.http.put('/Seller/' + seller.sellerID, { 'sellerDto':seller });
    else
      return this.http.post('/Seller', { 'sellerDtoObj': seller } );
  }

  deleteSeller(sellerId:number) :Observable<any> {
    return this.http.delete('/Seller/' + sellerId);    
  }

}
