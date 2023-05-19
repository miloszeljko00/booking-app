import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HostGrading } from "../model/hostGrading";

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

}