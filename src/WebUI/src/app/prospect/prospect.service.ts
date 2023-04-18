/** Angular Imports */
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpEvent } from '@angular/common/http';
import { IPropertyVM } from '../property/property.model';
//import { PropertyDto } from '../ReProServices-api';


/** rxjs Imports */
import { Observable } from 'rxjs';
//import { isDefined } from '@angular/compiler/src/util';

/**
 * Accounting service.
 */
@Injectable({
  providedIn: 'root'
})
export class ProspectService {

  /**
   * @param {HttpClient} http Http Client to send requests.
   */
  constructor(private http: HttpClient) { }


  /**
   * @param {string} provisioningEntryId Provisioning entry ID of provisioning entry.
   * @returns {Observable<any>} Provisioning entry.
   */
  isValid(param:any) {
    return (param != "" && param != null && !(param===undefined));
  }
  getProspectList(premises: any, customer: any, pan: any, unitno: any): Observable<any> {
    let params = new HttpParams();
    if (this.isValid(premises))
      params = params.set("propertyID", premises);
    if (this.isValid(customer))
      params = params.set("customer", customer);
    if (this.isValid(pan))
      params = params.set("pan", pan);
    if (this.isValid(unitno))
      params = params.set("unitNo", unitno);

    return this.http.get(`/prospect/list`, { params: params });
  }

  /**
   * @returns {Observable<any>} Loan products data.
   */
  getProperties(): Observable<any> {
    return this.http.get('/Property/dropdown');
  }

  getPropertiesByFilter(filter: string): Observable<any> {
    //let params = new HttpParams().set("para    let params = new HttpParams();
    let params = new HttpParams();
    if (filter != "" && filter != null)
      params = params.set("AddressPremises", filter);
    return this.http.get('/Property', { params: params });
  }

  MoveProspectToCustomer(prospect: any): Observable<any> {         
    return this.http.post('/prospect/process', { 'prospectProcessDto': prospect });  
  }

  getProspectPropertyByID(id:number): Observable<any> {
    return this.http.get('/prospect/prospectAndProperty/'+id);
  }

  deleteProspectAndProperty(id: number): Observable<any> {
    return this.http.delete('/prospect/' + id);
  }
}
