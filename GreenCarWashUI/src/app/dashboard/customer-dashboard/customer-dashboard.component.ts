import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderHistoryService } from '../../services/orderService/order-history.service';
import { PackageplanserviceService } from '../../services/packageplanservice/packageplanservice.service';
import { InvoiceService } from '../../services/invoiceservice/invoice.service';
import { EditprofileserviceService } from '../../services/customerservice/editprofileservice.service';
import { AddonService } from '../../services/addonservice/addon.service';
import { AddOn } from '../../Interface/addon';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-customer-dashboard',
  imports: [CommonModule, FormsModule],
  templateUrl: './customer-dashboard.component.html',
  styleUrl: './customer-dashboard.component.css'
})
export class CustomerDashboardComponent implements OnInit {
  isDropdownVisible = false;
  profileImage: string | null = null;
  userId: number | null = null;
  userName = 'Customer';
  userEmail = '';
  userInitial = 'C';

  orders: any[] = [];
  isPopupVisible = false;
  errorMessage = '';
  orderSearch = '';
  orderFilter = '';

  packages: any[] = [];
  addOns: AddOn[] = [];
  oId: number | null = null;
  invoice: any;
  isPopupVisible1 = false;
  flag = 0;

  // Stats
  totalOrders = 0;
  totalSpent = 0;
  completedOrders = 0;
  activeOrderStatus: string | null = null;
  activeOrder: any = null;

  pkgIcons = ['bi-droplet', 'bi-stars', 'bi-gem', 'bi-shield-check'];

  features = [
    { icon: 'bi-droplet-half', color: '#22c55e', title: 'Eco Friendly',   desc: '90% less water used' },
    { icon: 'bi-shield-check', color: '#3b82f6', title: 'Safe Products',  desc: 'Non-toxic biodegradable' },
    { icon: 'bi-lightning',    color: '#f59e0b', title: 'Fast Service',   desc: 'Done in under 30 min' },
    { icon: 'bi-star-fill',    color: '#a855f7', title: 'Top Rated',      desc: '4.9★ from 2k+ reviews' }
  ];

  private pkgFeatureMap: string[][] = [
    ['Exterior wash', 'Rinse & dry', 'Wheel clean'],
    ['Exterior + interior', 'Vacuum & wipe', 'Tire shine', 'Air freshener'],
    ['Full detail', 'Wax & polish', 'Engine clean', 'Odor removal', 'Ceramic coat']
  ];

  constructor(
    private http: HttpClient,
    private invoiceService: InvoiceService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private orderService: OrderHistoryService,
    private packageService: PackageplanserviceService,
    private profileService: EditprofileserviceService,
    private addonService: AddonService
  ) {}

  ngOnInit() {
    const uid = localStorage.getItem('userId');
    if (uid) this.userId = Number(uid);

    this.loadUserProfile();
    this.packageService.getAllPackages().subscribe(data => this.packages = data);
    this.addonService.getAllAddOns().subscribe(data => this.addOns = data);

    this.activatedRoute.queryParams.subscribe(params => {
      if (params['orderId']) {
        this.oId = params['orderId'];
        localStorage.setItem('orderId', params['orderId']);
        this.loadInvoices();
      }
    });

    if (this.userId) this.loadOrderStats();
  }

  loadUserProfile() {
    if (!this.userId) return;
    this.profileService.getUserById(this.userId).subscribe({
      next: (user: any) => {
        this.userName  = user.fullName || user.fullname || user.name || 'Customer';
        this.userEmail = user.email || '';
        this.userInitial = this.userName.charAt(0).toUpperCase();
      },
      error: () => {
        const stored = localStorage.getItem('user');
        if (stored) {
          const u = JSON.parse(stored);
          this.userName  = u.fullName || u.name || 'Customer';
          this.userEmail = u.email || '';
          this.userInitial = this.userName.charAt(0).toUpperCase();
        }
      }
    });
  }

  loadOrderStats() {
    if (!this.userId) return;
    this.orderService.getOrdersByUserId(this.userId).subscribe({
      next: (orders: any[]) => {
        this.orders = orders;
        this.totalOrders = orders.length;
        this.completedOrders = orders.filter(o => o.status === 'Completed').length;
        this.totalSpent = orders
          .filter(o => o.status === 'Completed')
          .reduce((sum, o) => sum + (o.totalPrice || 0), 0);
        const active = orders.find(o => !['Completed', 'Cancelled'].includes(o.status));
        if (active) {
          this.activeOrder = active;
          this.activeOrderStatus = active.status;
        }
      },
      error: () => {}
    });
  }

