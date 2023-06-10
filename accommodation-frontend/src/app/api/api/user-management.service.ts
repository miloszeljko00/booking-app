import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';


const httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  @Injectable({
    providedIn: 'root'
  })


export class UserManagementService{
    private apiUrl = environment.apiUrl + '/User'

    constructor(private http: HttpClient) { }

    getUser(id: string) {
      return this.http.get(this.apiUrl + '/' + id);
    }
    delete(id: string) {
      return this.http.delete(this.apiUrl + '/' + id);
    }
    register(request: any){
      return this.http.post(this.apiUrl, request);
    }
    update(request: any){
      return this.http.put(this.apiUrl + '/' + request.userId, request);
    }
}
