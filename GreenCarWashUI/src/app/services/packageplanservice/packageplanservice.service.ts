import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PackagePlanComponent } from '../../packageplan/package-plan/package-plan.component';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PackageplanserviceService {

  private apiUrl = 'https://localhost:7004/api/Package'; 

  constructor(private http: HttpClient) {}

 // Fetch all packages
 getAllPackages(): Observable<any[]> {
  const token = localStorage.getItem('token');
  if (token) {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`, 
      });
  return this.http.get<any[]>(`${this.apiUrl}/active`,{ headers });
} else {
 
  return new Observable(observer => {
    observer.error('Token is missing or invalid');
  });
}
}


  // Add a new package
  addPackage(packageData: any): Observable<any> {
    // Convert Active to true and Inactive to false
  if (packageData.isActive === 'Active') {
    packageData.isActive = true;
  } else if (packageData.isActive === 'Inactive') {
    packageData.isActive = false;
  }
    console.log("data:"+packageData.packageName);
    const token = localStorage.getItem('token'); 
    console.log('Token fetched from localStorage:', token);
    if (token) {
      const headers = new HttpHeaders({
          'Authorization': `Bearer ${token}`, 
        });
        console.log('header:'+headers.get('Authorization'));
      console.log('Request URL:', `${this.apiUrl}}`);
    return this.http.post(`${this.apiUrl}/add`, packageData,{ headers });
  } else {
   
    return new Observable(observer => {
      observer.error('Token is missing or invalid');
    });
  }
}

  // Edit an existing package (used for update)
  editPackage(packageId:number,packageData: any): Observable<any> {

// Convert string to boolean before sending
packageData.isActive = packageData.isActive === "true"; // "true" -> true, otherwise false

    console.log('Data being sent to backend:', packageData); // Log the package data

    const token = localStorage.getItem('token');
    if (token) {
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`, // Add token to the Authorization header
      });
      return this.http.put(`${this.apiUrl}/update/${packageId}`, packageData, { headers });
    } else {
      // Gracefully handle the missing token case by returning an observable with an error
      return new Observable(observer => {
        observer.error('Token is missing or invalid');
      });
    }
  }

  // Activate or Deactivate a package
  togglePackageStatus(packageId: number, action: 'activate' | 'deactivate'): Observable<any> {
    const url = action === 'activate' 
      ? `${this.apiUrl}/activate/${packageId}` 
      : `${this.apiUrl}/deactivate/${packageId}`;
      
    return this.http.post(url, {});
  }
}
