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
import { CustomerPaymentComponent } from 'app/customer-payment/customer-payment.component';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { MatDialogModule } from '@angular/material/dialog';
import { MatExpansionModule } from '@angular/material/expansion';
import { AuthenticationGuard } from 'app/core/authentication/authentication.guard';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
const routes = [
  {
    path: 'client-payment',
    component: CustomerPaymentComponent,
    canActivate: [AuthenticationGuard]
  }
];

@NgModule({
  declarations: [
    CustomerPaymentComponent
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
    MatDialogModule,
    MatExpansionModule,
    NgxMatSelectSearchModule
  ],
  exports: []
})
export class CustomerPaymentModule {
}
