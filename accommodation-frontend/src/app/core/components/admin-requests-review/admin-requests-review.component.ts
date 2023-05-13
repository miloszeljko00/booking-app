import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AccommodationService } from 'src/app/api/api/accommodation.service';
import { AuthService } from '../../keycloak/auth.service';
import { RequestByAdmin } from 'src/app/api/model/requestByAdmin';
import { User } from '../../keycloak/model/user';
import { MatTableDataSource } from '@angular/material/table';
import { RequestManagement } from 'src/app/api/model/requestManagement';

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
  request!: RequestManagement;
  user!: User | null;

  constructor(public dialog: MatDialog,private accService: AccommodationService, private toastr : ToastrService, private authService: AuthService) {
    this.user = this.authService.getUser()
  }

  ngOnInit() {
    this.accService.getRequestsByAdmin(this.user?.email ?? '').subscribe((response: any) => {
      this.requestsList = response;
      this.requestsList.sort((a, b) => {
        const dateA = new Date(a.start);
        const dateB = new Date(b.start);
        return dateA.getTime() - dateB.getTime();
      });
      this.dataSourceRequests.data = this.requestsList
    })
  }

  showSuccess(message: string) {
    this.toastr.success(message, 'Booking application');
  }
  showError(message: string) {
    this.toastr.error(message, 'Booking application');
  }

  manageRequest(request:RequestByAdmin, operation: string){
    this.request = {operation: operation, accommodationId: request.accommodationId, reservationId: request.id}
    this.accService.manageReservationRequest(this.request).subscribe({
      next: () => {
        this.showSuccess('Successfully managed reservation request');
        this.accService.getRequestsByAdmin(this.user?.email ?? '').subscribe((response: any) => {
          this.requestsList = response;
          this.requestsList.sort((a, b) => {
            const dateA = new Date(a.start);
            const dateB = new Date(b.start);
            return dateA.getTime() - dateB.getTime();
          });
          this.dataSourceRequests.data = this.requestsList
        })
      },
      error: (e) => {
        this.showError(e.error);
      }
    });
  }

}
