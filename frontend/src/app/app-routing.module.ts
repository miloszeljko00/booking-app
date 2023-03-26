import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateFlightComponent } from './core/components/create-flight/create-flight.component';
import { ReviewFlightsComponent } from './core/components/review-flights/review-flights.component';
import { SearchFlightsComponent } from './core/components/search-flights/search-flights.component';
import { UserFlightsComponent } from './core/components/user-flights/user-flights.component';
const routes: Routes = [{ path: '', loadChildren: () => import('./pages/home/home.module').then(m => m.HomeModule) },
{path: 'create-flight', component: CreateFlightComponent},
{path: 'review-flights', component: ReviewFlightsComponent},
{path: 'search-flights', component: SearchFlightsComponent},
{path: 'user-flights', component: UserFlightsComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
