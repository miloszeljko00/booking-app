import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { UserManagementService } from 'src/app/api/api/user-management.service';
import { AuthService } from 'src/app/core/keycloak/auth.service';

@Component({
  templateUrl: './registration.page.html',
  styleUrls: ['./registration.page.scss']
})
export class RegistrationPage {
  formGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)]),
    roles: new FormControl('guest', [Validators.required]),
    name: new FormControl('', [Validators.required]),
    surname: new FormControl('', [Validators.required]),
    country: new FormControl('', [Validators.required]),
    city: new FormControl('', [Validators.required]),
    street: new FormControl('', [Validators.required]),
    number: new FormControl('', [Validators.required]),
  });

  constructor(private userManagementService: UserManagementService, private authService: AuthService, private toastr: ToastrService) {}

  register() {
    if(!this.formGroup.get('email')) return
    if(!this.formGroup.get('password')) return
    if(!this.formGroup.get('roles')) return
    if(!this.formGroup.get('name')) return
    if(!this.formGroup.get('surname')) return
    if(!this.formGroup.get('country')) return
    if(!this.formGroup.get('city')) return
    if(!this.formGroup.get('street')) return
    if(!this.formGroup.get('number')) return
    const request = {
      email: this.formGroup.get('email')!.value,
      password: this.formGroup.get('password')!.value,
      roles: [this.formGroup.get('roles')!.value],
      name: this.formGroup.get('name')!.value,
      surname: this.formGroup.get('surname')!.value,
      country: this.formGroup.get('country')!.value,
      city: this.formGroup.get('city')!.value,
      street: this.formGroup.get('street')!.value,
      number: this.formGroup.get('number')!.value,
    }

    this.userManagementService.register(request).subscribe({
      next: (response) => {
        if(response) this.authService.login()
        else this.toastr.error('Username already registered!')
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error('Username already registered!')
      }
    })

  }
}
