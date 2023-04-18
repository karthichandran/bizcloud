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
export class BankAccountService {

  /**
   * @param {HttpClient} http Http Client to send requests.
   */
  constructor(private http: HttpClient) { }


  /**
   * @returns {Observable<any>} Loan products data.
   */
  getAccountList(userName: string,): Observable<any> {
    let params = new HttpParams();
    if (userName != "" && userName != null)
      params = params.set("userName", userName);
    return this.http.get('/BankAccountDetails', { params: params });
  }
  //getSellersByFilter(sellerName: string, pan: string, mobile: string): Observable<any> {
  //  let params = new HttpParams();
  //  if (sellerName != "" && sellerName != null)
  //    params = params.set("SellerName", sellerName)
  //  if (pan != "" && pan != null)
  //    params = params.set("PAN", pan)
  //  if (mobile != "" && mobile != null)
  //    params = params.set("MobileNo", mobile);

  //  return this.http.get('/Seller', { params: params });
  //}

  saveAccount(acct: any): Observable<any> {

    if (acct.accountId > 0)
      return this.http.put('/BankAccountDetails', { 'bankDetails': acct });
    else
      return this.http.post('/BankAccountDetails', { 'bankDetails': acct });
  }

  deleteAccount(acctId: number): Observable<any> {
    return this.http.delete('/BankAccountDetails/' + acctId);
  }

}
