import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ApiKeyRoutingModule } from './api-key-routing.module';
import { ApiKeyComponent } from './api-key.component';
import { MaterialModule } from 'src/app/core/material/material.module';


@NgModule({
  declarations: [
    ApiKeyComponent
  ],
  imports: [
    CommonModule,
    ApiKeyRoutingModule,
    MaterialModule,
  ]
})
export class ApiKeyModule { }
