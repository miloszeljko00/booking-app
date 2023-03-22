import { KeycloakService } from 'keycloak-angular';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private user: any;
  private token: string = '';
  private authenticated: boolean = false;

  constructor(private keycloak: KeycloakService) { 
    this.keycloak.isLoggedIn().then((authenticated) => {
      this.authenticated = authenticated;
      if (authenticated) {
        this.keycloak.getToken().then((token) => {
          this.token = token;
          // TODO: decode token and create user object
        });
      }
    });
  }
}
