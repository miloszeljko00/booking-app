import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HostGrading } from "../model/hostGrading";
import { CreateHostGrading } from "../model/createHostGrading";
import { UpdateHostGrading } from "../model/updateHostGrading";

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

}