import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  packages = [
    { name: 'Basic Wash', price: 25, description: 'Quick exterior wash and dry.', icon: 'bi-droplet', featured: false, features: ['Exterior Wash', 'Rinse & Dry', 'Window Clean'] },
    { name: 'Full Wash', price: 50, description: 'Exterior and interior cleaning.', icon: 'bi-droplet-half', featured: false, features: ['Everything in Basic', 'Interior Vacuum', 'Dashboard Wipe'] },
    { name: 'Deluxe Wash', price: 75, description: 'Full service with waxing and tire shine.', icon: 'bi-stars', featured: true, features: ['Everything in Full', 'Waxing', 'Tire Shine', 'Air Freshener'] },
    { name: 'Premium Wash', price: 105, description: 'Deluxe service plus interior detailing.', icon: 'bi-gem', featured: false, features: ['Everything in Deluxe', 'Interior Detailing', 'Leather Conditioning'] },
    { name: 'Full Detailing', price: 120, description: 'Complete interior & exterior detailing.', icon: 'bi-award', featured: false, features: ['Everything in Premium', 'Engine Cleaning', 'Upholstery Cleaning', 'Paint Protection'] },
  ];

  features = [
    { icon: 'bi-calendar-check', title: 'Easy Scheduling', desc: 'Book your wash in seconds and schedule future appointments.' },
    { icon: 'bi-shield-lock', title: 'Secure Payments', desc: 'Your payment info is always safe with encrypted transactions.' },
    { icon: 'bi-bell', title: 'Real-time Alerts', desc: 'Get notified when your car is ready for pickup.' },
    { icon: 'bi-star', title: 'Ratings & Reviews', desc: 'Read honest reviews from thousands of happy customers.' },
    { icon: 'bi-leaf', title: 'Eco-Friendly', desc: 'We use biodegradable products to protect the environment.' },
    { icon: 'bi-person-check', title: 'Verified Washers', desc: 'All our washers are background-checked and trained professionals.' },
  ];

  testimonials = [
    { name: 'Sarah M.',   role: 'Regular Customer', text: 'Best car wash I have ever used. My car looks brand new every time and I love that they use eco-friendly products!' },
    { name: 'James K.',   role: 'Premium Member',   text: 'Super fast, super clean. The booking process is seamless and the washers are always professional and on time.' },
    { name: 'Priya R.',   role: 'New Customer',     text: 'Tried the deluxe package and was blown away. The interior detailing was spotless. Will definitely be back!' },
  ];

  bookNow(packageName: string, price: number): void {
    console.log(`Package: ${packageName}, Price: $${price}`);
  }
}
