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
export class UserRoleService {

  /**
   * @param {HttpClient} http Http Client to send requests.
   */
  constructor(private http: HttpClient) { }


  /**
   * @param {string} provisioningEntryId Provisioning entry ID of provisioning entry.
   * @returns {Observable<any>} Provisioning entry.
   */
  getUserRoleEntry(roleId: number): Observable<any> {
    return this.http.get(`/role/roleById/${roleId}`);
  }

  /**
   * @returns {Observable<any>} Loan products data.
   */
  getUserRoles(): Observable<any> {
    return this.http.get('/role');
  }

  save(user: any, isNew: boolean): Observable<any> {

    if (isNew)
      return this.http.post('/role', { 'RolesDto': user });
    else
      return this.http.put('/role', { 'RolesDto': user });
  }

}
