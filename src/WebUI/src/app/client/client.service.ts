/** Angular Imports */
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ICustomer ,ICustomerVM} from './CustomerDto';
import { CustomerPropertyDto } from './CustomerPropertyDto';
/** rxjs Imports */
import { Observable } from 'rxjs';

/**
 * Accounting service.
 */
@Injectable({
  providedIn: 'root'
})
export class ClientService {

  /**
   * @param {HttpClient} http Http Client to send requests.
   */
  constructor(private http: HttpClient) { }


  /**
   * @param {string} provisioningEntryId Provisioning entry ID of provisioning entry.
   * @returns {Observable<any>} Provisioning entry.
   */

  getCustomer(): Observable<any> {
    return this.http.get(`/Customer`);
  }

  getCustomerByID(Id: string): Observable<any> {
    return this.http.get(`/customer/${Id}`);
  }
  getCustomerByPan(Id: string): Observable<any> {
    return this.http.get(`/customer/pan/${Id}`);
  }

  saveCustomer(customer: ICustomerVM, isNewEntry: boolean): Observable<any> {
    //return this.http.post('/Customer', { 'customerVM': { 'customers': [{ 'customerID': 0 }] } });
    if (!isNewEntry)
      return this.http.put('/Customer', { 'customerVM': customer });
    else {
      return this.http.post('/Customer', { 'customerVM': customer });
      //return this.http.post('/Customer', { 'customerVM': { 'customers': [{'customerID':0}]} });
    }
  }

  saveCustomerProperty(customer: ICustomerVM,customerPropertyID:number): Observable<any> {
    //return this.http.post('/Customer', { 'customerVM': { 'customers': [{ 'customerID': 0 }] } });
    if (customerPropertyID>0)
      return this.http.put('/CustomerProperty' , { 'customerVM': customer });
    else {
      return this.http.post('/CustomerProperty', { 'customerVM': customer });
    }
  }

   uploadFile(formData: FormData,id:string,categoryId:number): Observable<any> {    
     return this.http.post('/files/guid/' + id+'/'+categoryId, formData, { reportProgress: true, observe: 'events' });
  }


  uploadPan(formData: FormData, id: string, categoryId: number): Observable<any> {
    return this.http.post('/files/panId/' + id + '/' + categoryId, formData, { reportProgress: true, observe: 'events' });
  }

  getUploadedFiles(Id: string): Observable<any> {
    return this.http.get(`/files/fileslist/${Id}`);
  }

  getUploadedPan(Id: string): Observable<any> {
    return this.http.get(`/files/fileDetails/panId/${Id}`);
  }

  deleteFile(Id: string ): Observable<any> {
    return this.http.delete(`/files/blobId/${Id}`);    
  }

  downloadFiles(Id: string): Observable<any> {
    return this.http.get(`/files/blobId/${Id}`, { responseType: 'blob' });
  }

  getRemarks(): Observable<any> {
    return this.http.get(`/remarks`);
  }

  getCustomersByFilter(customerName: string, pan: string,propertyId:string, premises: string, unitNo: string, remark: string, statusType: number): Observable<any> {
    let params = new HttpParams();
    if (customerName != "" && customerName != null)
      params = params.set("customerName", customerName);
    if (pan != "" && pan != null)
      params = params.set("PAN", pan.toUpperCase());
    if (propertyId != "" && propertyId != null)
      params = params.set("propertyId", propertyId);
    if (premises != "" && premises != null)
      params = params.set("premises", premises);
    if (unitNo != "" && unitNo != null)
      params = params.set("unitNo", unitNo);
    if (remark != "" && remark != null)
      params = params.set("remarks", remark);
    if (statusType != 0 && statusType != null)
      params = params.set("statusTypeId", statusType.toString());

    return this.http.get('/customer', { params: params });
  }

  downloadExcelByFilter(customerName: string, pan: string, propertyId: string, premises: string, unitNo: string, remark: string, statusType: number): Observable<any> {
    let params = new HttpParams();
    if (customerName != "" && customerName != null)
      params = params.set("customerName", customerName);
    if (pan != "" && pan != null)
      params = params.set("PAN", pan.toUpperCase());
    if (propertyId != "" && propertyId != null)
      params = params.set("propertyId", propertyId);
    if (premises != "" && premises != null)
      params = params.set("premises", premises);
    if (unitNo != "" && unitNo != null)
      params = params.set("unitNo", unitNo);
    if (remark != "" && remark != null)
      params = params.set("remarks", remark);
    if (statusType != 0 && statusType != null)
      params = params.set("statusTypeId", statusType.toString());

    return this.http.get('/customer/getExcel', { params: params, responseType: 'blob' });
  }


  updateStatusAndremarks(model: any): Observable<any> {
    return this.http.put(`/customerProperty/status`, { 'custPropStatusObj': model });
  }
  deleteCustomer(Id: number,ownershipId:string): Observable<any> {
    return this.http.delete(`/customer/${Id}/${ownershipId}`);
  }
  getCustomerCount(): Observable<any> {
    return this.http.get(`/customer/getCustomerCount`);
  }

  getSellerName(id: string): Observable<any> {
    let params = new HttpParams();
    params = params.set("PropertyId", id);   
    return this.http.get('/SellerProperty', { params: params });
  }
  groupMail(id:string): Observable<any> {
    return this.http.get(`/customer/groupMail/${id}`);
  }
}
