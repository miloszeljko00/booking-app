import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { GetSuggestedFlightsDto } from '../model/getSuggestedFlightsDto';
import { BookFlightDto } from '../model/bookFlightDto';


  @Injectable({
    providedIn: 'root'
  })


export class FlightsService{
    private apiUrl = environment.apiUrl + '/Suggestion'

    constructor(private http: HttpClient) { }

    getSuggestedFlights(body: GetSuggestedFlightsDto): Observable<any>{
        return this.http.post<GetSuggestedFlightsDto>(this.apiUrl + '/get-suggested-flights', body);
    }

    bookFlight(body: BookFlightDto): Observable<any> {
      return this.http.post<BookFlightDto>(this.apiUrl + '/book-flight', body);
    }
}
