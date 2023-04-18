import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatRadioModule } from '@angular/material/radio';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatStepperModule } from '@angular/material/stepper';
import { FuseSharedModule } from '@fuse/shared.module';
import { GridModule } from '@fuse/components';
import { CustomerBillingComponent } from 'app/customer-billing/customer-billing.component';
import { CustomerBillingDialogComponent } from 'app/customer-billing/customer-billing-dialog/customer-billing-dialog.component';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { MatDialogModule } from '@angular/material/dialog';
import { AuthenticationGuard } from 'app/core/authentication/authentication.guard';
const routes = [
  {
    path: 'customer-billing',
    component: CustomerBillingComponent,
    canActivate: [AuthenticationGuard]
  }
];

@NgModule({
  declarations: [
    CustomerBillingComponent,
    CustomerBillingDialogComponent
  ],
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    MatTabsModule,
    MatButtonModule,
    MatRadioModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatStepperModule,
    FuseSharedModule,
    GridModule,
    NgxDatatableModule,
    MatDialogModule
  ],
  exports:[CustomerBillingDialogComponent]
})
export class CustomerBillingModule {
}
