import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectorRef, Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { UserApiService } from 'src/app/core/services/user-api.service';
import { AuthService } from 'src/app/core/keycloak/auth.service';

@Component({
  selector: 'app-api-key',
  templateUrl: './api-key.component.html',
  styleUrls: ['./api-key.component.scss']
})
export class ApiKeyComponent {
  formGroup = new FormGroup({
    apiKey: new FormControl('', [Validators.required]),
  });

  constructor(
    private authService: AuthService,
    private toastr: ToastrService,
    private userApiService: UserApiService
  ) {}


  ngOnInit() {
    this.getApiKey()
  }

  getApiKey() {
    const user = this.authService.getUser()
    if(!user) return
    this.userApiService.getApiKey(user.id).subscribe({
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
  revoke() {
    const user = this.authService.getUser()
    if(!user) return

    this.userApiService.revokeApiKey(user.id).subscribe({
      next: (response:any) => {
        this.toastr.success('API Key revoked successfully!')
          const formValues = {
            apiKey: ''
          };
        
          this.formGroup.patchValue(formValues);
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error('Error occurred while updating API Key!')
      }
    })
  }
  update() {
    const user = this.authService.getUser()
    if(!user) return

    this.userApiService.generateApiKey(user.id).subscribe({
      next: (response:any) => {
        if(response) {
          this.toastr.success('API Key generated successfully!')
          const formValues = {
            apiKey: response.apiKey
          };
        
          this.formGroup.patchValue(formValues);
        }
        else this.toastr.error('Error occurred while generating API Key!')
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error('Error occurred while generating API Key!')
      }
    })

  }
}
