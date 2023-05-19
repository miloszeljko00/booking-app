import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GuestNotification } from '../model/guestNotification';
import { Observable } from 'rxjs';
import { HostNotification } from '../model/hostNotification';
import { CreateGuestNotification } from '../model/createGuestNotification';
import { CreateHostNotification } from '../model/createHostNotification';


const httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  @Injectable({
    providedIn: 'root'
  })


export class NotificationService{
    private apiUrl = 'https://localhost:7136/Notification'

    constructor(private http: HttpClient) { }

    getGuestNotifications(email: string): Observable<GuestNotification>{
        return this.http.get<GuestNotification>(this.apiUrl + '/' + email + '/guest-notifications');
    }

    getHostNotifications(email: string): Observable<HostNotification> {
        return this.http.get<HostNotification>(this.apiUrl + '/' + email + '/host-notifications');
    }
    
    setGuestNotifications(guestNotification: CreateGuestNotification){
      return this.http.put(this.apiUrl + '/guest-notifications', guestNotification, httpOptions);
    }

    setHostNotifications(hostNotification: CreateHostNotification) {
        return this.http.put(this.apiUrl + '/host-notifications', hostNotification, httpOptions);
    }

}