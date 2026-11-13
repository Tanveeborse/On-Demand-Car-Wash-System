import { Injectable ,inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EditprofileserviceService {
  private apiUrl = 'https://localhost:7004/api/User';
  constructor(private http: HttpClient) {}
  getUserById(userId: number): Observable<any> {
    const token = localStorage.getItem('token'); 
    const userIdFromStorage = localStorage.getItem('userId');
    console.log('Token fetched from localStorage:', token);
    console.log('UserId fetched from localStorage:', userIdFromStorage);
    if (token) {
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`,
      });
       console.log('header:'+headers.get('Authorization'));
      return this.http.get(`${this.apiUrl}/${userId}`, { headers });
    } else {
      return new Observable((observer) => {
        observer.error('Token not found. Please log in.');
      });
    }
  }

  updateUserProfile(userId: number, userData: any): Observable<any> {
    const token = localStorage.getItem('token'); 
    const userIdFromStorage = localStorage.getItem('userId');
    console.log('Token fetched from localStorage:', token);
    console.log('UserId fetched from localStorage:', userIdFromStorage);
    if (token) {
      const headers = { 'Authorization': `Bearer ${token}` }
      console.log('Request URL:', `${this.apiUrl}/update/${userId}`);
      console.log("user data:",userData.fullname);
      return this.http.put(`${this.apiUrl}/update/${userId}`,userData, { headers });
    } else {
      return new Observable(observer => {
        observer.error('Token is missing or invalid');
      });
    }
  }
}
