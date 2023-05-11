import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AccommodationService } from 'src/app/api/api/accommodation.service';
import { AuthService } from '../../keycloak/auth.service';
import { RequestByAdmin } from 'src/app/api/model/requestByAdmin';
import { User } from '../../keycloak/model/user';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-admin-requests-review',
  templateUrl: './admin-requests-review.component.html',
  styleUrls: ['./admin-requests-review.component.scss']
})
export class AdminRequestsReviewComponent {
  dataSourceRequests = new MatTableDataSource<RequestByAdmin>();
  displayedColumnsRequests = ['email', 'name', 'start', 'end', 'numberOfGuests', 'numberOfCancellations', 'status', 'accept', 'reject'];
                            
  requestsList!: RequestByAdmin[];
  req!: RequestByAdmin;
  user!: User | null;

  constructor(public dialog: MatDialog,private accService: AccommodationService, private toastr : ToastrService, private authService: AuthService) {
    this.user = this.authService.getUser()
  }

  ngOnInit() {
    this.accService.getRequestsByAdmin(this.user?.email ?? '').subscribe((response: any) => {
      this.requestsList = response;
      this.dataSourceRequests.data = this.requestsList
    })
  }

  showSuccess(message: string) {
    this.toastr.success(message, 'Booking application');
  }
  showError(message: string) {
    this.toastr.error(message, 'Booking application');
  }

}
