import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
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
import { environment } from 'src/environments/environment';
import { AccommodationMain } from '../model/accommodationMain';


const httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  @Injectable({
    providedIn: 'root'
  })


export class AccommodationService{
    private apiUrl = environment.apiUrl + '/Accommodation'
    private reccommendationUrl = environment.apiUrl + '/Suggestion'

    constructor(private http: HttpClient) { }

    getAll(){
        return this.http.get(this.apiUrl);
    }

    makeReservation(request: Request): Observable<Accommodation> {
      return this.http.put<Accommodation>(this.apiUrl + "/reservation", request, httpOptions);

    }

    getHostsByGuestReservation(email: string): Observable<string[]> {
      return this.http.get<string[]>(this.apiUrl + "/" + email + "/hosts");
    }

    getAccommodationByGuestReservation(email: string): Observable<AccommodationMain[]> {
      return this.http.get<AccommodationMain[]>(this.apiUrl + "/" + email + "/accommodation");
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

    manageReservationRequest(reservationRequest: RequestManagement): Observable<Accommodation>{
      return this.http.put<Accommodation>(this.apiUrl + "/manage-request", reservationRequest, httpOptions);
    }

    searchAccommodation(address: string, numberOfGuests: number, startDate: string, endDate: string){
      const params = new HttpParams()
      .set('address', address)
      .set('numberOfGuests', numberOfGuests)
      .set('startDate', startDate)
      .set('endDate', endDate);
      return this.http.get(this.apiUrl + "/search", {params});
    }

    getAllAccommodationByAdmin(adminEmail: string){
      return this.http.get(this.apiUrl + "/" + adminEmail + "/admin-accommodation" );
    }

    checkHighlightedHost(adminEmail: string): Observable<boolean>{
      return this.http.get<boolean>(this.apiUrl + "/" + adminEmail + "/highlighted-host" );
    }

    AddPrice(price: Price){
      return this.http.post<Price>(this.apiUrl+"/add-price", price, httpOptions);
    }
filterAccommodation(minPrice: number, maxPrice: number, benefits: number[], isHost: boolean, date:string)
    {
      let params = new HttpParams();
      params = params.set('maxPrice', maxPrice.toString());
      params = params.set('minPrice', minPrice.toString());
      params = params.set('isHighlighted', isHost.toString());
      params = params.set('date', date);
      for (const benefit of benefits) {
        params = params.append('benefits', benefit);
      }

      return this.http.get(this.apiUrl + "/filter", {params});

    }
    getRecommendedAccommodation(guestEmail: string){
      return this.http.get(this.reccommendationUrl + "/recommend-accomodation/" + guestEmail );
    }

  
}
