import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { InvoiceService } from '../../services/invoiceservice/invoice.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-paymentsuccess',
  imports: [CommonModule],
  templateUrl: './paymentsuccess.component.html',
  styleUrl: './paymentsuccess.component.css'
})
export class PaymentsuccessComponent implements OnInit{
oid : number|null = null;

constructor(private router: Router,private update: InvoiceService) {}

  ngOnInit(): void {
   let value = localStorage.getItem('orderId');
    if(value){
      this.oid = Number(value);
    }
    if (this.oid) {
      this.update.updatePayment(this.oid).subscribe(
        (data) => {
          console.log(data);
        },
        (error) => {
          console.log(error);
        }
      );
    }
  }
 
  goToHome() {
    this.router.navigate(['/receipt']); // Change '/home' to your desired route
  }
}
