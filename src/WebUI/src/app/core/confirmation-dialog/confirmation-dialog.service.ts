/** Angular Imports */
import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'
import { ConfirmationDialogComponent} from './confirmation-dialog.component';

/** rxjs Imports */
import { Observable} from 'rxjs';
/**
 * Accounting service.
 */
@Injectable({
  providedIn: 'root'
})
export class ConfirmationDialogService {

  /**
   * @param {HttpClient} http Http Client to send requests.
   */
  constructor(private dialog: MatDialog) { }

  showDialog(msg: string): Observable<any> {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      hasBackdrop: false,
      maxHeight: 300,
      maxWidth: 1000,
      width: "500px",
      data: {
        'message': msg
      }

    });

   return dialogRef.afterClosed();
  }



}
