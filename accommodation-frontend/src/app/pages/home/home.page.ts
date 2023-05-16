import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/core/keycloak/auth.service';
import { User } from 'src/app/core/keycloak/model/user';

@Component({
  selector: 'app-home',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.scss']
})
export class HomePage {
  user!:User | null;

  constructor(private toastr : ToastrService, private authService: AuthService) {
  }
  ngOnInit() {
    this.user = this.authService.getUser()
    if(this.user?.roles.includes('host')){
      this.toastr.clear()
      this.toastr.info('DODELJEN VAM JE STATUS ISTAKNUTOG HOSTA!', '', {"timeOut":0, "extendedTimeOut":0, tapToDismiss:false, "positionClass":'toast-bottom-left'})
    }
  }
}
