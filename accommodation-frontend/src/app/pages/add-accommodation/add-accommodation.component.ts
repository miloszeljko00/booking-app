import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { AccommodationService } from 'src/app/api/api/accommodation.service';

@Component({
  selector: 'app-add-accommodation',
  templateUrl: './add-accommodation.component.html',
  styleUrls: ['./add-accommodation.component.scss']
})
export class AddAccommodationComponent implements OnInit {

  selectedFiles?: FileList;
  previews: string[] = [];
  formGroup1!: FormGroup;
  name: string = "";
  min: number = 1;
  max: number = 1;
  street: string = "";
  num: string = "";
  city: string = "";
  country: string = "";
  benefits = new FormControl('');
  benefitList: string[] = ['Extra cheese', 'Mushroom', 'Onion', 'Pepperoni', 'Sausage', 'Tomato'];
  constructor(private datepipe: DatePipe, private toastr : ToastrService, private accommodationService: AccommodationService){}

  ngOnInit(): void {
    this.formGroup1 = new FormGroup({
      name: new FormControl('', [Validators.required]),
      min: new FormControl('',[Validators.required]),
      max: new FormControl('',[Validators.required]),
      city: new FormControl('',[Validators.required]),
      street: new FormControl('',[Validators.required]),
      num: new FormControl('',[Validators.required]),
      country: new FormControl('',[Validators.required])
    });
    this.accommodationService.getBenefits().subscribe((response: any) => {
      this.benefitList = response;
      console.log(response)
    })
  }

  create(){

  }
  showSuccess(message: string) {
    this.toastr.success(message, 'Booking application'
    );
  }
  showError(message: string) {
    this.toastr.error(message, 'Booking application');
  }

  selectFiles(event: any): void {
    this.selectedFiles = event.target.files;
  
    this.previews = [];
    if (this.selectedFiles && this.selectedFiles[0]) {
      const numberOfFiles = this.selectedFiles.length;
      for (let i = 0; i < numberOfFiles; i++) {
        const reader = new FileReader();
  
        reader.onload = (e: any) => {
          console.log(e.target.result);
          this.previews.push(e.target.result);
        };
  
        reader.readAsDataURL(this.selectedFiles[i]);
      }
    }
  }

}
