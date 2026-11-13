import { Component } from '@angular/core';
import { AuthService } from '../../services/authservice.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  errorMessage: string = '';
  loading: boolean = false;
  showPassword: boolean = false;

  brandFeatures = [
    { icon: 'bi-calendar-check', text: 'Easy Online Booking' },
    { icon: 'bi-shield-check', text: 'Secure Payments' },
    { icon: 'bi-leaf', text: 'Eco-Friendly Products' },
    { icon: 'bi-star', text: 'Verified Professionals' },
  ];

  constructor(private authService: AuthService, private router: Router) {}

  // Method to handle login action
  onLogin(): void {
    this.loading = true;
    this.authService.login(this.email, this.password).subscribe(
      (response) => {
        console.log("Hello :"+response.token);
        this.loading = false;
        if (response.token) {
          // Navigate based on user role
           // Store the token in localStorage
        localStorage.setItem('token', response.token);
          if (response.role === 'Customer') {
            this.router.navigate(['/customer-dashboard']);
          } else if (response.role === 'Washer') {
            this.router.navigate(['/washer-dashboard']);
          } else if (response.role === 'Admin') {
            this.router.navigate(['/admin-dashboard']);
          }
        }
      },
      (error) => {
        this.errorMessage = 'Invalid credentials. Please try again.';
        this.loading = false;
      }
    );
  }
}

