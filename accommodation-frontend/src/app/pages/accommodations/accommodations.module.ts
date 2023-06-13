import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccommodationsComponent } from './accommodations.component';
import { AccommodationRoutingModule } from './accommodation-routing.module';
import { MatTableModule } from '@angular/material/table';
import { MaterialModule } from 'src/app/core/material/material.module';
import { SuggestedFlightsDialog } from './dialogs/suggested-flights/suggested-flights.dialog';
@NgModule({
  imports: [
    CommonModule,
    AccommodationRoutingModule,
    MatTableModule,
    MaterialModule
  ],
  declarations: [AccommodationsComponent, SuggestedFlightsDialog]
})
export class AccommodationsModule { }
