import { NgModule } from '@angular/core';
import {MomentPipe } from './date-format.pipe';




@NgModule({
  declarations: [
    MomentPipe
  ],
  imports: [
  ],
  exports: [
    MomentPipe
  ]
})
export class SharedModule {
}
