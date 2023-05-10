import { Component } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ReservationByGuest } from 'src/app/api/model/reservationByGuest';
import { User } from '../../keycloak/model/user';
import { DatePipe } from '@angular/common';
import { AccommodationService } from 'src/app/api/api/accommodation.service';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../keycloak/auth.service';

@Component({
  selector: 'app-reservations-review',
  templateUrl: './reservations-review.component.html',
  styleUrls: ['./reservations-review.component.scss']
})
export class ReservationsReviewComponent {
  dataSourceReservations = new MatTableDataSource<ReservationByGuest>();
  displayedColumnsReservations = ['name', 'start', 'end', 'price', 'numberOfGuests', 'cancel'];
                            
  reservationList!: ReservationByGuest[];
  res!: ReservationByGuest;
  user!: User | null;

  constructor(private datepipe: DatePipe,private accService: AccommodationService, private toastr : ToastrService, private authService: AuthService) {
    this.user = this.authService.getUser()
  }

  ngOnInit() {
    this.accService.getReservationsByGuest(this.user?.email ?? '').subscribe((response: any) => {
      this.reservationList = response;
      this.dataSourceReservations.data = this.reservationList
    })
  }

  dayBefore(date: string){
    let dayBefore = new Date(date)
    dayBefore.setDate(dayBefore.getDate() - 1)
    dayBefore.setHours(0)
    if(new Date() >= dayBefore)
      return true
    return false
  }
}
