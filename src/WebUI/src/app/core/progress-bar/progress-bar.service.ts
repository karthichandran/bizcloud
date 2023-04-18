/** Angular Imports */
import { Inject, Injectable, EventEmitter ,ElementRef,Renderer2,RendererFactory2} from '@angular/core';

import { DOCUMENT } from '@angular/common';
/**
 * Progress bar service.
 */
@Injectable()
export class ProgressBarService {


    csmSpinnerEl: any;
    private renderer: Renderer2;
  /** Progress bar update event. */
  public updateProgressBar: EventEmitter<any>;
  /** Denotes the number of requests currently running. */
  private requestsRunning = 0;

  /**
   * Initializes progress bar update event.
   */
    constructor(rendererFactory: RendererFactory2, @Inject(DOCUMENT) private _document: any) {
        this.updateProgressBar = new EventEmitter();

        this.renderer = rendererFactory.createRenderer(null, null);

        // Get the splash screen element

        this.csmSpinnerEl = this._document.body.querySelector('.csm-spinner-container');

       
  }

  /**
   * Returns the number of Http requests currently running.
   * @returns {number} Number of Http requests currently running.
   */
  public getRequestsRunning(): number {
    return this.requestsRunning;
  }

  /**
   * Increments the number of Http requests currently running
   * and emits a progress bar update event `indeterminate` when the first request is run.
   */
  public increase() {
    this.requestsRunning++;
    if (this.requestsRunning === 1) {
        this.updateProgressBar.emit('indeterminate');

        this.showSpinner();
    }
  }

  /**
   * Decrements the number of Http requests currently running
   * and emits a progress bar update event `none` when no request is running.
   */
  public decrease() {
    if (this.requestsRunning > 0) {
      this.requestsRunning--;
      if (this.requestsRunning === 0) {
          this.updateProgressBar.emit('none');
          this.hideSpinner();
      }
    }
  }

    showSpinner(): void {
       // let el = this.spinnerEl.nativeElement;
        this.renderer.setStyle(this.csmSpinnerEl, "display", "inline");
    }

    hideSpinner(): void {
       // let el = this.spinnerEl.nativeElement;
        this.renderer.setStyle(this.csmSpinnerEl, "display", "none");
    }

}
