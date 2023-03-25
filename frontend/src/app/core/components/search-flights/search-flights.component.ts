import { Component } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Flight, FlightGetAllResponse, FlightService } from 'src/app/api';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { FormGroup } from '@angular/forms';
import { FormControl } from '@angular/forms';
@Component({
  selector: 'app-search-flights',
  templateUrl: './search-flights.component.html',
  styleUrls: ['./search-flights.component.scss']
})
export class SearchFlightsComponent {
  dataSourceFlights = new MatTableDataSource<Flight>();
  displayedColumnsFlights = ['departurePlace', 'departureTime', 'arrivalPlace', 'arrivalTime',
                             'totalTickets', 'availableTickets', 'ticketPrice', 'totalPrice', 'cancel'
                            ];
  flights!: Flight[];
  flight!: Flight;
  formGroup1!: FormGroup;
  placeDeparture: string = '';
  dateDeparture: Date = new Date();
  placeArrival: string = '';
  availableTickets: number = 0;

  constructor(private datepipe: DatePipe, private flightService: FlightService, private toastr : ToastrService) { }

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
    console.log(this.placeDeparture)
    console.log(this.placeArrival)
    console.log(this.dateDeparture)
    console.log(this.availableTickets)
  }
  showSuccess(message: string) {
    this.toastr.success(message, 'Booking application');
  }
  showError(message: string) {
    this.toastr.error(message, 'Booking application');
  }
}
