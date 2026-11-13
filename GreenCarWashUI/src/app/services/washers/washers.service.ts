import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WashersService {

  private apiUrl = 'https://localhost:7004/api/User';

  constructor(private http: HttpClient) { }

  // Get all washers based on the role
  getAllWashers(role:string): Observable<any[]> {
    const token = localStorage.getItem('token');
    //const washerrole=localStorage.getItem('role');
    console.log('Token fetched from localStorage:', token);
    console.log('role fetched from localStorage:', role);
  if (token && role) {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`, // Add token to the Authorization header
      });
    return this.http.get<any[]>(`${this.apiUrl}/role/${role}`,{headers});
  }else {
    // Gracefully handle the missing token case by returning an observable with an error
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
}

  // Get a single washer by ID
  getWasherById(userId: number): Observable<any> {
    const token = localStorage.getItem('token');
   
    console.log('Token fetched from localStorage:', token);
    console.log('userId fetched from localStorage:', userId);
  if (token) {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`, // Add token to the Authorization header
      });

    return this.http.get<any>(`${this.apiUrl}/${userId}`,{headers});
  }else {
    // Gracefully handle the missing token case by returning an observable with an error
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
}

  // Add a new washer
  addWasher(washerData: any): Observable<any> {
    const token = localStorage.getItem('token');
    //const washerrole=localStorage.getItem('role');
    console.log('Token fetched from localStorage:', token);
    // console.log('role fetched from localStorage:', role);
  if (token) {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`, // Add token to the Authorization header
      });
      console.log('Data: ',washerData);
    return this.http.post(`${this.apiUrl}/add`, washerData,{headers});
  }else {
    // Gracefully handle the missing token case by returning an observable with an error
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
}

  // Update an existing washer
  updateWasher(userId: number, washerData: any): Observable<any> {
    const token = localStorage.getItem('token');
   // const userid = localStorage.getItem('userId');
    console.log('Token fetched from localStorage:', token);
  if (token) {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`, // Add token to the Authorization header
      });
      console.log('Data: ',washerData);
    return this.http.put(`${this.apiUrl}/update/${userId}`, washerData,{headers});
  }else {
    // Gracefully handle the missing token case by returning an observable with an error
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
}

  // Activate a washer
  activateWasher(userId: number): Observable<any> {
    const token = localStorage.getItem('token');
     console.log('Token fetched from localStorage:', token);
     console.log('userId fetched from localStorage:', userId);
   if (token) {
     const headers = new HttpHeaders({
         'Authorization': `Bearer ${token}`, // Add token to the Authorization header
       });
       console.log('Authorization', headers.get('Authorization'));
       console.log('url', `${this.apiUrl}/activate/${userId}`);
       console.log('headers', headers);
    return this.http.post(`${this.apiUrl}/activate/${userId}`, null, {headers});
  }else {
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
}

  // Deactivate a washer
  deactivateWasher(userId: number): Observable<any> {
    const token = localStorage.getItem('token');
    console.log('Token fetched from localStorage:', token);
  if (token) {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`,
      });
    return this.http.post(`${this.apiUrl}/deactivate/${userId}`,null, {headers});
  }else {
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
}

  // Export washer report (Excel)
  exportWasherReport(): Observable<Blob> {
    const token = localStorage.getItem('token');
    console.log('Token fetched from localStorage:', token);
  if (token) {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`,
      });
    // Pass headers along with responseType 'blob' to handle the binary data (Excel file)
    return this.http.get(`${this.apiUrl}/exportWasherReport`, { 
      headers, 
      responseType: 'blob' // Specify that the response should be a Blob (binary data)
    });
  }else {
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
}
}
