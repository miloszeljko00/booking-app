import { Component } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { RequestByGuest } from 'src/app/api/model/requestByGuest';
import { User } from '../../keycloak/model/user';
import { DatePipe } from '@angular/common';
import { AccommodationService } from 'src/app/api/api/accommodation.service';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../keycloak/auth.service';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent, ConfirmDialogModel } from '../confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-requests-review',
  templateUrl: './requests-review.component.html',
  styleUrls: ['./requests-review.component.scss']
})
export class RequestsReviewComponent {
  dataSourceRequests = new MatTableDataSource<RequestByGuest>();
  displayedColumnsRequests = ['name', 'start', 'end', 'numberOfGuests', 'status', 'cancel'];
                            
  requestsList!: RequestByGuest[];
  req!: RequestByGuest;
  user!: User | null;

  constructor(public dialog: MatDialog,private accService: AccommodationService, private toastr : ToastrService, private authService: AuthService) {
    this.user = this.authService.getUser()
  }

  ngOnInit() {
    this.accService.getRequestsByGuest(this.user?.email ?? '').subscribe((response: any) => {
      this.requestsList = response;
      this.requestsList.forEach(el => {
        if(new Date(el.start) < new Date() && el.status === 'PENDING')
          el.status = 'EXPIRED'
      })
      this.dataSourceRequests.data = this.requestsList
    })
  }

  cancelRequest(req:RequestByGuest){
    const message = `Are you sure you want to cancel this request?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });

    dialogRef.afterClosed().subscribe(dialogResult => {
      if(dialogResult === 'false')
        return
      else if(dialogResult === 'true'){
        let parameter = {accommodationId: req.accommodationId, reservationId: req.id}
        this.accService.cancelReservationRequest(parameter).subscribe({
        next: () => {
          this.showSuccess('Successfully canceled reservation request');
          this.requestsList = this.requestsList.filter(item => item.id !== req.id);
          this.dataSourceRequests.data = this.requestsList
        },
        error: (e) => {
          this.showError(e.error);
        }
      });
      }
    });
  }

  showSuccess(message: string) {
    this.toastr.success(message, 'Booking application');
  }
  showError(message: string) {
    this.toastr.error(message, 'Booking application');
  }

}
