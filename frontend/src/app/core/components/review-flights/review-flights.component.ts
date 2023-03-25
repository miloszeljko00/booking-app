import { Component } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Flight, FlightGetAllResponse, FlightService } from 'src/app/api';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-review-flights',
  templateUrl: './review-flights.component.html',
  styleUrls: ['./review-flights.component.scss']
})
export class ReviewFlightsComponent {

  dataSourceFlights = new MatTableDataSource<Flight>();
  displayedColumnsFlights = ['departurePlace', 'departureTime', 'arrivalPlace', 'arrivalTime',
                             'totalTickets', 'availableTickets', 'ticketPrice', 'totalPrice', 'cancel'
                            ];
  flights!: Flight[];
  flight!: Flight;

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
  }

  cancel(flight:Flight){
    this.flightService.deleteFlightsId(flight.flightId).subscribe({
      next: () => {
        this.showSuccess('Successfully deleted flight');
        this.flights.forEach(f => {
          if(f.flightId == flight.flightId)
            f.canceled = !f.canceled;
        });
        this.dataSourceFlights.data = this.flights;
      },
      error: (e) => this.showError('Error happened while deleting flight')
    })
  }

  showSuccess(message: string) {
    this.toastr.success(message, 'Booking application'
    );
  }
  showError(message: string) {
    this.toastr.error(message, 'Bbooking application');
  }
}
