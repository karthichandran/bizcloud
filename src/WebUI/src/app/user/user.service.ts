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
export class UserService {

  /**
   * @param {HttpClient} http Http Client to send requests.
   */
  constructor(private http: HttpClient) { }


  /**
   * @param {string} provisioningEntryId Provisioning entry ID of provisioning entry.
   * @returns {Observable<any>} Provisioning entry.
   */
  getUserEntry(userId: string): Observable<any> {
    return this.http.get(`/user/${userId}`);
  }

  /**
   * @returns {Observable<any>} Loan products data.
   */
  getUsers(userName:string,code:string,isActive:any): Observable<any> {
    let params = new HttpParams();
    if (userName != "" && userName != null)
      params = params.set("userName", userName);
    if (code != "" && code != null)
      params = params.set("code", code);
    if ( isActive != null)
      params = params.set("isActive", isActive);
    return this.http.get('/user', { params: params});
  }

  getUserById(userId:number): Observable<any> {
    return this.http.get('/user/UserById/' + userId);
  }

  getUserProfile(username: string): Observable<any> {
    return this.http.get('/user/UserProfile/' + username);
  }

  save(user: any,isNew:boolean): Observable<any> {

    if (isNew)
      return this.http.post('/user', { 'userVM': user });
    else
      return this.http.put('/user', { 'userVM': user });
  }


}
