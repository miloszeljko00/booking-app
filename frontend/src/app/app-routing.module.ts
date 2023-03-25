import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateFlightComponent } from './core/components/create-flight/create-flight.component';
import { ReviewFlightsComponent } from './core/components/review-flights/review-flights.component';

const routes: Routes = [{ path: '', loadChildren: () => import('./pages/home/home.module').then(m => m.HomeModule) },
{path: 'create-flight', component: CreateFlightComponent},
{path: 'review-flights', component: ReviewFlightsComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
