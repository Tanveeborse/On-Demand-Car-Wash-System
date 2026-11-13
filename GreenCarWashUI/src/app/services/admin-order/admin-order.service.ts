import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminOrderService {
  private apiUrl = 'https://localhost:7004/api/Orders'; 

  constructor(private http: HttpClient) {}

  getPendingOrders(): Observable<any[]> {
    const token = localStorage.getItem('token');
    if (token) {
      const headers = new HttpHeaders({
          'Authorization': `Bearer ${token}`, 
        });
    return this.http.get<any[]>(`${this.apiUrl}/pending_all`,{headers});
  }else {
    
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
}

  assignRandomWasher(orderId: number): Observable<any> {
    const token = localStorage.getItem('token');
    if (token) {
      const headers = new HttpHeaders({
          'Authorization': `Bearer ${token}`, // Add token to the Authorization header
        });
    return this.http.post<any>(`${this.apiUrl}/${orderId}/assignrandom`, {headers});
  }else {
    // Gracefully handle the missing token case by returning an observable with an error
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
}

  getOrdersByStatus(status: string): Observable<any[]> {
    const token = localStorage.getItem('token');
    if (token) {
      const headers = new HttpHeaders({
          'Authorization': `Bearer ${token}`, // Add token to the Authorization header
        });
    return this.http.get<any[]>(`${this.apiUrl}/all_status`,{headers});
  }else {
    // Gracefully handle the missing token case by returning an observable with an error
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
}
}

