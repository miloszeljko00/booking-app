import { DatePipe } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { ReservationRequest } from 'src/app/api/model/reservationRequest';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.scss']
})
export class DialogComponent {
  formGroup!: FormGroup;
  startDate: string = '';
  endDate: string = '';
  startTime: string = '';
  endTime: string = '';
  numberOfGuests: number = 1;

  constructor(
    public dialogRef: MatDialogRef<DialogComponent>
  ) {}

  ngOnInit(){
    this.formGroup = new FormGroup({
      numberOfGuests: new FormControl('',[Validators.min(1), Validators.required]),
      startingDate: new FormControl('',[Validators.required]),
      endingDate: new FormControl('',[Validators.required]),
      startingTime: new FormControl('',[Validators.required]),
      endingTime: new FormControl('',[Validators.required]),
    });
  }

  getStartingDate(date: string){
    this.startDate = date;
  }

  getEndingDate(date: string){
    this.endDate = date;
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
