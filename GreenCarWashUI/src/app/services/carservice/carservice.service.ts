import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CarserviceService {
  private apiUrl = 'https://localhost:7004/api/Car';

  constructor(private http: HttpClient) {}

  // getCarsByUserId(userId: number): Observable<any[]> {
  //   return this.http.get<any[]>(`${this.apiUrl}/${userId}`);
  // }
  // carservice.service.ts
getCarsByUserId(userId: number): Observable<any[]> {
 // console.log("userId: ", userId);
  const token = localStorage.getItem('token');
  const userid = localStorage.getItem('userId');
  console.log("userid: ", userid);
  if (token) {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`, // Add token to the Authorization header
      });
      console.log("url: "+`${this.apiUrl}/user/${userid}`);
  return this.http.get<any[]>(`${this.apiUrl}/user/${userid}`,{headers}).pipe(
    map((response) => {
      console.log("response: "+response);
      // If response is a single object, convert it into an array
      if (!Array.isArray(response)) {
        return [response];  // Wrap the single object into an array
      }
      return response;  // Return the response as-is if it's already an array
    })
  );
}else {
  // Gracefully handle the missing token case by returning an observable with an error
  return new Observable(observer => {
    observer.error('Token is missing or invalid');
  });
}
}


  // addCar(newCar: any): Observable<any> {
  //   const token = localStorage.getItem('token');
    
  //   if (token) {
  //     const headers = new HttpHeaders({
  //         'Authorization': `Bearer ${token}`, // Add token to the Authorization header
  //       });
  //   return this.http.post<any>(`${this.apiUrl}/add`, newCar, {
  //     headers: { 'Content-Type': 'application/json' }
  //   });
  // }else {
  //   // Gracefully handle the missing token case by returning an observable with an error
  //   return new Observable(observer => {
  //     observer.error('Token is missing or invalid');
  //   });
  // }
  // }
  addCar(newCar: any): Observable<any> {

  const token = localStorage.getItem('token');

  console.log('TOKEN:', token);

  if (!token) {
    return new Observable(observer => {
      observer.error('Token missing');
    });
  }

  const headers = new HttpHeaders({
    Authorization: `Bearer ${token}`,
    'Content-Type': 'application/json'
  });

  return this.http.post<any>(
    `${this.apiUrl}/add`,
    newCar,
    { headers }
  );
}
}

