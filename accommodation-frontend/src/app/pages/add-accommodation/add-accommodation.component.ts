import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { AccommodationService } from 'src/app/api/api/accommodation.service';
import { AccommodationCreate } from 'src/app/api/model/accommodationCreate';
import { AuthService } from 'src/app/core/keycloak/auth.service'; 
import { User } from 'src/app/core/keycloak/model/user';

@Component({
  selector: 'app-add-accommodation',
  templateUrl: './add-accommodation.component.html',
  styleUrls: ['./add-accommodation.component.scss']
})
export class AddAccommodationComponent implements OnInit {
  accommodation!: AccommodationCreate;
  isChecked: boolean = false;
  isCheckedPrice: boolean = false;
  selectedFiles?: FileList;
  previews: string[] = [];
  formGroup1!: FormGroup;
  reserveAutomatically: boolean=false;
  name: string = "";
  min: number = 1;
  max: number = 1;
  street: string = "";
  num: string = "";
  city: string = "";
  country: string = "";
  benefits = new FormControl('');
  selectedIndexes: number[] = [];
  user!: User | null;
  
  benefitList: string[] = [];
  constructor(private authService: AuthService, private toastr : ToastrService, private accommodationService: AccommodationService){
    this.user = this.authService.getUser()
  }

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
      
    })
  }

  create(){
    var perGuest = 1
    if(this.isCheckedPrice)
      perGuest=0;
    console.log(this.selectedIndexes)
    this.accommodation = {name: this.name, address: {street: this.street, number: this.num, city: this.city, country: this.country}, pricePerGuest: [],
                          capacity: {min: this.min, max: this.max}, benefits:this.selectedIndexes, priceCalculation: perGuest, reserveAutomatically: this.isChecked,
                        pictures:this.getPictureList(), hostEmail: this.user?.email ?? ''}; 
    this.accommodationService.createAccomodation(this.accommodation).subscribe({
      next: (acc) => {
        this.showSuccess('Successfully created flight');
        this.formGroup1.reset();
        this.isChecked = false;
        this.isCheckedPrice = false;
        this.selectedIndexes = [];
      },
      error: (e) => this.showError('Error happened while creating flight')
    })

  }

  getPictureList(){
    var returnList = []
    if (this.selectedFiles && this.selectedFiles[0]) {
      const numberOfFiles = this.selectedFiles.length;
      for (let i = 0; i < numberOfFiles; i++) {
          returnList.push({filename: this.selectedFiles[i].name, base64: this.previews[i]})
      }
    }
    return returnList

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
        console.log(this.selectedFiles[i].name);
        reader.onload = (e: any) => {
          console.log(e.target.result);
          
          this.previews.push(e.target.result);
          console.log(this.previews)
        };
  
        reader.readAsDataURL(this.selectedFiles[i]);
      }
    }
  }

}
