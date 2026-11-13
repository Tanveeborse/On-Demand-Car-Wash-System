import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {
  private apiUrl = 'https://localhost:7004/api'; // Update with your actual backend URL

  constructor(private http: HttpClient) {}

  getInvoices(orderId:  number): Observable<any> {
    const token = localStorage.getItem('token');
    console.log('token in InvoiceService: ', token);
    if(token){
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`,
      });
      console.log("Authorization",headers.get('Authorization'));
      return this.http.get<any>(`${this.apiUrl}/OrderAddOns/${orderId}/invoice`, {headers});
    }
    else{
      throw new Error('Token not found in localStorage');
    }
  }

  updatePayment(orderId: number): Observable<any> {
    const token = localStorage.getItem('token');
    if (token) {
      const headers = new HttpHeaders({
          'Authorization': `Bearer ${token}`, // Add token to the Authorization header
        });
      return this.http.put<any>(`${this.apiUrl}/Orders/update-payment-status/${orderId}`, {headers});
    } else {
      throw new Error('Token not found in localStorage');
    }
  }
}
