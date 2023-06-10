import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-update-host-grading-dialog',
  templateUrl: './update-host-grading-dialog.component.html',
  styleUrls: ['./update-host-grading-dialog.component.scss']
})
export class UpdateHostGradingDialogComponent {
  formGroup!: FormGroup;
  grade: number = 0;

  constructor(
    public dialogRef: MatDialogRef<UpdateHostGradingDialogComponent>,
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
