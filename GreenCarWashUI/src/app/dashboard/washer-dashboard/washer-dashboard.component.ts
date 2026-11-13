import { Component } from '@angular/core';
import { WasherservicService } from '../../services/washerservice/washerservic.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-washer-dashboard',
  imports: [FormsModule,CommonModule],
  templateUrl: './washer-dashboard.component.html',
  styleUrl: './washer-dashboard.component.css'
})

export class WasherDashboardComponent {
  constructor(private router: Router, private washerservice: WasherservicService) {}
  pendingOrders: any[] = [];
  acceptedOrders: any[] = [];
  inProcessOrders: any[] = [];
  washersLeaderboard: any[] = [];
  uId : number | null = null;
  errorMessage: string | null = null;
  
  ngOnInit() {
    let value = localStorage.getItem('userId');
    if(value){
      this.uId = Number(value);
    }
    console.log('User ID is:', this.uId);
    if (this.uId) {
      this.allPendingOrders();
      this.getCurrentOrders();
      this.leaderboard();
    } else {
      this.errorMessage = 'User not authenticated';
    }
  }

  getOrderHistory()
  {
    this.router.navigate(['/washerorder-history']);
  }
  allPendingOrders() {
    if(this.uId){
      this.washerservice.getPendingOrders().subscribe(orders => {
          this.pendingOrders = orders;
      });
    }
  }
  getCurrentOrders() {
    if(this.uId){
      this.washerservice.getViewOrders(this.uId).subscribe(orders => {
          this.acceptedOrders = orders.filter((order: { orderStatus: string; }) => order.orderStatus === 'Accepted');
          this.inProcessOrders = orders.filter((order: { orderStatus: string; }) => order.orderStatus === 'In-Process');
        });
    }
  }
  leaderboard() {
    if(this.uId){
      this.washerservice.getLeaderboard().subscribe(orders => {
          this.washersLeaderboard = orders;
      });
    }
  }
  // Action when Logout is clicked
  logout() {
    localStorage.removeItem('user'); // Removing user data
    this.router.navigate(['/login']); // Redirect to Login
  }
  // Action when Accept is clicked
  acceptWash(orderId: number) {
    console.log('Accept wash clicked');
    if(this.uId && orderId){
      this.washerservice.acceptOrder(orderId, this.uId).subscribe(
        response => {
          console.log(response);
        }
      );
    }
  }
  // Action when Start is clicked
  startWash(orderId: number) {
    console.log('Start wash clicked');
    if(this.uId && orderId){
      this.washerservice.updateStartTime(orderId).subscribe(
        response => {
          console.log(response);
        }
      );
      this.washerservice.updateOrderStatus(orderId, 'In-Process').subscribe(
        response => {
          console.log(response);
        }
      );
    }
  }
  // Action when Completed is clicked
  completedWash(orderId: number){
    console.log('Completed wash clicked. OrderId: ', orderId);
    if(this.uId && orderId){
      console.log('OrderId: ', orderId);
      this.washerservice.updateOrderStatus(orderId, 'Completed').subscribe(
        response => {
          console.log(response);
        }
      );
      this.washerservice.updateEndTime(orderId).subscribe(
        response => {
          console.log('Wash ended successfully');
        }
      );
    }
  }
}

