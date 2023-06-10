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
  goToRegistrationPage() {
    this.router.navigate(['registration'])
  }
  goToHomePage(): void {
    this.router.navigate(['']);
  }
  goToAccountPage(): void {
    this.router.navigate(['account']);
  }
  
  goToSearchAccommodations():void{
    this.router.navigate(['search-accommodations']);
  }

  goToUserFlights():void{
    this.router.navigate(['user-flights']);
  }

  goToReservationsReview():void{
    this.router.navigate(['reservations-review']);
  }

  goToRequestsReview():void{
    this.router.navigate(['requests-review']);
  }

  goToAdminRequestsReview():void{
    this.router.navigate(['admin-requests-review']);
  }

  goToAddAccomodation():void{
    this.router.navigate(['add-accommodation'])
  }

  goToAdminAccommodation():void{
    this.router.navigate(['admin-accommodation'])
  }

  goToNotification():void{
    this.router.navigate(['notification'])
  }
  
  goToGrading():void{
    this.router.navigate(['grading'])
  }

}
