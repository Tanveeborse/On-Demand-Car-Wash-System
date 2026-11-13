import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/authservice.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-signup',
  imports: [CommonModule,FormsModule,RouterModule],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
  fullName: string = '';
  email: string = '';
  password: string = '';
  phoneNumber: string = '';
  address: string = '';
  profilePicture: string = ''; // You can handle profile picture uploads separately
  role: string = ''; 
  errorMessage: string = '';
  successMessage: string = '';
  loading: boolean = false;
  showPassword: boolean = false;
  steps = ['Create your account', 'Choose a wash package', 'Schedule your wash', 'Enjoy a clean car!'];

  constructor(private authService: AuthService, private router: Router) {}

  // Method to handle registration
  onRegister(): void {
    this.loading = true;
    const user = {
      fullName: this.fullName,
      emailId: this.email,
      password: this.password,
      phoneNumber: this.phoneNumber,
      address: this.address,
      profilePicture: this.profilePicture, // Handle file upload as needed
      role: this.role, // You can set this to dynamic role, if needed
    };

    this.authService.register(user).subscribe(
      (response) => {
        this.loading = false;
        this.successMessage = 'Registration successful. You can log in now.';
        // Optionally redirect the user to login page after success
        this.router.navigate(['/home']);
      },
      (error) => {
        this.errorMessage = 'Registration failed. Please try again.';
        this.loading = false;
      }
    );
  }
}


