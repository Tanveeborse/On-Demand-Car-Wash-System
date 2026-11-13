import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {
  private apiUrl = 'https://localhost:7004/api/Reviews'; // Replace with your actual backend API URL

  constructor(private http: HttpClient) { }

   // Method to fetch WasherId by OrderId
   getWasherId(orderId: number): Observable<number | null> {
    const token = localStorage.getItem('token');
    if (token) {
      const headers = new HttpHeaders({
          'Authorization': `Bearer ${token}`, // Add token to the Authorization header
        });
    return this.http.get<number | null>(`${this.apiUrl}/GetWasherId/${orderId}`);
  }else {
    // Gracefully handle the missing token case by returning an observable with an error
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
}

  // Method to submit the review data
  submitReview(reviewData: { rating: number, comment: string }): Observable<any> {
    const token = localStorage.getItem('token');
    console.log("review data"+reviewData.rating);
    console.log("rson data"+reviewData.comment);
  if (token) {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json' // Add token to the Authorization header
      });
      console.log("Authorization",headers.get('Authorization'));
    return this.http.post<any>(`${this.apiUrl}/add`, reviewData,{headers});
  }else {
    // Gracefully handle the missing token case by returning an observable with an error
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
}
}
