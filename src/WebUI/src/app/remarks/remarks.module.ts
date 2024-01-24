import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import {MatRadioModule} from '@angular/material/radio';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { FuseSharedModule } from '@fuse/shared.module';
import { GridModule } from '@fuse/components';
import { RemarksComponent } from 'app/remarks/remarks.component';
import { AuthenticationGuard } from 'app/core/authentication/authentication.guard';
import { MatDialogModule } from '@angular/material/dialog';
const routes = [
  {
    path: 'remark',
    component: RemarksComponent,
    canActivate: [AuthenticationGuard]
  }
];

@NgModule({
  declarations: [
    RemarksComponent   
  ],
  imports: [
    RouterModule.forChild(routes),
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
    MatDialogModule
  ]
})
export class RemarksModule {
}
