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
export class RemarkService {

  /**
   * @param {HttpClient} http Http Client to send requests.
   */
  constructor(private http: HttpClient) { }


  getRemarks(isRemittance): Observable<any> {
    let params = new HttpParams();
    if (isRemittance != "" && isRemittance != null)
      params = params.set("isRemittance", isRemittance);
    
    return this.http.get(`/remittanceremark`,{ params: params });
  }
  getRemarkById(id): Observable<any> {       
    return this.http.get('/remittanceremark/'+id);
  }

  save(model: any, isNew: number): Observable<any> {
    if(isNew==0)
      return this.http.post('/remittanceremark', { 'remittanceRemarkDto': model });
    else
      return this.http.put('/remittanceremark', {'remittanceRemarkDto': model });
  }

  deleteRemark(id: number) {
    return this.http.delete(`/remittanceremark/${id}`);
  }

}
