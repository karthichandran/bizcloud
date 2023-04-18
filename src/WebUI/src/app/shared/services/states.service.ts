/** Angular Imports */
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { StateDto } from '../../ReProServices-api';

/** rxjs Imports */
import { Observable } from 'rxjs';

/**
 * Accounting service.
 */
@Injectable({
  providedIn: 'root'
})
export class StatesService {

  /**
   * @param {HttpClient} http Http Client to send requests.
   */
  constructor(private http: HttpClient) { }

  
  /**
   * @returns {Observable<any>} Loan products data.
   */
  getStates(): Observable<StateDto[]> {
    return this.http.get<StateDto[]>('/States');
  }
   
}
