import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FlightsIntegrationRoutingModule } from './flights-integration-routing.module';
import { FlightsIntegrationComponent } from './flights-integration.component';
import { MaterialModule } from 'src/app/core/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    FlightsIntegrationComponent
  ],
  imports: [
    CommonModule,
    FlightsIntegrationRoutingModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class FlightsIntegrationModule { }
