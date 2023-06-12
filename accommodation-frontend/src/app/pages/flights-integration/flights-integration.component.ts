import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectorRef, Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { UserManagementService } from 'src/app/api/api/user-management.service';
import { AuthService } from 'src/app/core/keycloak/auth.service';

@Component({
  selector: 'app-flights-integration',
  templateUrl: './flights-integration.component.html',
  styleUrls: ['./flights-integration.component.scss']
})
export class FlightsIntegrationComponent {
  formGroup = new FormGroup({
    apiKey: new FormControl('', [Validators.required]),
  });

  constructor(
    private authService: AuthService,
    private toastr: ToastrService,
    private userManagementService: UserManagementService
  ) {}


  ngOnInit() {
    this.getApiKey()
  }

  getApiKey() {
    const user = this.authService.getUser()
    if(!user) return
    this.userManagementService.getApiKey(user.id).subscribe({
      next: (response: any) => {
        const formValues = {
          apiKey: response.apiKey
        };
      
        this.formGroup.patchValue(formValues);
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error('Error occurred while getting API Key!')
      }
    })
  }

  update() {
    const user = this.authService.getUser()
    if(!user) return
    if(!this.formGroup.get('apiKey')) return
    const request = {
      userId: user.id,
      apiKey: this.formGroup.get('apiKey')!.value
    }

    this.userManagementService.updateApiKey(request).subscribe({
      next: (response) => {
        if(response) this.toastr.success('API Key updated successfully!')
        else this.toastr.error('Error occurred while updating API Key!')
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error('Error occurred while updating API Key!')
      }
    })

  }
}
