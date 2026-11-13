import { Component, OnInit } from '@angular/core';
import { WashersService } from '../../services/washers/washers.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-washers',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './washers.component.html',
  styleUrl: './washers.component.css'
})
export class WashersComponent implements OnInit {
  washers: any[] = [];
  selectedWasher: any = null;
  formData = {
    fullname: '',
    emailId: '',
    phoneNumber: '',
    password: '',
    address: '',
    profilePicture: null,
    role: 'Washer'
  };
  modalVisible = false;

  constructor(private washerService: WashersService, private router: Router) {}

  goBack() { this.router.navigate(['/admin-dashboard']); }

  ngOnInit(): void {
    const role = 'Washer'; // Get the role from the localStorage
    this.getWashers(role);  // Fetch all washers based on the role
  }

   // Get the list of all washers based on the role
   getWashers(role: string) {
    this.washerService.getAllWashers(role).subscribe({
      next: (data) => {
        this.washers = data;
      },
      error: (err) => {
        console.error('Error fetching washers:', err);
      },
    });
  }

  // Open the add washer form
  openAddUserForm() {
    this.selectedWasher = null;  // Reset selected washer for adding a new one
    this.formData = {
      fullname: '',
      emailId: '',
      password: '',
      phoneNumber: '',
      address: '',
      profilePicture: null,
      role: 'Washer'
    };
    this.modalVisible = true;  // Open the modal

  }

  // Open the update washer form
  openUpdateUserForm(userId: number) {
    this.washerService.getWasherById(userId).subscribe({
      next: (user) => {
        this.selectedWasher = user;
        this.formData = {
          fullname: user.fullname,
          emailId: user.emailId,
          password: user.password,
          phoneNumber: user.phoneNumber,
          address: user.address,
          profilePicture: user.profilePicture,
          role: user.role
        };
        this.modalVisible = true;  // Open the modal
      },
      error: (err) => {
        console.error('Error fetching user for update:', err);
      },
    });
  }

  // Submit the add or update form
  submitForm() {
    if (this.selectedWasher) {
      // Update washer
      this.washerService.updateWasher(this.selectedWasher.userId, this.formData).subscribe({
        next: () => {
        //  this.getWashers(role);  // Refresh list after update
        this.modalVisible = false;  // Close the modal after submission
        },
        error: (err) => {
          console.error('Error updating user:', err);
        }
      });
    } else {
      // Add new washer
      this.washerService.addWasher(this.formData).subscribe({
        next: () => {
          //this.getWashers(this.role);  // Refresh list after add
        this.modalVisible = false;  // Close the modal after submission
        },
        error: (err) => {
          console.error('Error adding user:', err);
        }
      });
    }
  }

  // Close the modal (cancel the form without saving)
  closeModal() {
    this.modalVisible = false;
  }

  // Activate a washer
  activateUser(userId: number) {
    this.washerService.activateWasher(userId).subscribe({
      next: () => {
      //  this.getWashers();
      },
      error: (err) => {
        console.error('Error activating user:', err);
      },
    });
  }

  // Deactivate a washer
  deactivateUser(userId: number) {
    this.washerService.deactivateWasher(userId).subscribe({
      next: () => {
     //   this.getWashers();
      },
      error: (err) => {
        console.error('Error deactivating user:', err);
      },
    });
  }

  // Export the washer report
  exportReport() {
    this.washerService.exportWasherReport().subscribe({
      next: (response) => {
        const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'WasherReport.xlsx';
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        console.error('Error exporting report:', err);
      },
    });
  }
}