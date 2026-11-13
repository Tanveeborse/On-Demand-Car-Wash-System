import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CustomerOrderaddonService {

  private apiUrl = 'https://localhost:7004/api/OrderAddOns'; // Update with your actual backend URL

  constructor(private http: HttpClient) {}


  // Get AddOn Names by Order ID
  getAddOnsByOrderId(orderId: number): Observable<string[]> {
    const token = localStorage.getItem('token');
    if (token) {
      const headers = new HttpHeaders({
          'Authorization': `Bearer ${token}`, // Add token to the Authorization header
        });
    console.log('Authorization:', headers.get('Authorization')); // Debug log
    console.log('Requesting add-ons for order ID:', headers); // Debug log
    return this.http.get<string[]>(`${this.apiUrl}/order/${orderId}`, { headers }).pipe(
      catchError(error => {
        console.error('Error occurred while fetching add-ons:', error);
        return throwError(error); // Rethrow the error for further handling if necessary
      })
    );

  }
  else {
    // Handle missing token gracefully by returning an error observable
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }

}

createOrderAddOn(orderId: number, addonId: number): Observable<any> {
  console.log('createOrderAddOn called with orderId:', orderId, 'addonId:', addonId);
  const body = {
    orderId: orderId,
    addonId: addonId
  };
  const token = localStorage.getItem('token');
  if (token) {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`, // Add token to the Authorization header
      });
    return this.http.post<any>(`${this.apiUrl}/add`, body, {headers}).pipe(
      catchError(error => {
        console.error('Error occurred while fetching add-ons:', error);
        return throwError(error); // Rethrow the error for further handling if necessary
      })
    );
  } else {
    // Handle missing token gracefully by returning an error observable
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
}
}
