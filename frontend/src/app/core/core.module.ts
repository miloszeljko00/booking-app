import { MatButtonModule } from '@angular/material/button';
import { AppRoutingModule } from './../app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NavbarComponent } from './components/navbar/navbar.component';
import { MaterialModule } from './material/material.module';
import { NavbarButtonComponent } from './components/navbar-button/navbar-button.component';


@NgModule({
  declarations: [
    NavbarComponent,
    NavbarButtonComponent
  ],
  imports:[
    BrowserModule,
    AppRoutingModule,
    MaterialModule,
  ],
  exports:[
    NavbarComponent
  ]
})
export class CoreModule { }
