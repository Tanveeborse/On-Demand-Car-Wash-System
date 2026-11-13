import { Component ,OnInit} from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { jwtDecode } from 'jwt-decode';
import { EditprofileserviceService } from '../../services/customerservice/editprofileservice.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-profile',
  imports: [RouterModule,ReactiveFormsModule,FormsModule,CommonModule],
  standalone: true,
  templateUrl: './edit-profile.component.html',
  styleUrl: './edit-profile.component.css'
})
export class EditProfileComponent implements OnInit{
  profileForm: FormGroup;
  userId: number | null = null; // Store the userId
  userProfile: any; // Store the fetched user profile
  errorMessage: string = '';
  successMessage: string = ''; 

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private userService: EditprofileserviceService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    // Initialize form group
    this.profileForm = this.fb.group({
      fullname: ['', [Validators.required]],
      phoneNumber: ['', [Validators.required]],
      emailId: ['', [Validators.required, Validators.email]],
      address: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    // Retrieve userId from the route or token (assuming it's in the URL or JWT)
    const userId = +this.route.snapshot.paramMap.get('id')!; // Get userId from the URL
    console.log('User ID:', this.userId);  //
    this.fetchUserProfile(userId); // Fetch the user profile on init
    this.userId = this.getUserIdFromToken(); 
  
  }

  // Extract userId from JWT token stored in localStorage
  getUserIdFromToken(): number | null {
    const customerData = localStorage.getItem('customer');
    console.log("");
    if (customerData) {
      try {
        const parsedCustomer = JSON.parse(customerData);
        const token = parsedCustomer?.token;
        if (token) {
          const decodedToken: any = jwtDecode(token);
          return decodedToken?.nameid || null;
        }
      } catch (e) {
        console.error('Error decoding token:', e);
      }
    }
    return null;
  }

  // Fetch user profile data from the server
  fetchUserProfile(userId: number): void {
    console.log("userId in fetchuserpro"+userId);
    this.userService.getUserById(userId).subscribe(
      (response) => {
        console.log("response: "+response);
        this.userProfile = response; // Store the response (user data)
        console.log("userProfile: "+ this.userProfile);
      },
      (error) => {
        console.error('Error fetching user profile:', error);
        if (error.status === 401) {
          this.errorMessage = 'Unauthorized. Please log in again.';
          // Handle redirect or show an error message to the user
        } else {
          this.errorMessage = 'An error occurred while fetching the user profile.';
        }
      }
    );
  }

  goBack() { this.router.navigate(['/customer-dashboard']); }

  onSubmit(): void {
    if (this.profileForm.valid) {
      const updatedUserData = {
        ...this.profileForm.value,  // Get the data from the form
        userId: this.getUserIdFromToken(),  // Attach the userId to the data, which is dynamically fetched from the JWT
      };
     
      console.log("attached user id:"+this.userId);

      // If userId is null, we should handle this case
      if (this.userId === null) {
        this.errorMessage = 'User ID is missing. Please log in again.';
        return;
      }
      
      // Call the updateUserProfile service method with the userId and updated data
      this.userService.updateUserProfile(this.userId, updatedUserData).subscribe(
        (response) => {

          // Success case: notify the user that their profile was updated
          this.successMessage = 'Profile updated successfully!';
          console.log('Profile updated:', response);
        },
        (error) => {
          // Error case: notify the user that there was an issue
          this.errorMessage = 'Error updating profile.';
          console.error('Error updating profile:', error);
        }
      );
    }
  }
  
  }





