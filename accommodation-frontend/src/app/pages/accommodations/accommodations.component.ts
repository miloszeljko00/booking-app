import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Accommodation } from 'src/app/api/model/accommodation';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { AccommodationService } from 'src/app/api/api/accommodation.service';
import { User } from 'src/app/core/keycloak/model/user';
import { AuthService } from 'src/app/core/keycloak/auth.service';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from 'src/app/core/components/dialog/dialog.component';

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
        if(!this.validTimeFormat(result.startingTime) || !this.validTimeFormat(result.endingTime) || !this.validDate(result.startingDate) || !this.validDate(result.endingDate) ||
        this.dateInPast(result.startingDate) || this.dateInPast(result.endingDate) || this.endDateBeforeStartDate(result.startingDate, result.endingDate)) {
          this.openDialog(accommodation)
        }
        else {
          let startingDateString = this.dateConversion(result.startingDate, result.startingTime)
          let endingDateString = this.dateConversion(result.endingDate, result.endingTime)
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
      }
    });
  }

  dateConversion(date: string, time:string){
    let dateFormat = new Date(date);
    let stringFormat =this.datepipe.transform(dateFormat, 'yyyy-MM-dd');
    stringFormat = stringFormat + 'T' + time + ':00.000Z'
    return stringFormat
  }

  validDate(date: string){
    let dateFormat = new Date(date);
    if(isNaN(dateFormat.getTime())){
      this.showError("Date format is not valid")
      return false
    }
    return true
  }

  endDateBeforeStartDate(start: string, end: string){
    let startDate = new Date(start);
    let endDate = new Date(end);
    if(endDate <= startDate){
      this.showError("End date is before start date")
      return true
    }
    return false
  }

  dateInPast(date: string){
    let dateFormat = new Date(date);
    let today = new Date();
    if(dateFormat <= today){
      this.showError("Date is in the past")
      return true
    }
    return false
  }

  validTimeFormat(time: string){
    if(time.length != 5){
      this.showError("Time is in wrong format")
      return false
    }
    return true
  }

  showSuccess(message: string) {
    this.toastr.success(message, 'Booking application');
  }
  showError(message: string) {
    this.toastr.error(message, 'Booking application');
  }

}
