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
export class UserPermissionsService {

  /**
   * @param {HttpClient} http Http Client to send requests.
   */
  constructor(private http: HttpClient) { }


  /**
   * @param {string} provisioningEntryId Provisioning entry ID of provisioning entry.
   * @returns {Observable<any>} Provisioning entry.
   */
  getUserPermissionsByRoleId(roleId: number): Observable<any> {
    return this.http.get(`/rolePermissions/roleBy/${roleId}`);
  }

  /**
   * @returns {Observable<any>} Loan products data.
   */
  getUserPermissions(): Observable<any> {
    return this.http.get('/rolePermissions');
  }

  save(rolePermission: any): Observable<any> {
  
      return this.http.put('/rolePermissions', { 'permissionsDto': rolePermission });
  }

}
