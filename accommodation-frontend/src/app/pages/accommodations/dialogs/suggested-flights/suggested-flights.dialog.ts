import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { FlightsService } from 'src/app/api/api/flight.service';
import { UserManagementService } from 'src/app/api/api/user-management.service';
import { BookFlightDto } from 'src/app/api/model/bookFlightDto';
import { GetSuggestedFlightsDto } from 'src/app/api/model/getSuggestedFlightsDto';
import { AuthService } from 'src/app/core/keycloak/auth.service';

@Component({
  selector: 'app-suggested-flights',
  templateUrl: './suggested-flights.dialog.html',
  styleUrls: ['./suggested-flights.dialog.css']
})
export class SuggestedFlightsDialog implements OnInit {

  dataSource = new MatTableDataSource<any>();
  dataSource2 = new MatTableDataSource<any>();
  displayedColumnsFlights = ['departure', 'arrival', 'price', 'buttons'];
  showSpinner: boolean = false;
  numberOfTicketsForGoing: number[] = [];
  numberOfTicketsForReturning: number[] = [];
  apiKeysForGoing: string[] = [];
  apiKeysForReturning: string[] = [];
  constructor(
    public flightService: FlightsService,
    public dialogRef: MatDialogRef<SuggestedFlightsDialog>,
    @Inject (MAT_DIALOG_DATA) public data: GetSuggestedFlightsDto,
    public toastr: ToastrService,
    public userService : AuthService,
    public userManagementService: UserManagementService
  ) { }

  ngOnInit() {
    this.showSpinner = true;
    this.flightService.getSuggestedFlights(this.data).subscribe({
      next: (result: any) => {
        this.showSpinner = false;
        this.dataSource.data = result.flightsForGoing
        this.dataSource2.data = result.flightsForReturning
        this.numberOfTicketsForGoing = new Array(this.dataSource.data.length).fill(1);
        this.numberOfTicketsForReturning = new Array(this.dataSource.data.length).fill(1);
        this.userManagementService.getApiKey(this.userService.getUser()?.id!).subscribe({
          next: (result: any) => {
            this.apiKeysForGoing = new Array(this.dataSource.data.length).fill(result.apiKey);
            this.apiKeysForReturning = new Array(this.dataSource.data.length).fill(result.apiKey);
          },
          error: (e:any) => {
            this.toastr.error("something went wrong")
          }
        })
      },
      error: (e: any) => {
        this.showSpinner = false;
        this.toastr.error("Something went wrong while getting suggested flights :/");
        console.log(e);
      }
    })
  }

  bookFlightForGoing(flight:any, index:number){
    var dto : BookFlightDto = {
      flightId: flight.id,
      numberOfTickets: this.numberOfTicketsForGoing[index],
      apiKey: this.apiKeysForGoing[index],
      userId: this.userService.getUser()!.email
    }
    this.flightService.bookFlight(dto).subscribe({
      next: (result : boolean) => {
        if(result){
          this.toastr.success("Flight booked successfully!")
          this.dialogRef.close();
        }else{
          this.toastr.error("Something went wrong while booking flights :/")
        }
      },
      error: (e:any) => {
        this.toastr.error("Error happened on the server :/")
        console.log(e);
      }
    })
  }
  bookFlightForReturning(flight:any, index:number){
    var dto : BookFlightDto = {
      flightId: flight.id,
      numberOfTickets: this.numberOfTicketsForReturning[index],
      apiKey: this.apiKeysForReturning[index],
      userId: this.userService.getUser()!.email
    }
    this.flightService.bookFlight(dto).subscribe({
      next: (result : boolean) => {
        if(result){
          this.toastr.success("Flight booked successfully!")
          this.dialogRef.close();
        }else{
          this.toastr.error("Something went wrong while booking flights :/")
        }
      },
      error: (e:any) => {
        this.toastr.error("Error happened on the server :/")
        console.log(e);
      }
    })
  }
}
