import { provideRouter,RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home/home.component';
import { LoginComponent } from './Authentication/login/login.component';
import { SignupComponent } from './Authentication/signup/signup.component';
import { CustomerDashboardComponent } from './dashboard/customer-dashboard/customer-dashboard.component';
import { EditProfileComponent } from './customer/edit-profile/edit-profile.component';
import { AdminDashboardComponent } from './dashboard/admin-dashboard/admin-dashboard.component';
import { PackagePlanComponent } from './packageplan/package-plan/package-plan.component';
import { AddonComponent } from './addon/addon/addon.component';
import { WashersComponent } from './washers/washers/washers.component';
import { AdminUserComponent } from './admin-user/admin-user/admin-user.component';
import { PromocodeComponent } from './promocode/promocode/promocode.component';
import { AdminOrderComponent } from './admin-order/admin-order/admin-order.component';
import { OrderBookingComponent } from './order-booking/order-booking/order-booking.component';
import { ReceiptComponent } from './receipt/receipt/receipt.component';
import { PaymentsuccessComponent } from './paymentsuccess/paymentsuccess/paymentsuccess.component';
import { ReviewComponent } from './review/review/review.component';
import { WasherDashboardComponent } from './dashboard/washer-dashboard/washer-dashboard.component';
import { WasherorderHistoryComponent } from './washerorder-history/washerorder-history/washerorder-history.component';

//import { FormsModule } from '@angular/forms';

export const routes: Routes = [  
    { path: 'home', component: HomeComponent },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: SignupComponent},
    { path: 'customer-dashboard', component: CustomerDashboardComponent},
    { path: 'washer-dashboard', component: WasherDashboardComponent},
    { path: 'admin-dashboard', component: AdminDashboardComponent },
    { path: 'profile/edit/:id', component: EditProfileComponent},
    { path: 'order-booking/:packageId/:price', component: OrderBookingComponent},
    { path: 'package-plan', component: PackagePlanComponent},
    { path: 'addon', component: AddonComponent },
    { path: 'washer', component: WashersComponent },
    { path: 'admin-user', component: AdminUserComponent },
    { path: 'promocode', component: PromocodeComponent },
    { path: 'admin-order', component: AdminOrderComponent },
    { path: 'receipt', component: ReceiptComponent },
    { path: 'paymentsuccess', component: PaymentsuccessComponent},
    { path: 'review' ,component: ReviewComponent},
    { path: 'washerorder-history', component: WasherorderHistoryComponent},
    { path: '', redirectTo: '/home', pathMatch: 'full' }
];
