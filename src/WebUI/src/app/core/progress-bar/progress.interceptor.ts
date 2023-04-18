/** Angular Imports */
import { Injectable } from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse} from '@angular/common/http';

/** rxjs Imports */
import { Observable } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';

/** Custom Services */
import { ProgressBarService } from './progress-bar.service';
import { FuseProgressBarService } from '@fuse/components/progress-bar/progress-bar.service';
/**
 * Http Request interceptor to start/stop loading the progress bar.
 */
@Injectable()
export class ProgressInterceptor implements HttpInterceptor {

  /**
   * @param {ProgressBarService} progressBarService Progress Bar Service.
   */
  constructor(private progressBarService: ProgressBarService, private fuseProgressBarService: FuseProgressBarService) {
    fuseProgressBarService.setMode('query');
  }

  /**
   * Intercepts a Http request to start loading the progress bar for a pending request
   * and stop when a response or error is received.
   */
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
   // this.fuseProgressBarService.show();
    if (request.url.includes('refresh-token'))
      return next.handle(request)

    this.progressBarService.increase();
    return next.handle(request)
      .pipe(
        tap(event => {
          if (event instanceof HttpResponse) {
           // this.fuseProgressBarService.hide();
            this.progressBarService.decrease();
          }
        })
      )
      .pipe(
        catchError(error => {
        //  this.fuseProgressBarService.hide();
          this.progressBarService.decrease();
          throw error;
        })
      );
  }

}
