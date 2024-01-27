// shipment.component.ts
import { Component } from '@angular/core';
import { deliveryOption } from 'src/app/shared/models/deliveryOption';
import { BasketService } from 'src/app/basket/basket.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CheckoutComponent } from '../checkout.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-shipment',
  templateUrl: './shipment.component.html',
  styleUrls: ['./shipment.component.scss'],
})
export class ShipmentComponent {
  deliveryOptions: deliveryOption[] = [
    {
      id: 1,
      name: 'FedEx',
      deliveryTime: '2-3 days',
      description: 'Fast and reliable',
      price: 100.0,
    },
    {
      id: 2,
      name: 'DTDC',
      deliveryTime: '3-5 days',
      description: 'Economical option',
      price: 80.0,
    },
    {
      id: 3,
      name: 'USPS',
      deliveryTime: '4-6 days',
      description: 'Standard shipping',
      price: 50.0,
    },
    {
      id: 4,
      name: 'UPS',
      deliveryTime: '1-2 days',
      description: 'Express delivery',
      price: 150.0,
    },
  ];
  selectedOption: number | undefined;
  shipmentForm: FormGroup;

  constructor(
    private basketService: BasketService,
    private formBuilder: FormBuilder,
    private router: Router,
    private checkoutComponent: CheckoutComponent
  ) {
    this.shipmentForm = this.formBuilder.group({
      selectedOption: [this.selectedOption],
    });
    this.selectedOption = this.deliveryOptions[0].id;
    this.updateShipmentPrice();
  }

  updateShipmentPrice() {
    const selectedDeliveryOption = this.deliveryOptions.find(
      (option) => option.id === this.selectedOption
    );

    if (selectedDeliveryOption) {
      this.basketService.updateShippingPrice(selectedDeliveryOption.price);
    }
  }

  onNextClick(): void {
    
    if (this.shipmentForm.valid) {
      // Navigate to the next step
      const nextStep = this.checkoutComponent.getNextStep();
      this.checkoutComponent.setCurrentStep(nextStep);
    } else {
      // Mark all controls as touched to show validation messages
      Object.keys(this.shipmentForm.controls).forEach((control) => {
        this.shipmentForm.get(control)?.markAsTouched();
      });
    }
  }
}
