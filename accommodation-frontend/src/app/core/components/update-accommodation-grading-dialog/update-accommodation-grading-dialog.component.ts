import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-update-accommodation-grading-dialog',
  templateUrl: './update-accommodation-grading-dialog.component.html',
  styleUrls: ['./update-accommodation-grading-dialog.component.scss']
})
export class UpdateAccommodationGradingDialogComponent {
  formGroup!: FormGroup;
  grade: number = 0;

  constructor(
    public dialogRef: MatDialogRef<UpdateAccommodationGradingDialogComponent>,
    @Inject (MAT_DIALOG_DATA) public previousGrade: number,
  ) {}

  ngOnInit(){
    this.grade = this.previousGrade;
    this.formGroup = new FormGroup({
      grade: new FormControl('',[Validators.min(1), Validators.max(5),Validators.required]),
    });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
