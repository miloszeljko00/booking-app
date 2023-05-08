import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';


const httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  @Injectable({
    providedIn: 'root'
  })


export class AccommodationService{
    private apiUrl = 'https://localhost:5000/Accommodation'

    constructor(private http: HttpClient) { }

    getAll(){
        return this.http.get(this.apiUrl);
      }
}