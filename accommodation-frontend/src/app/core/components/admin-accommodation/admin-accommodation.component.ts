import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { AccommodationService } from 'src/app/api/api/accommodation.service';
import { Accommodation } from 'src/app/api/model/accommodation';
import { Price } from 'src/app/api/model/price';
import { AuthService } from '../../keycloak/auth.service';
import { User } from '../../keycloak/model/user';
import { AddPriceDialogComponent } from '../add-price-dialog/add-price-dialog.component';


@Component({
  selector: 'app-admin-accommodation',
  templateUrl: './admin-accommodation.component.html',
  styleUrls: ['./admin-accommodation.component.scss']
})
export class AdminAccommodationComponent {
 
  dataSourceAcc = new MatTableDataSource<Accommodation>();
  displayedColumnsFlights = ['name', 'address', 'price', 'priceCalculation' ,'benefits', 'min', 'max', 'reservation'];
                            
  accomodationList!: Accommodation[];
  acc!: Accommodation;
  user!: User | null;
  price!: Price;
  constructor(private datepipe: DatePipe,public dialog: MatDialog, private accService: AccommodationService, private toastr : ToastrService, private authService: AuthService) {
    this.user = this.authService.getUser()
  }

  ngOnInit() {
    this.accService.getAllAccommodationByAdmin(this.user?.email??'').subscribe((response: any) => {
      this.accomodationList = response;
      this.dataSourceAcc.data = this.accomodationList
      console.log(response)
    })
    
  }

  

  dateConversion(date: string, time:string){
    let dateFormat = new Date(date);
    let hour = 0
    if(Number(time.split(':')[0]) >= 12){
      hour = Number(time.split(':')[0]) - 2
      time = hour + ':' + time.split(':')[1]
    }
    else if(Number(time.split(':')[0]) < 12 && Number(time.split(':')[0]) >= 2){
      hour = Number(time.split(':')[0]) - 2
      time = '0' + hour + ':' + time.split(':')[1]
    }
    else if(Number(time.split(':')[0]) == 1){
      hour = 23
      time = hour + ':' + time.split(':')[1]
      dateFormat.setDate(dateFormat.getDate() - 1)
    }
    else if(Number(time.split(':')[0]) == 0){
      hour = 22
      time = hour + ':' + time.split(':')[1]
      dateFormat.setDate(dateFormat.getDate() - 1)
    }
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

  openDialog(accommodation:Accommodation){
    const dialogRef = this.dialog.open(AddPriceDialogComponent);
    dialogRef.afterClosed().subscribe(result => {
      if(result){
        if( !this.validDate(result.startingDate) || !this.validDate(result.endingDate) ||
        this.dateInPast(result.startingDate) || this.dateInPast(result.endingDate) || this.endDateBeforeStartDate(result.startingDate, result.endingDate)) {
          this.openDialog(accommodation)
        }
        else {
   
          let startingDateString = this.datepipe.transform(result.startingDate, 'MM/dd/yyyy')??''
          let endingDateString = this.datepipe.transform(result.endingDate, 'MM/dd/yyyy')??''
          this.price = {accommodationId : accommodation.id, startDate: startingDateString, endDate: endingDateString, value: result.price } 
          console.log(this.price)
          this.accService.AddPrice(this.price).subscribe({
            next: (res) => {
              this.showSuccess('Successfully added price');
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

}
