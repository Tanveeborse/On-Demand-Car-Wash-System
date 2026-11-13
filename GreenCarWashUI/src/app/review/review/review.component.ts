import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ReviewService } from '../../services/review/review.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-review',
  imports: [FormsModule,CommonModule],
  templateUrl: './review.component.html',
  styleUrl: './review.component.css'
})
export class ReviewComponent {

  maxStars: number = 5;
  rating: number = 0;
  selectedReason: string = '';
  oid: number | null = null;
  isReviewSubmitted: boolean = false;
  washerId: number | null = null;

  options = [
    { value: 'Excellent',  label: 'Excellent service, very professional', icon: 'bi-emoji-laughing' },
    { value: 'Very Good',  label: 'Friendly and arrived on time',          icon: 'bi-emoji-smile' },
    { value: 'Good',       label: 'Average service, could be better',      icon: 'bi-emoji-neutral' },
    { value: 'Average',    label: 'Not satisfied with the wash quality',   icon: 'bi-emoji-frown' },
    { value: 'Poor',       label: 'Other comments',                        icon: 'bi-chat-left-text' },
  ];

  constructor(private reviewService: ReviewService,private router:Router) { }

  ngOnInit(): void {
   
  }

  setRating(star: number): void {
    this.rating = star;
  }

  getStarClass(index: number): string {
    return index < this.rating ? 'active' : '';
  }
  
  getWasherId(): void {
    let value = localStorage.getItem('orderId');
  if(value){
    this.oid = Number(value);
  
 
    this.reviewService.getWasherId(this.oid).subscribe({
      next: (washerId: number | null) => {
        this.washerId = washerId;
        console.log('WasherId retrieved:', this.washerId);
      },
      error: (error: any) => {
        console.error('Error fetching WasherId:', error);
      }
    });
  }
  }
 
  submitReview(): void {
    if (this.rating === 0 && !this.selectedReason) {
      alert('Please provide a rating and select a reason for your review.');
      return;
    }
    const userId=localStorage.getItem('userId');

  
   if (this.oid !== null) {
    this.getWasherId(); 
  }

     
    console.log("Retrieved orderId:", this.oid);
    const reviewData = {
      orderId: this.oid,
      userId: userId,
      washerId: this.washerId,
      rating: this.rating,
      comment: this.selectedReason
    };

    console.log("reason",this.selectedReason);

    
    this.reviewService.submitReview(reviewData).subscribe({
     
      next: (response) => {
        alert('Thank you for your feedback!');
        console.log('Review submitted successfully', response);
       
        this.isReviewSubmitted = true;
       
        this.rating = 0;
        this.selectedReason = '';
      },
      error: (error) => {
        alert('Error submitting the review. Please try again.');
        console.error('Error submitting review:', error);
      }
    });
  }

goToCustomerDashboard(): void {
 
  this.router.navigate(['/customer-dashboard']);
}
}
