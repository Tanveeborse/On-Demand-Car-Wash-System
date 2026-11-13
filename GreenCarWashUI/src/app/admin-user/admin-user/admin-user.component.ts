import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { User } from '../../Interface/User';
import { AdminUserService } from '../../services/admin-user/admin-user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-user',
  imports: [FormsModule, CommonModule],
  templateUrl: './admin-user.component.html',
  styleUrl: './admin-user.component.css'
})
export class AdminUserComponent implements OnInit {
  users: User[] = [];
  selectedUser: User | null = null;

  constructor(private userService: AdminUserService, private router: Router) {}

  goBack() { this.router.navigate(['/admin-dashboard']); }

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getAllUsers().subscribe((users) => {
      this.users = users;
    });
  }

  openEditPopup(user: User | null) {
    if (user) {
      console.log('Selected User:', user); // Log user to check if id exists
      this.selectedUser = { ...user };  // Open the form with selected user details
      console.log('Selected User after copy:', this.selectedUser); // Check after assigning
    } else {
      this.selectedUser = { userId: null, fullname: '', emailId: '', phoneNumber: '', address: '', isActive: false };
    }
  }
  
  
  saveUser() {
    if (this.selectedUser) {
      console.log('Saving user with ID:', this.selectedUser.userId);  // Log user ID
      if (this.selectedUser.userId !== null && this.selectedUser.userId !== undefined) {
        console.log('UserId: ', this.selectedUser.userId);
        this.userService.updateUser(this.selectedUser.userId, this.selectedUser).subscribe(() => {
          this.loadUsers();  // Reload user list after saving the changes
          this.selectedUser = null;  // Close the popup
        });
      } else {
        console.error("No valid user selected or user id is missing.");
      }
    }
  }
  
  
  closeModal() {
    this.selectedUser = null; // Close the modal by setting selectedUser to null
  }

  activateUser(userId: number) {
    console.log('Activating user with ID:', userId); // Log user ID
    this.userService.activateUser(userId).subscribe(() => {
      this.loadUsers(); // Reload user list after activation
    });
  }

  deactivateUser(userId: number) {
    console.log('Deactivating user with ID:', userId); // Log
    this.userService.deactivateUser(userId).subscribe(() => {
      this.loadUsers(); // Reload user list after deactivation
    });
  }
}