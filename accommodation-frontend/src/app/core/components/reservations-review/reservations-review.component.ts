import { Component } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ReservationByGuest } from 'src/app/api/model/reservationByGuest';
import { User } from '../../keycloak/model/user';
import { DatePipe } from '@angular/common';
import { AccommodationService } from 'src/app/api/api/accommodation.service';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../keycloak/auth.service';
import { ConfirmDialogComponent, ConfirmDialogModel } from '../confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';

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

  constructor(public dialog: MatDialog,private accService: AccommodationService, private toastr : ToastrService, private authService: AuthService) {
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

  cancelReservation(res:ReservationByGuest){
    const message = `Are you sure you want to cancel this reservation?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });

    dialogRef.afterClosed().subscribe(dialogResult => {
      if(dialogResult == 'false')
        return
      else if(dialogResult == 'true'){
        let parameter = {accommodationId: res.accommodationId, reservationId: res.id}
        this.accService.cancelReservation(parameter).subscribe({
        next: () => {
          this.showSuccess('Successfully canceled reservation');
          this.reservationList = this.reservationList.filter(item => item.id !== res.id);
          this.dataSourceReservations.data = this.reservationList
        },
        error: (e) => {
          this.showError(e.error);
        }
      });
      } 
    });
  }

  showSuccess(message: string) {
    this.toastr.success(message, 'Booking application');
  }
  showError(message: string) {
    this.toastr.error(message, 'Booking application');
  }
}
