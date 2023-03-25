import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { FlightCreateRequest, FlightService } from 'src/app/api';

@Component({
  selector: 'app-create-flight',
  templateUrl: './create-flight.component.html',
  styleUrls: ['./create-flight.component.scss']
})
export class CreateFlightComponent {
  formGroup1!: FormGroup;
  formGroup2!: FormGroup;
  flight!: FlightCreateRequest;
  placeDeparture: string = '';
  dateDeparture: string = '';
  timeDeparture: string = '';
  placeArrival: string = '';
  dateArrival: string = '';
  timeArrival: string = '';
  ticketPrice: number = 0;
  totalTickets: number = 0;

  constructor(private datepipe: DatePipe, private flightService: FlightService, private toastr : ToastrService){}

  ngOnInit(): void {
    this.formGroup1 = new FormGroup({
      placeDeparture: new FormControl('',[Validators.required]),
      dateDeparture: new FormControl('',[Validators.required]),
      timeDeparture: new FormControl('',[Validators.required]),
      placeArrival: new FormControl('',[Validators.required]),
      dateArrival: new FormControl('',[Validators.required]),
      timeArrival: new FormControl('',[Validators.required]),
    });
    this.formGroup2 = new FormGroup({
      ticketPrice: new FormControl('',[Validators.min(1), Validators.required]),
      totalTickets: new FormControl('',[Validators.min(1), Validators.required]),
    });
  }

  getDateArrival(date: string){
    this.dateArrival = date;
  }

  getDateDeparture(date: string){
    this.dateDeparture = date;
  }

  getTimeArrival(time: string){
    this.timeArrival = time;
  }

  getTimeDeparture(time: string){
    this.timeDeparture = time;
  }

  create(){
    let dateDeparture = new Date(this.dateDeparture);
    let dateDepartureString =this.datepipe.transform(dateDeparture, 'yyyy-MM-dd');
    let dateArrival = new Date(this.dateArrival);
    let dateArrivalString =this.datepipe.transform(dateArrival, 'yyyy-MM-dd');
    if(dateDeparture > dateArrival){
      this.showError('Date of arrival is before date of departure');
      return;
    }
    else if(dateDepartureString == dateArrivalString){
      if(Number(this.timeDeparture.split(':')[0]) > Number(this.timeArrival.split(':')[0])){
        this.showError('Time of arrival is before time of departure');
        return;
      }
      else if(Number(this.timeDeparture.split(':')[0]) == Number(this.timeArrival.split(':')[0])){
        if(Number(this.timeDeparture.split(':')[1]) >= Number(this.timeArrival.split(':')[1])){
          this.showError('Time of arrival is before time of departure');
          return;
        }
      }
    }
    this.flight = {departure: {time: dateDepartureString + ' ' + this.timeDeparture, city: this.placeDeparture},
                   arrival: {time: dateArrivalString + ' ' + this.timeArrival, city: this.placeArrival},
                   totalTickets: this.totalTickets,
                   ticketPrice: this.ticketPrice
                  }
    this.flightService.postFlights(this.flight).subscribe({
      next: (flight) => {
        this.showSuccess('Successfully created flight');
      },
      error: (e) => this.showError('Error happened while creating flight')
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
