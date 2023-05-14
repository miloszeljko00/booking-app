import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TitleStrategy } from '@angular/router';
import { Observable } from 'rxjs';
import { Accommodation } from '../model/accommodation';
import { Request } from '../model/request';
import { ReservationByGuest } from '../model/reservationByGuest';
import { RequestByGuest } from '../model/requestByGuest';
import { AccommodationCreate } from '../model/accommodationCreate';
import { ReservationCancellation } from '../model/reservationCancellation';
import { RequestByAdmin } from '../model/requestByAdmin';
import { RequestManagement } from '../model/requestManagement';
import { Price } from '../model/price';


const httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  @Injectable({
    providedIn: 'root'
  })


export class UserManagementService{
    private apiUrl = 'https://localhost:7294/User'

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