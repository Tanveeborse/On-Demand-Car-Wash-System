import { Component } from '@angular/core';
import { WasherservicService } from '../../services/washerservice/washerservic.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-washerorder-history',
  imports: [FormsModule,CommonModule],
  templateUrl: './washerorder-history.component.html',
  styleUrl: './washerorder-history.component.css'
})
export class WasherorderHistoryComponent {

  loading = false;
  uId : number | null = null; // Replace with dynamic user ID as needed
  pastOrders: any[] = [];  // Store the orders
  //isPopupVisible = false; // Toggle visibility of the order history popup
  errorMessage: string | null = null;

  constructor(private washerDashboardService: WasherservicService) {}
  ngOnInit(): void {
    let value = localStorage.getItem('userId');
    if(value){
      this.uId = Number(value);
    }
    console.log('history.ts: ngOnInit: User ID:', this.uId);
    if (this.uId) {
      this.loadPastOrders();
    } else {
      this.errorMessage = 'User not authenticated';
    }
  }
  loadPastOrders(): void {
    console.log('Order History loading ');
    if (this.uId !== null) {
      this.washerDashboardService.getOrderHistory(this.uId).subscribe(
        (orders) => {
          this.pastOrders = orders;
          console.log('Fetched past orders:', orders);
        },
        (error) => {
          console.error('Error fetching order history', error);
        }
      );
    }
    else{
      console.error('User not authenticated');
    }
  }
}

