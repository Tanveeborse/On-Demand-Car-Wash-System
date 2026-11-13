import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PromoCode } from '../../Interface/promocode';

@Injectable({
  providedIn: 'root'
})
export class PromocodeService {

  private apiUrl = 'https://localhost:7004/api/PromoCode';  // Base URL for the Promo Code API

//   constructor(private http: HttpClient) { }

//   getActivePromoCodes(): Observable<PromoCode[]> {
//     const token = localStorage.getItem('token');
//   if (token) {
//     const headers = new HttpHeaders({
//         'Authorization': `Bearer ${token}`, // Add token to the Authorization header
//       });
//     return this.http.get<PromoCode[]>(`${this.apiUrl}/all`,{headers});
//   }else {
//     // Gracefully handle the missing token case by returning an observable with an error
//     return new Observable(observer => {
//       observer.error('Token is missing or invalid');
//     });
//   }
//   }

//   addPromoCode(promoCode: PromoCode): Observable<any> {
//     const token = localStorage.getItem('token');
//     if (token) {
//       const headers = new HttpHeaders({
//           'Authorization': `Bearer ${token}`, // Add token to the Authorization header
//         });
//     return this.http.post(`${this.apiUrl}/add`, promoCode,{headers});
//   }else {
//     // Gracefully handle the missing token case by returning an observable with an error
//     return new Observable(observer => {
//       observer.error('Token is missing or invalid');
//     });
//   }
//   }

//   updatePromoCode(id:number,promoCode: PromoCode): Observable<any> {
//     console.log(promoCode);
//     const token = localStorage.getItem('token');
//     if (token) {
//       const headers = new HttpHeaders({
//           'Authorization': `Bearer ${token}`, // Add token to the Authorization header
//         });
//     return this.http.put(`${this.apiUrl}/update/${id}`, promoCode,{headers});
//   }else {
//     // Gracefully handle the missing token case by returning an observable with an error
//     return new Observable(observer => {
//       observer.error('Token is missing or invalid');
//     });
//   }
//   }

//   activatePromoCode(id: number): Observable<any> {
//     const token = localStorage.getItem('token');
//     if (token) {
//       const headers = new HttpHeaders({
//           'Authorization': `Bearer ${token}`, // Add token to the Authorization header
//         });
//     return this.http.patch(`${this.apiUrl}/activate/${id}`, null,{headers});
//   }else {
//     // Gracefully handle the missing token case by returning an observable with an error
//     return new Observable(observer => {
//       observer.error('Token is missing or invalid');
//     });
//   }
//   }

//   deactivatePromoCode(id: number): Observable<any> {
//     const token = localStorage.getItem('token');
//     if (token) {
//       const headers = new HttpHeaders({
//           'Authorization': `Bearer ${token}`, // Add token to the Authorization header
//         });
//     return this.http.patch(`${this.apiUrl}/deactivate/${id}`,null, {headers});
//   }else {
//     // Gracefully handle the missing token case by returning an observable with an error
//     return new Observable(observer => {
//       observer.error('Token is missing or invalid');
//     });
//   }
//   }
// }

constructor(private http: HttpClient) {}

  getAllPromoCodes(): Observable<any> {
    const token = localStorage.getItem('token');
  if (token) {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`, // Add token to the Authorization header
      });
    return this.http.get<PromoCode[]>(`${this.apiUrl}/all`,{headers});
  }else {
    // Gracefully handle the missing token case by returning an observable with an error
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
  }

  getActivePromoCodes(): Observable<any> {
    const token = localStorage.getItem('token');
  if (token) {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`, // Add token to the Authorization header
      });
    return this.http.get<PromoCode[]>(`${this.apiUrl}/active`,{headers});
  }else {
    // Gracefully handle the missing token case by returning an observable with an error
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
  }

  getPromoCodeById(id: number): Observable<PromoCode> {
    const token = localStorage.getItem('token');
  if (token) {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`, // Add token to the Authorization header
      });
    return this.http.get<PromoCode>(`${this.apiUrl}/${id}`,{headers});
  }else {
    // Gracefully handle the missing token case by returning an observable with an error
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
  }

  addPromoCode(promoCode: any): Observable<any> {
    const token = localStorage.getItem('token');
    if (token) {
      const headers = new HttpHeaders({
          'Authorization': `Bearer ${token}`, // Add token to the Authorization header
        });
    return this.http.post(`${this.apiUrl}/add`,promoCode,{headers});
  }else {
    // Gracefully handle the missing token case by returning an observable with an error
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
  }

  updatePromoCode(promoCode: any): Observable<any> {
    const token = localStorage.getItem('token');
    if (token) {
      const headers = new HttpHeaders({
          'Authorization': `Bearer ${token}`, // Add token to the Authorization header
        });
    return this.http.put(`${this.apiUrl}/update`,promoCode,{headers});
  }else {
    // Gracefully handle the missing token case by returning an observable with an error
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
  }

  deletePromoCode(id: number): Observable<any> {
    const token = localStorage.getItem('token');
    if (token) {
      const headers = new HttpHeaders({
          'Authorization': `Bearer ${token}`, // Add token to the Authorization header
        });
    return this.http.delete(`${this.apiUrl}/${id}`);
  }else {
    // Gracefully handle the missing token case by returning an observable with an error
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
  }

  activatePromoCode(id: number): Observable<any> {
    const token = localStorage.getItem('token');
    if (token) {
      const headers = new HttpHeaders({
          'Authorization': `Bearer ${token}`, // Add token to the Authorization header
        });
    return this.http.patch(`${this.apiUrl}/activate/${id}`, null,{headers});
  }else {
    // Gracefully handle the missing token case by returning an observable with an error
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
  }

  deactivatePromoCode(id: number): Observable<any> {
    const token = localStorage.getItem('token');
    if (token) {
      const headers = new HttpHeaders({
          'Authorization': `Bearer ${token}`, // Add token to the Authorization header
        });
    return this.http.patch(`${this.apiUrl}/deactivate/${id}`,null, {headers});
  }else {
    // Gracefully handle the missing token case by returning an observable with an error
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
  }
}