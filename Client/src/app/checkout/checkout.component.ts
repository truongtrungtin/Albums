// checkout.component.ts
import { Component } from '@angular/core';
import { CheckoutService } from './checkout.service';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent {
  public currentStep: string = 'address';
  private selectedOptionSubject = new BehaviorSubject<number>(1);
  selectedOption$ = this.selectedOptionSubject.asObservable();

  constructor(private checkoutService: CheckoutService) {
    // You can use this.accountService.isAuthenticated() to check if the user is authenticated
  }


  getNextStep(): string {
    // Implement your logic to determine the next step
    switch (this.currentStep) {
      case 'address':
        return 'shipment';
      case 'shipment':
        return 'review';
      default:
        return 'address'; // Loop back to the beginning if needed
    }
  }

  setCurrentStep(step: string): void {
    this.currentStep = step;
  }
}
