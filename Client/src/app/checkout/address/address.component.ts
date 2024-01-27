import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Address } from 'src/app/shared/models/address';
import { CheckoutComponent } from '../checkout.component';

@Component({
  selector: 'app-address',
  templateUrl: './address.component.html',
  styleUrls: ['./address.component.scss'],
})
export class AddressComponent implements OnInit {
  addressForm: FormGroup;

  constructor(private formBuilder: FormBuilder, private checkoutComponent: CheckoutComponent) {
    this.addressForm = this.formBuilder.group({
      Fname: ['', Validators.required],
      Lname: ['', Validators.required],
      Street: ['', Validators.required],
      City: ['', Validators.required],
      State: ['', Validators.required],
      ZipCode: ['', [Validators.required, Validators.pattern(/^\d{5}(?:-\d{6})?$/)]],
    });
   
  }
  ngOnInit(): void {
   
  }

  onSubmit(): void {
    if (this.addressForm.valid) {
      // Process the address data (e.g., send it to the server)
      const addressData: Address = this.addressForm.value;
      console.log(addressData);
    } else {
      // Mark all controls as touched to show validation messages
      Object.keys(this.addressForm.controls).forEach((control) => {
        this.addressForm.get(control)?.markAsTouched();
      });
    }
  }

  hasError(controlName: string, errorType: string): boolean  {
    const control = this.addressForm.get(controlName)!;
  
    // Now TypeScript knows that control is not null
    return control.hasError(errorType) && (control.touched || control.dirty);
  }

  onNextClick(): void {
    if (this.addressForm.valid) {
      // Navigate to the next step
      const nextStep = this.checkoutComponent.getNextStep();
      this.checkoutComponent.setCurrentStep(nextStep);
    } else {
      // Mark all controls as touched to show validation messages
      Object.keys(this.addressForm.controls).forEach((control) => {
        this.addressForm.get(control)?.markAsTouched();
      });
    }
  }
  
}
