import { Component } from '@angular/core';
import { UserFlightTicket, UserService } from 'src/app/api';
import { User } from '../../keycloak/model/user';
import { AuthService } from '../../keycloak/auth.service';
import { MatTableDataSource } from '@angular/material/table';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-user-flights',
  templateUrl: './user-flights.component.html',
  styleUrls: ['./user-flights.component.scss']
})
export class UserFlightsComponent {

  user!: User | null;
  dataSourceFlights = new MatTableDataSource<UserFlightTicket>();
  displayedColumnsFlights = ['departurePlace', 'departureTime', 'arrivalPlace', 'arrivalTime',
                             'Purchased'];
  tickets!: UserFlightTicket[];
                            
  constructor(private userService: UserService, private authService:AuthService, private datepipe: DatePipe){
    this.user = this.authService.getUser()
  }

  ngOnInit(): void {
    if(this.user!=null)
      this.userService.getUsersIdFlightTickets(this.user.email).subscribe(res => {
        console.log(res)
        
        this.tickets = Array.from(res.flightTickets)
        this.tickets.forEach(f => {
        f.flight.departure.time = this.datepipe.transform(f.flight.departure.time, 'dd-MM-yyyy HH:mm') ?? '';
        f.flight.arrival.time = this.datepipe.transform(f.flight.arrival.time, 'dd-MM-yyyy HH:mm') ?? '';
        f.purchased = this.datepipe.transform(f.purchased, 'dd-MM-yyyy HH:mm') ?? '';
        })
        this.dataSourceFlights.data = this.tickets
      })
  }

}
