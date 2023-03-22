import { KeycloakService } from 'keycloak-angular';
import { Component } from '@angular/core';
import { FlightService } from './api';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'booking-app';

  authenticated = false;
  isUser = false;
  isAdmin = false;

  constructor(private readonly keycloak: KeycloakService) {
    this.keycloak.isLoggedIn().then((authenticated) => {
      this.authenticated = authenticated;
      if (authenticated) {
        const roles = this.keycloak.getUserRoles();
        this.isUser = roles.includes('user');
        this.isAdmin = roles.includes('admin');
      }
    });
  }

  login(){
    this.keycloak.login();
  }

  logout(){
    this.keycloak.logout();
  }
}
