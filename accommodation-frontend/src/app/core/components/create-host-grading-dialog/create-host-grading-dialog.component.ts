import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthService } from '../../keycloak/auth.service';
import { AccommodationService } from 'src/app/api/api/accommodation.service';

@Component({
  selector: 'app-create-host-grading-dialog',
  templateUrl: './create-host-grading-dialog.component.html',
  styleUrls: ['./create-host-grading-dialog.component.scss']
})
export class CreateHostGradingDialogComponent {
  formGroup!: FormGroup;
  grade: number = 0;
  host: string = '';
  hosts: string[] = []; 

  constructor(
    public dialogRef: MatDialogRef<CreateHostGradingDialogComponent>,
    private authService:AuthService,
    private accommodationService: AccommodationService
  ) {}

  ngOnInit(){
    this.formGroup = new FormGroup({
      grade: new FormControl('',[Validators.min(1), Validators.max(5),Validators.required]),
    });
    this.accommodationService.getHostsByGuestReservation(this.authService.getUser()?.email ?? '').subscribe((response: any) => {
      this.hosts = response;
    })
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
