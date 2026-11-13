import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { AdminUserService } from '../../services/admin-user/admin-user.service';
import { WashersService } from '../../services/washers/washers.service';
import { AdminOrderService } from '../../services/admin-order/admin-order.service';
import { PromocodeService } from '../../services/promocode/promocode.service';

@Component({
  selector: 'app-admin-dashboard',
  imports: [RouterModule, CommonModule, FormsModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.css'
})
export class AdminDashboardComponent implements OnInit {
  currentSection: string = '';
  modalVisible: boolean = false;
  modalTitle: string = '';
  formFields: any[] = [];
  actionType: string = '';

  totalUsers: number = 0;
  activeWashers: number = 0;
  pendingOrders: number = 0;
  activePromoCodes: number = 0;

  menuItems = [
    { key: 'userManagement',      label: 'User Management',         icon: 'bi-people' },
    { key: 'washerManagement',    label: 'Washer Management',       icon: 'bi-person-badge' },
    { key: 'servicePlanManagement', label: 'Service Plans',         icon: 'bi-card-list' },
    { key: 'addOnManagement',     label: 'Add-On Management',       icon: 'bi-plus-square' },
    { key: 'promoCodeManagement', label: 'Promo Codes',             icon: 'bi-tag' },
    { key: 'washNowSchedule',     label: 'Order Management',        icon: 'bi-calendar-check' },
  ];

  getActiveLabel(): string {
    return this.menuItems.find(m => m.key === this.currentSection)?.label ?? 'Dashboard';
  }

  // This method toggles the current section when clicked
  toggleButtons(section: string): void {
    this.currentSection = section;
  }

  constructor(
    private router: Router,
    private adminUserService: AdminUserService,
    private washersService: WashersService,
    private adminOrderService: AdminOrderService,
    private promocodeService: PromocodeService
  ) {}

  ngOnInit(): void {
    this.adminUserService.getAllUsers().subscribe(users => this.totalUsers = users.length);
    this.washersService.getAllWashers('Washer').subscribe(washers => {
      this.activeWashers = washers.filter(w => w.isActive).length;
    });
    this.adminOrderService.getPendingOrders().subscribe(orders => this.pendingOrders = orders.length);
    this.promocodeService.getActivePromoCodes().subscribe(codes => this.activePromoCodes = codes.length);
  }

  // Navigate to the PackagePlan component
  logout() {
    localStorage.clear();
    this.router.navigate(['/login']);
  }

  goToPackagePlan() {
    this.router.navigate(['/package-plan']);
  }

   // Navigate to the AddOn component
   goToAddOn() {
    this.router.navigate(['/addon']);
  }

  // Navigate to the Washer component
  goToWasher() {
    this.router.navigate(['/washer']);
  }
// Navigate to the Admin-User component
goToUser() {
  this.router.navigate(['/admin-user']);
}

// Navigate to the PromoCode component
goToPromoCode() {
  this.router.navigate(['/promocode']);
}

 // Navigate to the Order component
 goToOrder() {
  this.router.navigate(['/admin-order']);
}


  // This method will control the modal visibility and title based on actions
  openModal(action: string): void {
    this.modalVisible = true;
    this.actionType = action;
    

    // switch(action) {
    //   case 'addUser':
    //     this.modalTitle = 'Add User';
    //     this.formFields = [
    //       { id: 'username', label: 'Username' },
    //       { id: 'email', label: 'Email' },
    //       { id: 'password', label: 'Password' }
    //     ];
    //     break;
    //   case 'editUser':
    //     this.modalTitle = 'Edit User';
    //     this.formFields = [
    //       { id: 'username', label: 'Username' },
    //       { id: 'email', label: 'Email' },
    //     ];
    //     break;
    //   case 'addCar':
    //     this.modalTitle = 'Add Car';
    //     this.formFields = [
    //       { id: 'carModel', label: 'Car Model' },
    //       { id: 'carNumber', label: 'Car Number' },
    //     ];
    //     break;
    //   case 'editCar':
    //     this.modalTitle = 'Edit Car';
    //     this.formFields = [
    //       { id: 'carModel', label: 'Car Model' },
    //     ];
    //     break;
    //   case 'activeInactiveUser':
    //     this.modalTitle = 'Activate/Deactivate User';
    //     this.formFields = [
    //       { id: 'status', label: 'Status (Active/Inactive)' },
    //     ];
    //     break;
    //   default:
    //     this.modalTitle = 'Unknown Action';
    //     this.formFields = [];
    // }
  }

  // Optional: Close modal functionality
  closeModal(): void {
    this.modalVisible = false;
    this.formFields = []; // Clear form fields when modal is closed
  }

  // Submit form logic
  submitForm(): void {
    console.log(`Form submitted for ${this.modalTitle}`);
    this.closeModal();
  }
}