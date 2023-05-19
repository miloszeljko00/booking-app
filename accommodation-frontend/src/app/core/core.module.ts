import { AppRoutingModule } from './../app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { MaterialModule } from './material/material.module';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';
import { MatNativeDateModule, MatRippleModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { ToastrModule } from 'ngx-toastr';
import { MatDialogModule } from '@angular/material/dialog';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { NavbarComponent } from './components/navbar/navbar.component';
import { NavbarButtonComponent } from './components/navbar-button/navbar-button.component';
import { DialogComponent } from './components/dialog/dialog.component';
import { ReservationsReviewComponent } from './components/reservations-review/reservations-review.component';
import { RequestsReviewComponent } from './components/requests-review/requests-review.component';
import { AddAccommodationComponent } from '../pages/add-accommodation/add-accommodation.component';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { AdminRequestsReviewComponent } from './components/admin-requests-review/admin-requests-review.component';
import { AdminAccommodationComponent } from './components/admin-accommodation/admin-accommodation.component';
import { AddPriceDialogComponent } from './components/add-price-dialog/add-price-dialog.component';
import { MatSidenav, MatSidenavModule } from '@angular/material/sidenav';
import { NotificationsComponent } from './components/notifications/notifications.component';
import { GradingComponent } from './components/grading/grading.component';


@NgModule({
  declarations: [
    NavbarComponent,
    NavbarButtonComponent,
    DialogComponent,
    ReservationsReviewComponent,
    RequestsReviewComponent,
    AddAccommodationComponent,
    ConfirmDialogComponent,
    AdminRequestsReviewComponent,
    AdminAccommodationComponent,
    AddPriceDialogComponent,
    NotificationsComponent,
    GradingComponent,

  ],
  imports:[
    MatSidenavModule,
    MatButtonModule,
    MatRippleModule,
    BrowserModule,
    AppRoutingModule,
    MaterialModule,
    MatDatepickerModule,
    NgxMaterialTimepickerModule,
    MatNativeDateModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    FormsModule,
    MatDialogModule,
    ToastrModule.forRoot({
      timeOut: 4000,
      positionClass: 'toast-top-right',
    }),

  ],
  exports:[
    NavbarComponent,
    
  ]
})
export class CoreModule { }
