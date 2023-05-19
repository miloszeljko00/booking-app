import { Component } from '@angular/core';
import { AuthService } from '../../keycloak/auth.service';
import { ToastrService } from 'ngx-toastr';
import { User } from '../../keycloak/model/user';
import { GuestNotification } from 'src/app/api/model/guestNotification';
import { HostNotification } from 'src/app/api/model/hostNotification';
import { NotificationService } from 'src/app/api/api/notification.service';
import { CreateGuestNotification } from 'src/app/api/model/createGuestNotification';
import { CreateHostNotification } from 'src/app/api/model/createHostNotification';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent {
  user!: User | null;
  guestNotification: GuestNotification = {lastModified: new Date().toString(), guestEmail: '', receiveAnswer: false};
  hostNotification: HostNotification = {lastModified: new Date().toString(), hostEmail: '', receiveAnswerForCreatedRequest: false,
  receiveAnswerForCanceledReservation: false, receiveAnswerForHostRating: false, receiveAnswerForAccommodationRating: false, receiveAnswerForHighlightedHostStatus: false};

  constructor(private authService: AuthService, private toastr : ToastrService, private notificationService: NotificationService){
    this.user = this.authService.getUser()
    console.log(this.user?.email)
    if(this.user?.roles.includes('guest'))
      this.notificationService.getGuestNotifications(this.user?.email ?? '').subscribe((response: any) => {
        this.guestNotification = response;
      })
    else
      this.notificationService.getHostNotifications(this.user?.email ?? '').subscribe((response: any) => {
        this.hostNotification = response;
      })
  }

  checkIfGuest(){
    if(this.user?.roles.includes('guest'))
      return true
    return false
  }

  save(){
    if(this.user?.roles.includes('guest')){
      let guestNotification: CreateGuestNotification = {guestEmail: this.user?.email ?? '', receiveAnswer: this.guestNotification.receiveAnswer}
      this.notificationService.setGuestNotifications(guestNotification).subscribe({
        next: () => {
          this.showSuccess('Successfully changed notifications settings');
        },
        error: (e) => {
          this.showError(e.error);
        }
      });
    }
    else{
      let hostNotification: CreateHostNotification = {hostEmail: this.user?.email ?? '', receiveAnswerForCreatedRequest: this.hostNotification.receiveAnswerForCreatedRequest,
      receiveAnswerForCanceledReservation: this.hostNotification.receiveAnswerForCanceledReservation, receiveAnswerForHostRating: this.hostNotification.receiveAnswerForHostRating,
      receiveAnswerForAccommodationRating: this.hostNotification.receiveAnswerForAccommodationRating, receiveAnswerForHighlightedHostStatus: this.hostNotification.receiveAnswerForHighlightedHostStatus}
      this.notificationService.setHostNotifications(hostNotification).subscribe({
        next: () => {
          this.showSuccess('Successfully changed notifications settings');
        },
        error: (e) => {
          this.showError(e.error);
        }
      });
    }
  }

  showSuccess(message: string) {
    this.toastr.success(message, 'Booking application');
  }
  showError(message: string) {
    this.toastr.error(message, 'Booking application');
  }
}
