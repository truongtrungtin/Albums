import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  loadingRegCount = 0;
  constructor(private spinnerService: NgxSpinnerService) { }

  loading(){
    this.loadingRegCount++;
    this.spinnerService.show();
  }

  idle(){
    this.loadingRegCount--;
    if(this.loadingRegCount <= 0){
      this.loadingRegCount = 0;
      this.spinnerService.hide();
    }
  }
}
