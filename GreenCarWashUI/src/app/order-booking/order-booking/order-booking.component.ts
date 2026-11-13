import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CarserviceService } from '../../services/carservice/carservice.service';
import { OrderHistoryService } from '../../services/orderService/order-history.service';
import { AddonService } from '../../services/addonservice/addon.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CustomerOrderaddonService } from '../../services/customer-orderaddon/customer-orderaddon.service';
import { PromocodeService } from '../../services/promocode/promocode.service';
import { PromoCode } from '../../Interface/promocode';

@Component({
  selector: 'app-order-booking',
  imports: [FormsModule,CommonModule],
  templateUrl: './order-booking.component.html',
  styleUrl: './order-booking.component.css'
})

export class OrderBookingComponent {
  currentOrderId: number | null = null;

  public value:number=0;
  userId: number | null = null;
  cars: any[] = [];
  selectedCar: number | null = null;
  isAddingNewCar: boolean = false;
  newCarMake: string = '';
  newCarModel: string = '';
  newCarImage: string = '';

  // Schedule and AddOns
  scheduleNow: boolean = false;
  scheduleDate: string = '';
  scheduleTime: string = '';
  location: string = '';

  addons: any[] = [];
  selectedAddons: any[] = [];
  totalAmount: number = 0;
  static amount: number = 0;
  packageId: number = 0;
  price: number = 0;


promoCodes: any[] = [];        
selectedPromoCode: any = null; 
promoDiscount: number = 0;    

  constructor(
    private router: Router,
    private carService: CarserviceService,
    private orderService: OrderHistoryService,
    private addonService: AddonService,
    private activatedRoute: ActivatedRoute,
    private orderaddonservice: CustomerOrderaddonService,
    private promoCodeService: PromocodeService
  ) {}

  ngOnChanges():void {
     this.fetchUserCars();
  }
  ngOnInit(): void {
     const userId=localStorage.getItem('userId');
     
  if (userId) {
    this.userId = Number(userId);
  }

     console.log('Navigating to order booking...', this.userId);
    // this.userId = this.getUserIdFromToken();
     this.activatedRoute.params.subscribe((params) => {
     
      this.packageId = +params['packageId']; 
      this.price = +params['price']; 
      this.totalAmount = this.price;

      console.log(`Received package ID: ${this.packageId} for $${this.totalAmount}`);
    });
    console.log('User ID:', this.userId);
    if (this.userId) {
      this.fetchUserCars();
      this.fetchAddOns();
      this.fetchPromoCodes();
    } else {
      console.error('User is not authenticated.');
    }
  }


fetchUserCars(): void {
  const userId=localStorage.getItem('userId');
  
  if (!this.userId) {
    console.error('User ID is null');
    return;
  }

  // this.userId=this.getUserIdFromToken();
  this.carService.getCarsByUserId(this.userId!).subscribe((cars) => {
    console.log('Fetched Cars:', cars);
    this.cars = cars;  
    if (cars.length > 0) {
    this.selectedCar = cars[0].carId;
    console.log('Default selected car:', this.selectedCar);
  }
  });
}

  fetchAddOns(): void {
    this.addonService.getAllAddOns().subscribe((addons) => {
      console.log('Fetched Add-ons:', addons); 
      this.addons = addons;
    });
  }

 
  fetchPromoCodes(): void {
    this.promoCodeService.getActivePromoCodes().subscribe((promoCodes: PromoCode[]) => {
      console.log('Fetched Promo Codes:', promoCodes); 
      this.promoCodes = promoCodes;
    });
  }

  addNewCar(): void {
    const newCar = {
      userId: this.userId,
      isActive: true,
      carmake: this.newCarMake,
      carmodel: this.newCarModel,
      carimage: this.newCarImage
    };
    console.log(newCar);
    console.log("user id: "+this.userId);

    this.carService.addCar(newCar).subscribe(
      (response) => {
        console.log('Car added successfully:', response);
        this.fetchUserCars(); 
      },
      (error) => {
        console.error('Error adding car:', error);
        if (error.error) {
          console.error('Error details:', error.error);
        }
      }
    );
  }

  calculateTotalAmount(): void {
    OrderBookingComponent.amount = this.price;
    console.log("package price: "+this.price);
    this.selectedAddons.forEach((addonId) => {
      let addon = this.addons.find((addon) => addon.addonID === addonId);
      if (addon) {
        OrderBookingComponent.amount += addon.price;
        console.log("After each addON: "+OrderBookingComponent.amount);
      }
    });
  //  console.log('Total amount after adding selected addon: $' + this.totalAmount);
    this.totalAmount = OrderBookingComponent.amount;
    console.log('Total amount after adding selected addon: $' + this.totalAmount);
  }

