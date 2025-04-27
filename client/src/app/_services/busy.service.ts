import { inject, Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

// Services are not destroyed when the component is destroyed, so we can use them to manage the state of the spinner.
// Services are destroyed when for example the page is reloaded.
@Injectable({
  providedIn: 'root',
})
export class BusyService {
  busyRequestCount = 0;
  private spinnerService = inject(NgxSpinnerService);

  // Method to show the busy spinner
  busy() {
    this.busyRequestCount++;
    this.spinnerService.show(undefined, {
      type: 'line-scale-party',
      bdColor: 'rgba(255, 255, 255, 0)',
      color: '#333333',
    });
  }

  idle() {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this.spinnerService.hide();
    }
  }

  isBusy() {
    return this.busyRequestCount <= 0;
  }
}
