import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.scss']
})
export class DialogComponent {
  formGroup!: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<DialogComponent>,
    @Inject (MAT_DIALOG_DATA) public amount: number,
  ) {}

  ngOnInit(){
    this.formGroup = new FormGroup({
      amount: new FormControl('',[Validators.min(1), Validators.required]),
    });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
