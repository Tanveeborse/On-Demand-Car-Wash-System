import { Component, Injectable, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PackageplanserviceService } from '../../services/packageplanservice/packageplanservice.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-package-plan',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './package-plan.component.html',
  styleUrls: ['./package-plan.component.css']
})

@Injectable()
export class PackagePlanComponent implements OnInit{
  plans: any[] = [];                                        // Holds the list of plans
  currentPlan: any = null;                                  // Holds the currently selected plan for editing or viewing
  modalVisible: boolean = false;                            // Controls visibility of modal
  actionType: string = '';                                  // Tracks the current action (Add, Edit, Activate, Deactivate)
  packages:any[]=[];
 // packageId:number|null=null;

  constructor(private packageService: PackageplanserviceService, private router: Router) {}

  goBack() { this.router.navigate(['/admin-dashboard']); }

   // Fetch all packages from the API
   ngOnInit(): void {
    this.getAllPackages();
  }
   // Open the modal form
   openModal() {
    this.modalVisible = true;
  }

  // Opens modal for adding a new package
  openAddForm(): void {
    this.actionType = 'Add';
    this.currentPlan = { packageName: '', packageDescription: '', price: null, isActive: true };
    this.modalVisible = true;
  }

  // Opens modal for editing a selected package
  openEditForm(packageId: number, plan: any): void {
    this.actionType = 'Edit';
    this.currentPlan = { ...plan };                                  // Clone the plan to avoid direct mutation
    this.modalVisible = true;
  }

  // Opens modal for activating or deactivating a selected package
  openActivateDeactivateForm(plan: any): void {
    this.actionType = plan.isActive ? 'Deactivate' : 'Activate';
    this.currentPlan = { ...plan };
    this.modalVisible = true;
  }

  // Get the list of all packages from the backend using the service
  getAllPackages(): void {
    this.packageService.getAllPackages().subscribe(
      (data) => {
        this.packages = data;
      },
      (error) => {
        console.error('Error fetching packages', error);
      }
    );
  }

  // Close the modal
  closeModal(): void {
    this.modalVisible = false;
    this.resetForm();
  }

  // Reset the form fields
  resetForm(): void {
    this.currentPlan = { packageName: '', packageDescription: '', price: null, isActive: true };
  }

  // Handle form submission (Add/Edit/Activate/Deactivate)
  submitForm(): void {
    if (this.actionType === 'Add') {
      this.addPackage();
    } else if (this.actionType === 'Edit') {
      this.editPackage(this.currentPlan.packageId, this.currentPlan);
    } 
  }

  // Add new package
  addPackage(): void {
    console.log("Data from component:"+this.currentPlan);
    this.packageService.addPackage(this.currentPlan).subscribe(
      (response) => {
        console.log('Package added successfully');
       // this.getAllPackages(); // Refresh the list of packages
        this.closeModal();
      },
      (error) => {
        console.error('Error adding package', error);
        if (error.error) {
          console.error('Backend error:', error.error); // Log the backend error message
        } else {
          console.error('Unknown error:', error);
        }
      }
    );
  }

   // Edit an existing package
  editPackage(packageId: number, packageData: any): void {
    console.log('Data being sent to backend:', packageData); // Log the package data
    this.packageService.editPackage(packageId,packageData).subscribe(
      (response) => {
        console.log('Package updated successfully:', response);
        console.log('Package updated successfully');
        this.closeModal();
      },
      (error) => {
        console.error('Error updating package', error);
      }
    );
  }

}
