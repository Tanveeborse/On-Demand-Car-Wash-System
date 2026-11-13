import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { PromocodeService } from '../../services/promocode/promocode.service';
import { PromoCode } from '../../Interface/promocode';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-promocode',
  imports: [FormsModule,CommonModule,RouterModule],
  templateUrl: './promocode.component.html',
  styleUrl: './promocode.component.css'
})
export class PromocodeComponent implements OnInit {
  promoCodes: PromoCode[] = [];
  modalVisible = false;
  selectedPromoCode: PromoCode | null = null;
  formData: PromoCode = {
    code: '',
    description: '',
    discountPercentage: 0,
    expiryDate: '',
    isActive: false
  };

  constructor(private promoCodeService: PromocodeService, private router: Router) { }

  goBack() { this.router.navigate(['/admin-dashboard']); }

  ngOnInit(): void {
    this.getPromoCodes();
  }

  getPromoCodes(): void {
    this.promoCodeService.getAllPromoCodes().subscribe(data => {
      this.promoCodes = data;
    });
  }

  openAddPromoCodeForm(): void {
    this.selectedPromoCode = null;
    this.formData = { code: '', description: '', discountPercentage: 0, expiryDate: '', isActive: false };
    this.modalVisible = true;
  }

  openEditPromoCodeForm(promoCodeId: number): void {
    this.promoCodeService.getPromoCodeById(promoCodeId).subscribe(data => {
      this.selectedPromoCode = data;
      this.formData = { ...data };
      this.modalVisible = true;
    });
  }

  closeModal(): void {
    this.modalVisible = false;
  }

  submitForm(): void {
    if (this.selectedPromoCode) {
      this.promoCodeService.updatePromoCode(this.formData).subscribe(() => {
        this.getPromoCodes();
        this.closeModal();
      });
    } else {
      this.promoCodeService.addPromoCode(this.formData).subscribe(() => {
        this.getPromoCodes();
        this.closeModal();
      });
    }
  }

  activatePromoCode(id: number): void {
    this.promoCodeService.activatePromoCode(id).subscribe(() => {
      this.getPromoCodes();
    });
  }

  deactivatePromoCode(id: number): void {
    this.promoCodeService.deactivatePromoCode(id).subscribe(() => {
      this.getPromoCodes();
    });
  }
}