  bookOrder(): void {
   if (this.selectedCar === null) {
  alert('Please select a car');
  return;
}
console.log('Selected Car ID:', this.selectedCar);
    const userid=Number(localStorage.getItem('userId'));
    const orderData = {
      userId: userid,
      carId: this.selectedCar,
      washerId: 2,
      packageId: this.packageId,
      status: 'Pending',
      paymentStatus: 'Unpaid',
      totalAmount: this.totalAmount,
      location: this.location,
      scheduledDateTime: this.scheduleNow
        ? new Date().toISOString()
        : `${this.scheduleDate}T${this.scheduleTime}`
    };
    console.log('userId:', orderData.userId);
    console.log('carId:', orderData.carId);
    console.log('washerId:', orderData.washerId);
    console.log('pckageId:', orderData.packageId);
    console.log('status:', orderData.status);
    console.log('paymentStatus:', orderData.paymentStatus);
    console.log('location:', orderData.location);
    console.log("Amount Price: "+this.totalAmount);
    console.log("scheduledDateTime: "+orderData.scheduledDateTime);
    this.orderService.createOrder(orderData).subscribe(
      (response) => {
        console.log('Order created successfully', response);
    const orderId = response.orderId; 
    if (orderId === 0) {
      console.error('Error: Received order ID is 0');
    } else {
      console.log('Current Order ID:', orderId);
      this.currentOrderId = orderId;  
      
      this.addAddonsToOrder();
      if (this.currentOrderId) {
        this.router.navigate(['/customer-dashboard'], {
          queryParams: { orderId: this.currentOrderId }
        });
      } else {
        console.error('Order ID is missing');
      }
    }

        // this.router.navigate(['/customer-dashboard']);
      },
      (error) => {
        console.error('Error creating order', error);
      }
    );
  }

  toggleAddOns(item:any, event: any): void {
    console.log('Addon item object:', item);  
    console.log('Selected addon ID:', item.addonId);  
  
  const addonId = item.id;  
    if (event.target.checked) {
      console.log('Selected addon:', item.addonId);  
      console.log(parseInt(item.price));
      this.totalAmount += parseInt(item.price); 
      console.log("price added: "+this.totalAmount);
    //  this.applyPromoCode();  
      this.selectedAddons.push(item.addonId);  
    } else {
      this.totalAmount -= item.price;  
      console.log("price deducted: "+this.totalAmount);
      const index = this.selectedAddons.indexOf(item.addonId);
      if (index > -1) {
          this.selectedAddons.splice(index, 1);  
      }
    }
       

    console.log('Selected Add-ons:', this.selectedAddons);  
    this.applyPromoCodeDiscount();
  }

  addAddonsToOrder(): void {
    if (this.currentOrderId !== null && this.selectedAddons.length > 0) {
      console.log('Adding the following add-ons to order ID', this.currentOrderId, ':', this.selectedAddons);
      
    console.log('Number of selected add-ons:', this.selectedAddons.length);
     
      this.selectedAddons.forEach((addonId) => {
        console.log('Calling createOrderAddOn with orderId:', this.currentOrderId, 'addonId:', addonId);
        this.orderaddonservice.createOrderAddOn(this.currentOrderId as number, addonId).subscribe(
          (response: any) => {
            console.log('API response:', response); 
            console.log('Add-on added to orderaddon table successfully:', response);
          },
          (error: any) => {
            console.error('Error adding add-on to orderaddon table:', error);
          }
        );
      });
    } else {
      console.error('No add-ons selected or Order ID not available');
    }
  }
  
  applyPromoCodeDiscount(): void {
    if (this.selectedPromoCode) {
      const discountPercentage = this.selectedPromoCode.discountPercentage;
      this.promoDiscount = (this.totalAmount * discountPercentage) / 100;  
      this.totalAmount -= this.promoDiscount; 
      console.log(`Promo code applied: ${this.selectedPromoCode.codeName}`);
      console.log(`Discount: $${this.promoDiscount}`);
      console.log(`Total after discount: $${this.totalAmount}`);
    } else {
      console.log('No promo code selected');
      this.promoDiscount = 0;  
    }
  }
  
}



