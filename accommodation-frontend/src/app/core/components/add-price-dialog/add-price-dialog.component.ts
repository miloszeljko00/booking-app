import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { DialogComponent } from '../dialog/dialog.component';

@Component({
  selector: 'app-add-price-dialog',
  templateUrl: './add-price-dialog.component.html',
  styleUrls: ['./add-price-dialog.component.scss']
})
export class AddPriceDialogComponent {
  formGroup!: FormGroup;
  startDate: string = '';
  endDate: string = '';
  price: number = 1;

  constructor(
    public dialogRef: MatDialogRef<DialogComponent>
  ) {}

  ngOnInit(){
    this.formGroup = new FormGroup({
      price: new FormControl('',[Validators.min(1), Validators.required]),
      startingDate: new FormControl('',[Validators.required]),
      endingDate: new FormControl('',[Validators.required]),
    
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
