import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccommodationsComponent } from './accommodations.component';
import { AccommodationRoutingModule } from './accommodation-routing.module';
import { MatTableModule } from '@angular/material/table';

@NgModule({
  imports: [
    CommonModule,
    AccommodationRoutingModule,
    MatTableModule
  ],
  declarations: [AccommodationsComponent]
})
export class AccommodationsModule { }
