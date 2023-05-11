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

    createAccomodation(accomodationCreate: AccommodationCreate): Observable<AccommodationCreate> {
      return this.http.post<AccommodationCreate>(this.apiUrl, accomodationCreate, httpOptions);

    }

    getReservationsByGuest(email: string): Observable<ReservationByGuest[]> {
      return this.http.get<ReservationByGuest[]>(this.apiUrl + "/"+ email + "/reservations");
    }

    getRequestsByGuest(email: string): Observable<RequestByGuest[]> {
      return this.http.get<RequestByGuest[]>(this.apiUrl + "/"+ email + "/requests");
    }

    getRequestsByAdmin(email: string): Observable<RequestByAdmin[]> {
      return this.http.get<RequestByAdmin[]>(this.apiUrl + "/"+ email + "/admin-requests");
    }

    getBenefits(){
      return this.http.get(this.apiUrl + "/benefits");
    }

    cancelReservation(reservation: ReservationCancellation){
      return this.http.delete(this.apiUrl + "/reservation", { body : reservation, headers: httpOptions.headers});
    }

    cancelReservationRequest(reservationRequest: ReservationCancellation){
      return this.http.delete(this.apiUrl + "/request", { body : reservationRequest, headers: httpOptions.headers});
    }

}