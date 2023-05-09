import { User } from './../../keycloak/model/user';
import { Observable, of } from 'rxjs';
import { AuthService } from './../../keycloak/auth.service';
import { ChangeDetectionStrategy, Component, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NavbarComponent implements OnDestroy{
  user$: Observable<User|null> = of(null);


  constructor(private authService: AuthService, private router: Router) {
    this.user$ = this.authService.getUserObservable();
    this.goToHomePage();
  }
  ngOnDestroy(): void {
  }

  login(): void {
    this.authService.login();
  }

  logout(): void {
    this.authService.logout();
   }

  goToHomePage(): void {
    this.router.navigate(['']);
  }
  goToAccountPage(): void {
    const externalUrl = 'http://localhost:28080/realms/booking-app/account/#/personal-info';
    window.open(externalUrl, '_blank');
  }
  goToCreateFlight(): void {
    this.router.navigate(['create-flight']);
  }

  goToReviewFlights(): void {
    this.router.navigate(['review-flights']);
  }

  goToSearchAccommodations():void{
    this.router.navigate(['search-accommodations']);
  }
  goToUserFlights():void{
    this.router.navigate(['user-flights']);
  }
}
