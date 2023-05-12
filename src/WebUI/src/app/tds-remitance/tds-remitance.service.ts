/** Angular Imports */
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpEvent } from '@angular/common/http';
import { IPropertyVM} from '../property/property.model';
//import { PropertyDto } from '../ReProServices-api';

/** rxjs Imports */
import { Observable } from 'rxjs';
import { isNullOrUndefined } from '@swimlane/ngx-datatable';
/**
 * Accounting service.
 */
@Injectable({
  providedIn: 'root'
})
export class TdsRemitanceService {

  /**
   * @param {HttpClient} http Http Client to send requests.
   */
  constructor(private http: HttpClient) { }


  /**
   * @returns {Observable<any>} Loan products data.
   */
  getProperties(): Observable<any> {
    return this.http.get('/Property/dropdown');    
  }

  getRemitanceStatus(): Observable<any> {
    return this.http.get('/RemittanceStatus');
  }

  getTdsRemitance(customer: string, premises: string, unitNo: string, lotNo: string,searchByStatus:string,searchByAmount:string): Observable<any>  {
    let params = new HttpParams();
    if (customer != "" && customer != null)
      params = params.set("customerName", customer);      
    if (premises != "" && premises != null)
      params = params.set("PropertyPremises", premises);
    if (unitNo != "" && unitNo != null)
      params = params.set("unitNo", unitNo);
    if (lotNo != "" && lotNo != null)
      params = params.set("lotNo", lotNo);   
    if (!isNullOrUndefined(searchByStatus ) )
      params = params.set("remittanceStatusID", searchByStatus);
    if (searchByAmount != "" && searchByAmount != null)
      params = params.set("tdsAmount", searchByAmount);
    return this.http.get('/TdsRemittance', { params: params });
  }

  //proceedForm26qb(customerPaymentId:string): Observable<any> {
  //  return this.http.get('/autoFill/' + customerPaymentId);
  //}

  getTdsRemitanceById(clientPayTransId: string): Observable<any> {
    return this.http.get('/TdsRemittance/getRemittance/' + clientPayTransId);
  }

  save(remitanceModel: any, isNewEntry: boolean): Observable<any> {
    if (isNewEntry)
      return this.http.post('/TdsRemittance', { 'RemittanceDto': remitanceModel });
    else {
      return this.http.put('/TdsRemittance' ,{ 'RemittanceDto': remitanceModel });
    }
  }

  saveDebitAdvice(model: any, isNewEntry: boolean): Observable<any> {
    if (isNewEntry)
      return this.http.post('/DebitAdvice', { 'debitAdviceDto': model });
    else {
      return this.http.put('/DebitAdvice' ,{ 'debitAdviceDto': model });
    }
  }
  uploadDebitAdviceFile(formData: FormData): Observable<any> {
    return this.http.post('/DebitAdvice/uploadFile' , formData, { reportProgress: true, observe: 'events' });
  }
  deleteDebitAdvice(Id: string): Observable<any> {
    return this.http.delete(`/DebitAdvice/${Id}`);
  }

  uploadFile(formData: FormData,remittanceID:string, category: number): Observable<any> {
    //return this.http.post('/files/guid/' + ownershipID + '/' + category, formData, { reportProgress: true, observe: 'events' });
    return this.http.post('/traces/' + remittanceID + '/' + category, formData, { reportProgress: true, observe: 'events' });
  }

  getUploadedFiles(Id: string): Observable<any> {
    //return this.http.get(`/files/fileslist/${Id}`);
    return this.http.get(`/files/fileinfo/${Id}`);
  }
  downloadFiles(Id: string): Observable<any> {
    return this.http.get(`/files/blobId/${Id}`, { responseType: 'blob' });
  }
  deleteFile(Id: string): Observable<any> {
    return this.http.delete(`/files/blobId/${Id}`);
  }

  deleteRemittance(Id: string): Observable<any> {
    return this.http.delete(`/traces/${Id}`);
  }
}
