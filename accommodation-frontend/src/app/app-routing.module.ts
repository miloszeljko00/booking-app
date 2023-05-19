import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReservationsReviewComponent } from './core/components/reservations-review/reservations-review.component';
import { RequestsReviewComponent } from './core/components/requests-review/requests-review.component';
import { AddAccommodationComponent } from './pages/add-accommodation/add-accommodation.component';
import { AdminRequestsReviewComponent } from './core/components/admin-requests-review/admin-requests-review.component';
import { AdminAccommodationComponent } from './core/components/admin-accommodation/admin-accommodation.component';
import { NotificationsComponent } from './core/components/notifications/notifications.component';
const routes: Routes =
[
  { path: '', loadChildren: () => import('./pages/home/home.module').then(m => m.HomeModule) },
  { path: 'search-accommodations', loadChildren: () => import('./pages/accommodations/accommodations.module').then(m => m.AccommodationsModule) },
  { path: 'reservations-review', component: ReservationsReviewComponent},
  { path: 'registration', loadChildren: () => import('./pages/registration/registration.module').then(m => m.RegistrationModule) },
  { path: 'account', loadChildren: () => import('./pages/account/account.module').then(m => m.AccountModule) },
  { path: 'requests-review', component: RequestsReviewComponent },
  { path: 'admin-requests-review', component: AdminRequestsReviewComponent },
  { path: 'add-accommodation', component: AddAccommodationComponent },
  { path: 'admin-accommodation', component: AdminAccommodationComponent },
  { path: 'notification', component: NotificationsComponent },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
