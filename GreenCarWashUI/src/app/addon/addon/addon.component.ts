import { Component, OnInit } from '@angular/core';
import { AddOn } from '../../Interface/addon';
import { AddonService } from '../../services/addonservice/addon.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-addon',
  standalone:true,
  imports: [CommonModule,FormsModule],
  templateUrl: './addon.component.html',
  styleUrl: './addon.component.css'
})
export class AddonComponent implements OnInit {
  addOns: AddOn[] = [];
  modalVisible = false;
  currentAddOn: AddOn = {
    addonId: 0,
    addonName: '',
    addonDescription: '',
    price: 0,
    isActive: true
  };
  actionType = 'Add'; // "Add" or "Update"

  constructor(private addOnService: AddonService, private router: Router) { }

  goBack() { this.router.navigate(['/admin-dashboard']); }

  ngOnInit(): void {
    this.loadAddOns();
  }

  loadAddOns(): void {
    this.addOnService.getAllAddOns().subscribe(
      (response) => {
        this.addOns = response;
      },
      (error) => {
        console.error('Error loading add-ons:', error);
      }
    );
  }

  openAddForm(): void {
    this.currentAddOn = {
      addonId: 0,
      addonName: '',
      addonDescription: '',
      price: 0,
      isActive: true
    };
    this.actionType = 'Add';
    this.modalVisible = true;
  }

  openEditForm(addonId: number, addon: AddOn): void {
    this.currentAddOn = { ...addon };
    this.actionType = 'Update';
    this.modalVisible = true;
  }

  closeModal(): void {
    this.modalVisible = false;
  }

  submitForm(): void {
    if (this.actionType === 'Add') {
      this.addOnService.addAddOn(this.currentAddOn).subscribe(
        (response) => {
          console.log('Add-On added successfully:', response);
          this.loadAddOns();
          this.closeModal();
        },
        (error) => {
          console.error('Error adding add-on:', error);
        }
      );
    } else if (this.actionType === 'Update') {
      this.updateAddOnStatus(this.currentAddOn.addonId, this.currentAddOn);
      this.closeModal();
    }
  }

  updateAddOnStatus(addonId: number, addonData:any): void 
  {
    this.addOnService.updateAddOn(addonId,addonData).subscribe(
      (response) => {
        console.log('Add-On updated successfully:', response);
        this.loadAddOns();
      },
      (error) => {
        console.error('Error updating add-on:', error);
      }
    );
  }
}

