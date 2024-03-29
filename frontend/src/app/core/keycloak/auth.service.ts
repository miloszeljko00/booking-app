import { Observable, of, Subject } from 'rxjs';
import { KeycloakService } from 'keycloak-angular';
import { Injectable } from '@angular/core';
import jwtDecode from 'jwt-decode';
import { User } from './model/user';

@Injectable({
  providedIn: 'root'
})  
export class AuthService {

  private user$: Subject<User|null> = new Subject();
  private token$: Subject<string> =  new Subject();
  private authenticated$: Subject<boolean> =  new Subject();

  private user: User|null = null;
  private token: string = '';
  private authenticated: boolean = false;

  constructor(private keycloak: KeycloakService) { 
    this.keycloak.isLoggedIn().then((authenticated) => {
      this.authenticated = authenticated;
      if (authenticated) {
        this.keycloak.getToken().then((token) => {
          this.token = token;
          this.token$.next(this.token);
          let decoded: any = jwtDecode(token);
          this.user = decoded.user;   
          this.user$.next(this.user);  
        });
        
        this.authenticated$.next(this.authenticated);
      }
    });
  }

  getUser(): User|null {
    return this.user;
  }
  getUserObservable(): Observable<User|null> {
    return this.user$;
  }
  getToken(): string {
    return this.token;
  }
  getTokenObservable(): Observable<string> {
    return this.token$;
  }
  isAuthenticated(): boolean {
    return this.authenticated;
  }
  isAuthenticatedObservable(): Observable<boolean>{
    return this.authenticated$;
  }

  login() {
    this.keycloak.login();
  }
  logout() {
    this.keycloak.logout();
  }
}
