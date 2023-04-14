import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccommodationsComponent } from './accommodations.component';
import { AccommodationRoutingModule } from './accommodation-routing.module';

@NgModule({
  imports: [
    CommonModule,
    AccommodationRoutingModule
  ],
  declarations: [AccommodationsComponent]
})
export class AccommodationsModule { }
