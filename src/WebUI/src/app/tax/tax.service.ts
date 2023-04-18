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
export class TaxService {

  /**
   * @param {HttpClient} http Http Client to send requests.
   */
  constructor(private http: HttpClient) { }


  getTaxCodesById(id:string): Observable<any> {
    return this.http.get(`/taxcodes/${id}`);
  }
  getTaxCodesByTaxId(id: string): Observable<any> {
    return this.http.get(`/taxcodes/taxId/${id}`);
  }

  saveTax(model: any, isNew: number): Observable<any> {
    if(isNew==0)
      return this.http.post('/taxcodes', { 'taxCodeDtoObj': model });
    else
      return this.http.put('/taxcodes/' + isNew, {'taxCodeDtoObj': model });
  }

  copyTax(model: any): Observable<any> {
      return this.http.post('/taxcodes/copy', { 'taxCodeDtoObj': model });
  }

  getTaxCodes(): Observable<any>{
    return this.http.get('/taxcodes');
  }

  getTaxCodesByQuery(taxname: string, taxType: string): Observable<any> {
    let params = new HttpParams();
    if (taxname != "" && taxname != null)
      params = params.set("taxName", taxname);
    if (taxType != "" && taxType != null)
      params = params.set("taxType", taxType);
    return this.http.get('/taxcodes', { params: params });
  }

  deleteTaxCode(id: number) {
    return this.http.delete(`/taxcodes/${id}`);
  }

}
