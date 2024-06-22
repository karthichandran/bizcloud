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
import { FuseSharedModule } from '@fuse/shared.module';
import { GridModule } from '@fuse/components';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { StatusReportComponent } from './status-report/status-report.component';
import { SellerComplianceReportComponent } from './seller-compliance/seller-compliance-report.component';
import { TdsRemittanceReportComponent } from './tds-remittance/tds-remittance-report.component';
import { LotSummaryReportComponent} from './lot-Summary/lot-summary-report.component';
import { AuthenticationGuard } from 'app/core/authentication/authentication.guard';
import { StatementOfAccountComponent } from './statement-of-account/statement-of-account.component';
import { PasswordSettingReportComponent } from './password-setting-report/password-setting-report.component';
import { TaxPaymentReportComponent } from './tax-payment-report/tax-payment-report.component';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
const routes = [
  {
    path: 'status-report',
    component: StatusReportComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: 'seller-compliance-report',
    component: SellerComplianceReportComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: 'tds-remittance-report',
    component: TdsRemittanceReportComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: 'lot-summary-report',
    component: LotSummaryReportComponent,
    canActivate: [AuthenticationGuard]
  },
   {
    path: 'statement-of-account-report',
     component: StatementOfAccountComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: 'password-setting-report',
    component: PasswordSettingReportComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: 'tax-payment-report',
    component: TaxPaymentReportComponent,
    canActivate: [AuthenticationGuard]
  }

];

@NgModule({
  declarations: [
    StatusReportComponent,
    SellerComplianceReportComponent,
    TdsRemittanceReportComponent,
    LotSummaryReportComponent,
    StatementOfAccountComponent,
    PasswordSettingReportComponent,
    TaxPaymentReportComponent
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
    FuseSharedModule,
    GridModule,
    NgxDatatableModule,
    NgxMatSelectSearchModule
  ]
})
export class ReportsModule {
}
