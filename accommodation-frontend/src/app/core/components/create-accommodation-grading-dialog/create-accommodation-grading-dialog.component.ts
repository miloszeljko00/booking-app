import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AccommodationMain } from 'src/app/api/model/accommodationMain';
import { AuthService } from '../../keycloak/auth.service';
import { AccommodationService } from 'src/app/api/api/accommodation.service';

@Component({
  selector: 'app-create-accommodation-grading-dialog',
  templateUrl: './create-accommodation-grading-dialog.component.html',
  styleUrls: ['./create-accommodation-grading-dialog.component.scss']
})
export class CreateAccommodationGradingDialogComponent {
  formGroup!: FormGroup;
  grade: number = 0;
  accommodation: AccommodationMain = {name: '', hostEmail: ''};
  accommodationList: AccommodationMain[] = []; 

  constructor(
    public dialogRef: MatDialogRef<CreateAccommodationGradingDialogComponent>,
    private authService:AuthService,
    private accommodationService: AccommodationService
  ) {}

  ngOnInit(){
    this.formGroup = new FormGroup({
      grade: new FormControl('',[Validators.min(1), Validators.max(5),Validators.required]),
    });
    this.accommodationService.getAccommodationByGuestReservation(this.authService.getUser()?.email ?? '').subscribe((response: any) => {
      this.accommodationList = response;
    })
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
