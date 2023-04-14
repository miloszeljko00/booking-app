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

  constructor() {}

}
