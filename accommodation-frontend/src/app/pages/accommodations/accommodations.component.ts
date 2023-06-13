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
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { GetSuggestedFlightsDto } from 'src/app/api/model/getSuggestedFlightsDto';
import { SuggestedFlightsDialog } from './dialogs/suggested-flights/suggested-flights.dialog';

@Component({
  selector: 'app-accommodations',
  templateUrl: './accommodations.component.html',
  styleUrls: ['./accommodations.component.scss']
})
export class AccommodationsComponent implements OnInit {

  address: string = "";
  numberOfGuests: number = 1;
  startDate: string = "";
  endDate: string = "";
  formGroup1!: FormGroup;
  previews:{fileName: string, base64: string}[] = [];
  numOfDays: number = 1;
  minPrice: number = 0;
  maxPrice: number = 0;
  isHost: boolean = false;
  lastSearchedDate: string = '';

  benefitList: string[] = [];
  benefits = new FormControl('');
  selectedIndexes: number[] = [];

  dataSourceAcc = new MatTableDataSource<Accommodation>();
  displayedColumnsFlights = ['name', 'address', 'price', 'priceCalculation' , 'totalPrice','benefits', 'min', 'max', 'reservation'];

  accomodationList!: Accommodation[];
  acc!: Accommodation;
  user!: User | null;

  departureLocation: any;
  constructor(private datepipe: DatePipe,
    public dialog: MatDialog,
     private accService: AccommodationService,
      private toastr : ToastrService,
      private authService: AuthService,) {
    this.user = this.authService.getUser()
  }

  ngOnInit() {
    this.accService.getBenefits().subscribe((response: any) => {
      this.benefitList = response;

    })
    this.accService.getAll().subscribe((response: any) => {
      this.accomodationList = response;

      this.dataSourceAcc.data = this.accomodationList
    })
    this.formGroup1 = new FormGroup({
      startDate: new FormControl('', [Validators.required]),
      endDate: new FormControl('', [Validators.required]),
      address: new FormControl('', [Validators.required]),
      numberOfGuests: new FormControl('',[Validators.min(1), Validators.required]),
    });

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

  seeSuggestedFlights(){
    var firstDayPlusOneDay = new Date(this.startDate)
    firstDayPlusOneDay.setDate(firstDayPlusOneDay.getDate() + 1);
    var lastDayPlusOneDay = new Date(this.endDate)
    lastDayPlusOneDay.setDate(lastDayPlusOneDay.getDate() + 1);

    var data : GetSuggestedFlightsDto = {
      firstDayDate: firstDayPlusOneDay,
      lastDayDate: lastDayPlusOneDay,
      placeOfArrival: this.address,
      placeOfDeparture: this.departureLocation
    }
    const dialogRef = this.dialog.open(SuggestedFlightsDialog,
      {
        data: data,
      });
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

  search(){
    if(this.endDateBeforeStartDate(this.startDate, this.endDate) || this.dateInPast(this.startDate) ){

      return
    }

    var sd =  this.datepipe.transform(this.startDate, 'MM/dd/yyyy')??''
    var ed = this.datepipe.transform(this.endDate, 'MM/dd/yyyy')??''
    this.lastSearchedDate = sd;
    this.numOfDays = this.getNumberOfDays(this.startDate, this.endDate)

      this.accService.searchAccommodation(this.address, this.numberOfGuests, sd, ed).subscribe((response: any) => {
          this.accomodationList = response;
          this.dataSourceAcc.data = this.accomodationList;
          this.minPrice = 0;
          this.maxPrice = 0;
          this.isHost = false;
          this.selectedIndexes = []
      })

  }

  showPictures(acc: Accommodation){

      this.previews = acc.pictures;
  }
  getNumberOfDays(startDateStr: string, endDateStr: string): number {
    const startDate = new Date(startDateStr);
    const endDate = new Date(endDateStr);

    if (isNaN(startDate.getTime()) || isNaN(endDate.getTime())) {
      return 0;
    }
    const timeDiff = endDate.getTime() - startDate.getTime();
    const days = Math.floor(timeDiff / (1000 * 60 * 60 * 24));
    return days;
  }

  filter(){
    if(this.lastSearchedDate==='')
    {
      const today = new Date();
      this.lastSearchedDate =  this.datepipe.transform(today, 'MM/dd/yyyy')??''
    }
    this.accService.filterAccommodation(this.minPrice, this.maxPrice, this.selectedIndexes, this.isHost, this.lastSearchedDate).subscribe((response: any) => {
      var filteredList: Accommodation[];
      filteredList = []
      this.accomodationList.forEach(function(acc1: Accommodation){
        response.forEach(function(acc2: Accommodation){
          if(acc1.id === acc2.id){
              filteredList.push(acc1);
          }
        });
      });
      this.dataSourceAcc.data = filteredList;

    })

  }


}
