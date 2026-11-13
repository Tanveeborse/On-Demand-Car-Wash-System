import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { Router } from '@angular/router';
import { catchError, map } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7004/api/Auth';
  private currentUserSubject: BehaviorSubject<any>;
  public currentUser: Observable<any>;
  constructor(private http: HttpClient, private router: Router) {
    const currentUserData = JSON.parse(localStorage.getItem('customer') || '{}');
    this.currentUserSubject = new BehaviorSubject<any>(currentUserData);
    this.currentUser = this.currentUserSubject.asObservable();
  }
  // Method for registering a user
  register(user: any): Observable<any> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.http.post<any>(`${this.apiUrl}/register`, user, { headers }).pipe(
      map((response) => {
        // Handle success if needed
        return response;
      }),
      catchError((error) => {
        // Handle errors appropriately, returning an observable with an error message
        console.error('Registration error:', error);
        return of({ error: 'Registration failed. Please try again.' });  // Return an observable with the error message
      })
    );
  }
  login(email: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, { email, password }).pipe(
      map((response) => {
        console.log('Login Response:'+ response); // Add this line to log the response
        if (response && response.token) {
          // Save the JWT token to localStorage
          localStorage.setItem('token', response.token);
          localStorage.setItem('userId', response.userId);
          localStorage.setItem('role', response.role);
          console.log('Token saved to localStorage:', response.role);
          //.setItem('j', response.token);
          this.currentUserSubject.next(response);
          // this.router.navigate(['/home']);
        }
        return response;
      }),
      catchError((error) => {
        throw new Error('Login failed: ' + error.message);
      })
    );
  }
  // Method to check if the user is authenticated
  isAuthenticated(): boolean {
    const user = this.currentUserSubject.value;
    return !!user?.token;
  }
  // Method to log out a user
  logout(): void {
    localStorage.removeItem('user');
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }
}