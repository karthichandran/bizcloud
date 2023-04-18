/** Angular Imports */
import { Injectable } from '@angular/core';

/** rxjs Imports */
import { Observable,BehaviorSubject ,Subject} from 'rxjs';
import { ResizedEvent } from 'angular-resize-event';
/**
 * Accounting service.
 */
@Injectable({
  providedIn: 'root'
})
export class ResizeService {
  resize=new Subject<ResizedEvent>();  

  constructor() { }
}
