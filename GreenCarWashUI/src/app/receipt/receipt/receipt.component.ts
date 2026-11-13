import { Component } from '@angular/core';
import { Receipt } from '../../Interface/receipt';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ReceiptService } from '../../services/receiptservice/receipt.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-receipt',
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './receipt.component.html',
  styleUrl: './receipt.component.css'
})
export class ReceiptComponent {
  receipt: Receipt | null = null;
  errorMessage: string | null = null;
  oid : number|null = null;

  constructor( private route: ActivatedRoute, private receiptService: ReceiptService ,private router: Router) {}

  ngOnInit(): void {
    let value = localStorage.getItem('orderId');
    if(value){
      this.oid = Number(value);
    }
    console.log("Retrieved orderId:", this.oid); // Check what you get here
    if (this.oid) {
      this.receiptService.getReceipt(this.oid).subscribe(
        (data) => {
          this.receipt = data;
        },
        (error) => {
          this.errorMessage = error.error.error;
        }
      );
    }
  }
   // Method to handle the redirection
   closeReceipt() {
    // Redirect to another page (e.g., customer dashboard)
    this.router.navigate(['/review']);
  }
}
