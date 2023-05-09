import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Accommodation } from '../model/accommodation';
import { Request } from '../model/request';


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

      makeReservation(request: Request): Observable<Accommodation> {
        return this.http.put<Accommodation>(this.apiUrl + "/reservation", request, httpOptions);
      }
}