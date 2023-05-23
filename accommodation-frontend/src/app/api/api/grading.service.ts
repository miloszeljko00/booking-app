import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HostGrading } from "../model/hostGrading";
import { CreateHostGrading } from "../model/createHostGrading";
import { UpdateHostGrading } from "../model/updateHostGrading";
import { AccommodationGrading } from "../model/accommodationGrading";
import { CreateAccommodationGrading } from "../model/createAccommodationGrading";
import { UpdateAccommodationGrading } from "../model/updateAccommodationGrading";

const httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  @Injectable({
    providedIn: 'root'
  })


export class GradingService{
  private apiUrl = 'https://localhost:7274/Grading'

  constructor(private http: HttpClient) { }

  getHostGrading(): Observable<HostGrading[]>{
      return this.http.get<HostGrading[]>(this.apiUrl + '/host');
  }

  createHostGrading(createHostGrading: CreateHostGrading): Observable<any>{
    return this.http.post<any>(this.apiUrl + '/host', createHostGrading, httpOptions);
  }

  updateHostGrading(updateHostGrading: UpdateHostGrading): Observable<any>{
    return this.http.put<any>(this.apiUrl + '/host', updateHostGrading, httpOptions);
  }

  deleteHostGrading(gradeId: string): Observable<any>{
    return this.http.delete<any>(this.apiUrl + '/host/' + gradeId);
  }

  getAccommodationGrading(): Observable<AccommodationGrading[]>{
    return this.http.get<AccommodationGrading[]>(this.apiUrl + '/accommodation');
  }

  createAccommodationGrading(createAccommodationGrading: CreateAccommodationGrading): Observable<any>{
    return this.http.post<any>(this.apiUrl + '/accommodation', createAccommodationGrading, httpOptions);
  }

  updateAccommodationGrading(updateAccommodationGrading: UpdateAccommodationGrading): Observable<any>{
    return this.http.put<any>(this.apiUrl + '/accommodation', updateAccommodationGrading, httpOptions);
  }

  deleteAccommodationGrading(gradeId: string): Observable<any>{
    return this.http.delete<any>(this.apiUrl + '/accommodation/' + gradeId);
  }

}