import { AppRoutingModule } from './../app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NavbarComponent } from './components/navbar/navbar.component';
import { MaterialModule } from './material/material.module';
import { NavbarButtonComponent } from './components/navbar-button/navbar-button.component';
import { CreateFlightComponent } from './components/create-flight/create-flight.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';
import { MatNativeDateModule, MatRippleModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { ToastrModule } from 'ngx-toastr';
import { ReviewFlightsComponent } from './components/review-flights/review-flights.component';
import { SearchFlightsComponent } from './components/search-flights/search-flights.component';
import { UserFlightsComponent } from './components/user-flights/user-flights.component';
import { MatDialogModule } from '@angular/material/dialog';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { DialogComponent } from './components/dialog/dialog.component';


@NgModule({
  declarations: [
    NavbarComponent,
    NavbarButtonComponent,
    CreateFlightComponent,
    ReviewFlightsComponent,
    SearchFlightsComponent,
    UserFlightsComponent,
    DialogComponent
  ],
  imports:[
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
      timeOut: 3000,
      positionClass: 'toast-top-right',
    }),

  ],
  exports:[
    NavbarComponent
  ]
})
export class CoreModule { }
