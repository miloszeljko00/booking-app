import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReservationsReviewComponent } from './core/components/reservations-review/reservations-review.component';
import { RequestsReviewComponent } from './core/components/requests-review/requests-review.component';
import { AddAccommodationComponent } from './pages/add-accommodation/add-accommodation.component';
const routes: Routes =
[
  { path: '', loadChildren: () => import('./pages/home/home.module').then(m => m.HomeModule) },
  { path: 'search-accommodations', loadChildren: () => import('./pages/accommodations/accommodations.module').then(m => m.AccommodationsModule) },
  { path: 'reservations-review', component: ReservationsReviewComponent},
  { path: 'requests-review', component: RequestsReviewComponent},
  {path: 'add-accomodation', component: AddAccommodationComponent},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
