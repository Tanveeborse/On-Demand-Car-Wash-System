import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReceiptService {

  private apiUrl = 'https://localhost:7004/api';

  constructor(private http:HttpClient) { }

  getReceipt(orderId:  number): Observable<any> {
    const token = localStorage.getItem('token');
    console.log('token in getReceipt: ', token);
    if(token){
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`,
      });
      console.log("Authorization",headers.get('Authorization'));
      return this.http.get<any>(`${this.apiUrl}/OrderAddOns/${orderId}/generate`, {headers});
    }
    else{
      throw new Error('Token not found in localStorage');
    }
  }
}
