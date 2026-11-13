import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrderHistoryService {
  [x: string]: any;
  private apiUrl = 'https://localhost:7004/api/Orders'; // Adjust this to your API endpoint

  constructor(private http: HttpClient) {}
  
   // Fetch orders for a specific user
   getOrdersByUserId(userId: number): Observable<any> {
    const token = localStorage.getItem('token');
    if (token) {
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`,
      });

      return this.http.get<any>(`${this.apiUrl}/orderhistory/${userId}`, { headers });
    } else {
      return new Observable(observer => {
        observer.error('Token is missing or invalid');
      });
    }
  }

    // Method to fetch package name by packageId
    getPackageNameById(packageId: number): Observable<string> {
      const token = localStorage.getItem('token');
  if (token) {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`, // Add token to the Authorization header
      });
      console.log('Requesting package for ID:', packageId); // Debug log
      return this.http.get<string>(`https://localhost:7004/api/Package/${packageId}`,{headers});
    }else {
      // Gracefully handle the missing token case by returning an observable with an error
      return new Observable(observer => {
        observer.error('Token is missing or invalid');
      });
    }
    }

    // Fetch active add-ons
  getActiveAddOns(): Observable<any[]> {
    const token = localStorage.getItem('token');
    if (token) {
      const headers = new HttpHeaders({
          'Authorization': `Bearer ${token}`, // Add token to the Authorization header
        });
    return this.http.get<any[]>(`https://localhost:7004/api/AddOn/active`,{headers});
  }else {
    // Gracefully handle the missing token case by returning an observable with an error
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
  }
  
    // Create a new order
    createOrder(orderData: any): Observable<any> {
      console.log("order data"+orderData);
      const token = localStorage.getItem('token');
      if (token) {
        const headers = new HttpHeaders({
            'Authorization': `Bearer ${token}`, // Add token to the Authorization header
          });
      console.log('Authorization:', headers.get('Authorization')); // Debug log
      console.log('Request URL:', `${this.apiUrl}/add`); // Debug log-
      return this.http.post(`${this.apiUrl}/add`, orderData, { headers });
    }
    else {
      // Handle missing token gracefully by returning an error observable
      return new Observable(observer => {
        observer.error('Token is missing or invalid');
      });
    }
    }
}
