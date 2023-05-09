import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatTableModule } from '@angular/material/table';
import { Accommodation } from 'src/app/api/model/accommodation';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { AccommodationService } from 'src/app/api/api/accommodation.service';
import { User } from 'src/app/core/keycloak/model/user';
import { AuthService } from 'src/app/core/keycloak/auth.service';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from 'src/app/core/components/dialog/dialog.component';
import { ReservationRequest } from 'src/app/api/model/reservationRequest';

@Component({
  selector: 'app-accommodations',
  templateUrl: './accommodations.component.html',
  styleUrls: ['./accommodations.component.scss']
})
export class AccommodationsComponent implements OnInit {

  dataSourceAcc = new MatTableDataSource<Accommodation>();
  displayedColumnsFlights = ['name', 'address', 'price', 'priceCalculation' ,'benefits', 'min', 'max', 'reservation'];
                            
  accomodationList!: Accommodation[];
  acc!: Accommodation;
  user!: User | null;
  constructor(private datepipe: DatePipe,public dialog: MatDialog, private accService: AccommodationService, private toastr : ToastrService, private authService: AuthService) {
    this.user = this.authService.getUser()
  }

  ngOnInit() {
    this.accService.getAll().subscribe((response: any) => {
      this.accomodationList = response;
      
      this.dataSourceAcc.data = this.accomodationList
    })
  }

  openDialog(accommodation:Accommodation){
    const dialogRef = this.dialog.open(DialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if(result){
        let startingDate = new Date(result.startingDate);
        let startingDateString =this.datepipe.transform(startingDate, 'yyyy-MM-dd');
        startingDateString = startingDateString + 'T' + result.startingTime + ':00.000Z'
        let endingDate = new Date(result.endingDate);
        let endingDateString =this.datepipe.transform(endingDate, 'yyyy-MM-dd');
        endingDateString = endingDateString + 'T' + result.endingTime + ':00.000Z'
        let request = {accommodationId: accommodation.id, guestEmail: this.user?.email ?? '',
        start: startingDateString, end: endingDateString, numberOfGuests: result.numberOfGuests}
        this.accService.makeReservation(request).subscribe({
          next: (res) => {
            this.showSuccess('Successfully made reservation');
          },
          error: (e) => {
            this.showError(e.error);
            this.openDialog(accommodation)
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