  get filteredOrders(): any[] {
    return this.orders.filter(o => {
      const matchSearch = !this.orderSearch ||
        (o.packageName || '').toLowerCase().includes(this.orderSearch.toLowerCase()) ||
        (o.carMake || '').toLowerCase().includes(this.orderSearch.toLowerCase()) ||
        (o.carModel || '').toLowerCase().includes(this.orderSearch.toLowerCase());
      const matchFilter = !this.orderFilter || o.status === this.orderFilter;
      return matchSearch && matchFilter;
    });
  }

  getStatusClass(status: string): string {
    const map: Record<string, string> = {
      Pending:   'status-pending',
      Accepted:  'status-accepted',
      InProcess: 'status-inprocess',
      Completed: 'status-completed',
      Cancelled: 'status-cancelled'
    };
    return map[status] || 'status-pending';
  }

  isStepDone(step: string): boolean {
    const order = ['Pending', 'Accepted', 'InProcess', 'Completed'];
    const current = order.indexOf(this.activeOrder?.status);
    return current > order.indexOf(step);
  }

  getAddonIcon(name: string): string {
    const n = name.toLowerCase();
    if (n.includes('wax') || n.includes('polish'))  return 'bi-stars';
    if (n.includes('vacuum') || n.includes('interior')) return 'bi-wind';
    if (n.includes('tire') || n.includes('wheel'))  return 'bi-circle';
    if (n.includes('engine'))                        return 'bi-gear-fill';
    if (n.includes('odor') || n.includes('freshener')) return 'bi-flower1';
    if (n.includes('ceramic') || n.includes('coat')) return 'bi-shield-fill';
    if (n.includes('seat') || n.includes('leather')) return 'bi-person-fill';
    return 'bi-plus-circle-fill';
  }

  getPkgFeatures(index: number): string[] {
    return this.pkgFeatureMap[index] || this.pkgFeatureMap[0];
  }

  scrollToPackages() {
    document.getElementById('packages-section')?.scrollIntoView({ behavior: 'smooth' });
  }

  toggleDropdown() { this.isDropdownVisible = !this.isDropdownVisible; }

  editProfile() {
    const userId = localStorage.getItem('userId');
    if (userId) this.router.navigate(['/profile/edit', userId]);
  }

  orderHistory() {
    const userId = localStorage.getItem('userId');
    if (userId) {
      this.fetchOrderHistory(Number(userId));
      this.isPopupVisible = true;
    } else {
      this.errorMessage = 'User not authenticated';
    }
  }

  fetchOrderHistory(userId: number) {
    this.orderService.getOrdersByUserId(userId).subscribe({
      next: (orders: any[]) => { this.orders = orders; },
      error: (err) => { this.errorMessage = 'Failed to fetch orders: ' + err.message; }
    });
  }

  logout() {
    localStorage.removeItem('user');
    this.router.navigate(['/login']);
  }

  bookNow(packageId: number, price: number) {
    this.router.navigate(['/order-booking', packageId, price]);
  }

  loadInvoices() {
    if (!this.oId) return;
    this.invoiceService.getInvoices(this.oId).subscribe(response => {
      this.checkOrderStatus(response.status);
      if (this.flag === 1) this.showPopup(response);
    }, error => console.error('Error loading invoices', error));
  }

  checkOrderStatus(os: string) {
    if (os === 'Completed') this.flag = 1;
  }

  showPopup(invoice: any) {
    this.invoice = {
      orderId:        invoice.orderId,
      invoiceNumber:  `INV-${invoice.orderId}-${Date.now()}`,
      carmake:        invoice.carmake,
      packageDetails: invoice.packageDetails,
      addOnDetails:   invoice.addOnDetails,
      totalAmount:    invoice.totalAmount,
      createdAt:      new Date()
    };
    this.isPopupVisible1 = true;
  }

  hidePopup() { this.isPopupVisible1 = false; }

  payNow(totalAmount: number) {
    const paymentData = {
      amount:     totalAmount,
      currency:   'usd',
      successUrl: 'http://localhost:4200/paymentsuccess',
      cancelUrl:  'http://localhost:4200/customer-dashboard'
    };
    this.http.post<{ url: string }>('https://localhost:7004/api/PaymentGateway/create-checkout', paymentData)
      .subscribe({
        next: (res) => { if (res?.url) window.location.href = res.url; },
        error: (err) => console.error('Payment creation failed', err)
      });
  }
}
