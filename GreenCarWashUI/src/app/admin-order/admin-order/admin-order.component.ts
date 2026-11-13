import { Component, OnInit } from '@angular/core';
import { AdminOrderService } from '../../services/admin-order/admin-order.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-order',
  imports: [CommonModule,FormsModule],
  templateUrl: './admin-order.component.html',
  styleUrl: './admin-order.component.css'
})
export class AdminOrderComponent implements OnInit {
  orders: any[] = [];
  selectedOrder: any = null;
  acceptedOrders: any[] = [];
  inProcessOrders: any[] = [];
  completedOrders: any[] = [];
  canceledOrders: any[] = [];
  modalVisible: boolean = false;
  orderStatuses = ['Accepted', 'In-Process', 'Completed', 'Canceled'];

  constructor(private orderService: AdminOrderService, private router: Router) {}

  goBack() { this.router.navigate(['/admin-dashboard']); }

  ngOnInit() {
    this.loadPendingOrders();
  }

  loadPendingOrders() {
    this.orderService.getPendingOrders().subscribe((data) => {
      this.orders = data;
    });
  }

  assignWasher(orderId: number) {
    this.orderService.assignRandomWasher(orderId).subscribe((updatedOrder) => {
      this.loadPendingOrders(); // Reload the orders
      alert('Washer assigned successfully!');
    });
  }

  showStatusOrders(status: string) {
    this.orderService.getOrdersByStatus(status).subscribe(
      (data) => {
        if (data) {
          this.selectedOrder = data.filter((order: { orderStatus: string; }) => order.orderStatus === status);
          this.modalVisible = true;
        }
      });
  }

  closeModal() {
    this.modalVisible = false;
    this.selectedOrder = null;
  }
}

// getCurrentOrders() {
//   if(this.uId){
//     this.washservice.getViewOrders(this.uId).subscribe(orders => {
//         this.acceptedOrders = orders.filter((order: { orderStatus: string; }) => order.orderStatus === 'Accepted');
//         this.inProcessOrders = orders.filter((order: { orderStatus: string; }) => order.orderStatus === 'In-Process');
//       });
//   }