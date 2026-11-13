import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../../Interface/User';

@Injectable({
  providedIn: 'root'
})
export class AdminUserService {
  private apiUrl = 'https://localhost:7004/api/User';  // Update with your API URL

  
  constructor(private http: HttpClient) {}

  // Get all users
  getAllUsers(): Observable<User[]> {
    const token = localStorage.getItem('token');
    console.log('Token fetched from localStorage:', token);
   // console.log('userId fetched from localStorage:', userId);
  if (token) {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`, // Add token to the Authorization header
      });
    return this.http.get<User[]>(`${this.apiUrl}/role/Customer`,{headers});
  }else {
    // Gracefully handle the missing token case by returning an observable with an error
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
}

  // Get user by ID
  getUserById(userId: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${userId}`);
  }

  // Update user
  updateUser(userId: number, user: User): Observable<User> {
    const token = localStorage.getItem('token');
    console.log('Token fetched from localStorage:', token);
    console.log('userId fetched from localStorage:', userId);
    console.log('Data: ',user);
  if (token) {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`, // Add token to the Authorization header
      });
      console.log('Request URL:', `${this.apiUrl}/update/${userId}`);
    return this.http.put<User>(`${this.apiUrl}/update/${userId}`, user,{headers});
  }else {
    // Gracefully handle the missing token case by returning an observable with an error
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
}

  // Activate user
  activateUser(userId: number): Observable<void> {
    const token = localStorage.getItem('token');
    console.log('Token fetched from localStorage:', token);
   // console.log('userId fetched from localStorage:', userId);
  if (token) {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`, // Add token to the Authorization header
      });
    return this.http.post<void>(`${this.apiUrl}/activate/${userId}`,null, {headers});
  }else {
    // Gracefully handle the missing token case by returning an observable with an error
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
}

  // Deactivate user
  deactivateUser(userId: number): Observable<void> {
    const token = localStorage.getItem('token');
    console.log('Token fetched from localStorage:', token);
   // console.log('userId fetched from localStorage:', userId);
  if (token) {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`, // Add token to the Authorization header
      });
      console.log('Request URL:', `${this.apiUrl}/deactivate/${userId}`);
    return this.http.post<void>(`${this.apiUrl}/deactivate/${userId}`, null,{headers});
  }else {
    // Gracefully handle the missing token case by returning an observable with an error
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
}
}