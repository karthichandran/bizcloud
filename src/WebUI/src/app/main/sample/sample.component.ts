import { Component } from '@angular/core';
import * as Xlsx from 'xlsx';

import { FuseTranslationLoaderService } from '@fuse/services/translation-loader.service';
import { ProgressBarService} from '../../core/progress-bar/progress-bar.service';

import { locale as english } from './i18n/en';
import { locale as turkish } from './i18n/tr';

@Component({
    selector   : 'sample',
    templateUrl: './sample.component.html',
    styleUrls  : ['./sample.component.scss']
})
export class SampleComponent
{
    /**
     * Constructor
     *
     * @param {FuseTranslationLoaderService} _fuseTranslationLoaderService
     */
    constructor(
      private _fuseTranslationLoaderService: FuseTranslationLoaderService,
      private progrss:ProgressBarService
    )
    {
        this._fuseTranslationLoaderService.loadTranslations(english, turkish);
    }

  showProgress() {
    this.progrss.showSpinner();
    setTimeout(() => { this.progrss.hideSpinner(); }, 10000);
  }

  exportToExl() {

    let data = [{ 'name': 'name1', 'Id': "1", "Address": "75b" }, { 'name': 'name2', 'Id': "1", "Address": "75f" }, { 'name': 'name3', 'Id': "1", "Address": "75e" }, { 'name': 'name4', 'Id': "1", "Address": "75f" }];
    const ws: Xlsx.WorkSheet = Xlsx.utils.json_to_sheet(data);
    const wb: Xlsx.WorkBook = Xlsx.utils.book_new();
    Xlsx.utils.book_append_sheet(wb, ws, "Sheet1");
    Xlsx.writeFile(wb, 'testing.xls');
  }

  ImportFile(files: FileList) {
    const reader: FileReader = new FileReader();
    let data;
    reader.onload = (e: any) => {
      /* read workbook */
      const bstr: string = e.target.result;
      const wb: Xlsx.WorkBook = Xlsx.read(bstr, { type: 'binary' });

      /* grab first sheet */
      //const wsname: string = wb.SheetNames[0];
      //const ws: Xlsx.WorkSheet = wb.Sheets[wsname];

      ///* save data */
      //data = Xlsx.utils.sheet_to_json(ws, { header: 1 });
      //console.log(data);

      //For multiple sheets
      for (var i = 0; i < wb.SheetNames.length; i++) {
        const wsname: string = wb.SheetNames[i];
        const ws: Xlsx.WorkSheet = wb.Sheets[wsname];

        /* save data */
        data = Xlsx.utils.sheet_to_json(ws, { header: 1 });
        console.log(data);
      }
    };
    reader.readAsBinaryString(files[0]);
   
  }


}
