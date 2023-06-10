import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { UserManagementService } from 'src/app/api/api/user-management.service';
import { ConfirmDialogComponent, ConfirmDialogModel } from 'src/app/core/components/confirm-dialog/confirm-dialog.component';
import { AuthService } from 'src/app/core/keycloak/auth.service';

@Component({
  templateUrl: './account.page.html',
  styleUrls: ['./account.page.scss']
})
export class AccountPage {
  formGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    roles: new FormControl('guest', [Validators.required]),
    name: new FormControl('', [Validators.required]),
    surname: new FormControl('', [Validators.required]),
    country: new FormControl('', [Validators.required]),
    city: new FormControl('', [Validators.required]),
    street: new FormControl('', [Validators.required]),
    number: new FormControl('', [Validators.required]),
  });

  constructor(
    private userManagementService: UserManagementService, 
    private authService: AuthService,
    private toastr: ToastrService,
    private dialog: MatDialog) {}

  ngOnInit() {
    this.formGroup.get('email')!.disable();
    this.formGroup.get('roles')!.disable();
    const user = this.authService.getUser()
    if(!user) return

    const formValues = {
      email: user.email,
      roles: user.roles.filter(role => role == 'host' || role == 'guest')[0],
    };
  
    this.formGroup.patchValue(formValues);
    this.userManagementService.getUser(user.id).subscribe({
      next: (response:any) => {
        const formValues = {
          name: response.name,
          surname: response.surname,
          country: response.address.country,
          city: response.address.city,
          street: response.address.street,
          number: response.address.number
        };
      
        this.formGroup.patchValue(formValues);
      }
    })
  }
  changePassword(): void {
    const externalUrl = 'https://login-keycloak.azurewebsites.net/auth/realms/booking-app/account/password';
    window.open(externalUrl, '_blank');
  }
  enableTwoFactor(){
    const externalUrl = 'https://login-keycloak.azurewebsites.net/auth/realms/booking-app/account/totp';
    window.open(externalUrl, '_blank');
  }
  deleteAccount(): void {
    const userId = this.authService.getUser()?.id
    if(!userId) return
    const message = `Are you sure you want to delete your account?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });

    dialogRef.afterClosed().subscribe(dialogResult => {
      if(dialogResult === 'false')
        return
      else if(dialogResult === 'true'){
        this.userManagementService.delete(userId).subscribe({
          next: (response: any) => {
            if(response){
              this.toastr.success("Successfully deleted account!")
              setTimeout(() =>  this.authService.logout(), 2000);
            }else{
              const user = this.authService.getUser()
              if(!user){
                this.toastr.error("Error has occurred while deleting account!")
              }else{
                if(user.roles.includes("host")){
                  this.toastr.error("Can't delete account while your accommodations have active reservations!")
                }else{
                  this.toastr.error("Can't delete account while you have active reservations!")
                }
              }
            }
          },
          error: (err: HttpErrorResponse) => {
            const user = this.authService.getUser()
              if(!user){
                this.toastr.error("Error has occurred while deleting account!")
              }else{
                if(user.roles.includes("host")){
                  this.toastr.error("Can't delete account while your accommodations have active reservations!")
                }else{
                  this.toastr.error("Can't delete account while you have active reservations!")
                }
              }
          }
        });
      }
    });
  }
  update() {
    const user = this.authService.getUser()
    if(!user) return
    if(!this.formGroup.get('name')) return
    if(!this.formGroup.get('surname')) return
    if(!this.formGroup.get('country')) return
    if(!this.formGroup.get('city')) return
    if(!this.formGroup.get('street')) return
    if(!this.formGroup.get('number')) return
    const request = {
      userId: user.id,
      name: this.formGroup.get('name')!.value,
      surname: this.formGroup.get('surname')!.value,
      country: this.formGroup.get('country')!.value,
      city: this.formGroup.get('city')!.value,
      street: this.formGroup.get('street')!.value,
      number: this.formGroup.get('number')!.value,
    }

    this.userManagementService.update(request).subscribe({
      next: (response) => {
        if(response) this.toastr.success('Account updated successfully!')
        else this.toastr.error('Error occurred while updating account!')
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error('Error occurred while updating account!')
      }
    })

  }
}
