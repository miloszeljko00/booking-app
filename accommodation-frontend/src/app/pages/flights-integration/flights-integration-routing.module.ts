import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FlightsIntegrationComponent } from './flights-integration.component';

const routes: Routes = [{ path: '', component: FlightsIntegrationComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FlightsIntegrationRoutingModule { }
