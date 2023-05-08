import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatTableModule } from '@angular/material/table';
import { Accommodation } from 'src/app/api/model/accommodation';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { AccommodationService } from 'src/app/api/api/accommodation.service';

@Component({
  selector: 'app-accommodations',
  templateUrl: './accommodations.component.html',
  styleUrls: ['./accommodations.component.scss']
})
export class AccommodationsComponent implements OnInit {

  dataSourceAcc = new MatTableDataSource<Accommodation>();
  displayedColumnsFlights = ['name', 'address', 'price', 'benefits', 'min', 'max'];
                            
  accomodationList!: Accommodation[];
  acc!: Accommodation;
  constructor(private datepipe: DatePipe, private accService: AccommodationService, private toastr : ToastrService) { }

  ngOnInit() {
    this.accService.getAll().subscribe((response: any) => {
      console.log(response)
      this.accomodationList = response;
      
      this.dataSourceAcc.data = this.accomodationList
    })
  }

  showSuccess(message: string) {
    this.toastr.success(message, 'Booking application');
  }
  showError(message: string) {
    this.toastr.error(message, 'Bbooking application');
  }

}
