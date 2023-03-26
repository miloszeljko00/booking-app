import { Component } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { FlightService } from 'src/app/api';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { FormGroup } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { Flight } from 'src/app/api';
import { User } from '../../keycloak/model/user';
import { AuthService } from '../../keycloak/auth.service';

@Component({
  selector: 'app-search-flights',
  templateUrl: './search-flights.component.html',
  styleUrls: ['./search-flights.component.scss']
})
export class SearchFlightsComponent {
  dataSourceFlights = new MatTableDataSource<Flight>();
  displayedColumnsFlights = ['departurePlace', 'departureTime', 'arrivalPlace', 'arrivalTime',
                             'totalTickets', 'availableTickets', 'ticketPrice', 'totalPrice', 'book'
                            ];
  flights!: Flight[];
  flight!: Flight;
  formGroup1!: FormGroup;
  placeDeparture: string = '';
  dateDeparture: string = '';
  placeArrival: string = '';
  availableTickets: number = 0;
  user!: User | null;

  constructor(private datepipe: DatePipe, private flightService: FlightService, private toastr : ToastrService, private authService: AuthService) {
      this.user = this.authService.getUser()
   }

  ngOnInit(){
    this.flightService.getFlights().subscribe(res => {
      this.flights = Array.from(res.flights)
      this.flights.forEach(f => {
        f.departure.time = this.datepipe.transform(f.departure.time, 'dd-MM-yyyy HH:mm') ?? '';
        f.arrival.time = this.datepipe.transform(f.arrival.time, 'dd-MM-yyyy HH:mm') ?? '';
      })
      this.dataSourceFlights.data = this.flights
    })
    this.formGroup1 = new FormGroup({
      placeDeparture: new FormControl(''),
      dateDeparture: new FormControl(''),
      placeArrival: new FormControl(''),
      availableTickets: new FormControl(''),
    });
  }
  Book(flight:Flight):void{

  }
  search():void{
    
    this.dateDeparture = this.datepipe.transform(this.dateDeparture, 'dd-MM-yyyy HH:mm')??''
    
    this.flightService.getFlightsActionsSearch(this.placeArrival, this.placeDeparture, this.dateDeparture, this.availableTickets).subscribe(res => {
      this.flights = Array.from(res.flights)
      this.flights.forEach(f => {
        f.departure.time = this.datepipe.transform(f.departure.time, 'dd-MM-yyyy HH:mm') ?? '';
        f.arrival.time = this.datepipe.transform(f.arrival.time, 'dd-MM-yyyy HH:mm') ?? '';
      });
      this.dataSourceFlights.data = this.flights
      this.dateDeparture=''
  })
}
  showSuccess(message: string) {
    this.toastr.success(message, 'Booking application');
  }
  showError(message: string) {
    this.toastr.error(message, 'Booking application');
  }
}
