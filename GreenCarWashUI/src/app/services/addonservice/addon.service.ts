import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddOn } from '../../Interface/addon';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AddonService {
  private apiUrl = 'https://localhost:7004/api/AddOn'; 

  constructor(private http: HttpClient) { }

  getAllAddOns(): Observable<AddOn[]> {
    const token = localStorage.getItem('token');
  if (token) {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`, 
      });
    return this.http.get<AddOn[]>(`${this.apiUrl}/active`,{headers});
  }else {
   
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
}

  addAddOn(addOn: AddOn): Observable<AddOn> {
    const token = localStorage.getItem('token');
  if (token) {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`, 
      });
    return this.http.post<AddOn>(`${this.apiUrl}/add`,addOn,{headers} );
  }else {
    
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
}

  updateAddOn(addonId: number, addOn: any): Observable<any> {
   
    addOn.isActive = addOn.isActive === "true"; 

    console.log('Data being sent to backend:', addOn);
    const token = localStorage.getItem('token');
  if (token) {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`, 
      });
    return this.http.put(`${this.apiUrl}/update/${addonId}`, addOn, {headers});
  }else {
   
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
}

  deleteAddOn(addonId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${addonId}`);
  }
}

