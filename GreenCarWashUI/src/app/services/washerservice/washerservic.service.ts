import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WasherservicService {
  private apiUrl = 'https://localhost:7004/api';
  
  constructor(private http: HttpClient) {}
  // Add methods for viewOrders and orderHistory
  getPendingOrders(): Observable<any> {
    let token = localStorage.getItem('token');
    if(token){
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      });
      return this.http.get<any>(`${this.apiUrl}/Orders/pending_all`, {headers});
    }
    else{
      throw new Error('Token not found in localStorage');
    }
  }

  getViewOrders(userId:number): Observable<any> {
    let token = localStorage.getItem('token');
    console.log('token in washerService getViewHistory: ', token);
    console.log('userId in washerService getViewHistory: ', userId);
    if(token){
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      });
      return this.http.get<any>(`${this.apiUrl}/Orders/washer_current/${userId}`, {headers});
    }
    else{
      throw new Error('Token not found in localStorage');
    }
  }

  getOrderHistory(userId:number): Observable<any> {
    const token = localStorage.getItem('token');
    console.log('token in washerService getOrderHistory: ', token);
    console.log('userId in washerService getOrderHistory: ', userId);
    if(token){
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`,
      });
      console.log("Authorization",headers.get('Authorization'));
      return this.http.get<any>(`${this.apiUrl}/Orders/washer_past/${userId}`, {headers});
    }
    else{
      throw new Error('Token not found in localStorage');
    }
  }
  // LeaderBoard of washers-
  getLeaderboard(): Observable<any> {
    const token = localStorage.getItem('token');
    console.log('token in washerService getOrderHistory: ', token);
    if(token){
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`,
      });
      console.log("Authorization",headers.get('Authorization'));
      return this.http.get<any>(`https://localhost:7004/api/Leaderboards/top3`, {headers});
    }
    else{
      throw new Error('Token not found in localStorage');
    }
  }
  // when accept, start, completed buttons are clicked
  acceptOrder(orderId: number, userId: number) : Observable<any> {
    let token = localStorage.getItem('token');
    if(token){
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`,
      });
      console.log("Authorization",headers.get('Authorization'));
      return this.http.put(`${this.apiUrl}/Orders/${orderId}/assign/${userId}`, {}, {headers});
    }
    else{
      throw new Error('Token not found in localStorage');
    }
  }
  updateStartTime(orderId: number): Observable<any>{
    let token = localStorage.getItem('token');
    if(token){
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`,
      });
      console.log("Authorization",headers.get('Authorization'));
      return this.http.patch(`${this.apiUrl}/Orders/${orderId}/startWash`, {}, {headers});
    }
    else{
      throw new Error('Token not found in localStorage');
    }
  }
  updateEndTime(orderId: number): Observable<any> {
    let token = localStorage.getItem('token');
    console.log('In service. OrderId: ', orderId);
    if(token){
      console.log('In if. OrderId: ', orderId)
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`,
      });
      console.log("Authorization",headers.get('Authorization'));
      return this.http.patch(`${this.apiUrl}/Orders/${orderId}/endWash`, {}, { headers });
    }
    else{
      throw new Error('Token not found in localStorage');
    }
  }
  updateOrderStatus(orderId: number, oStatus: string): Observable<any> {
    let token = localStorage.getItem('token');
    if(token){
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`,
      });
      console.log("Authorization",headers.get('Authorization'));
      return this.http.put(`${this.apiUrl}/Orders/${orderId}/update/${oStatus}`, {}, {headers});
    }
    else{
      throw new Error('Token not found in localStorage');
    }
  }
}

