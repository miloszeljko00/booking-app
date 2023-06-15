import { Component } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { AccommodationService } from 'src/app/api/api/accommodation.service';
import { AccommodationRecommendation } from 'src/app/api/model/accommodationRecommendation';
import { AuthService } from 'src/app/core/keycloak/auth.service';
import { User } from 'src/app/core/keycloak/model/user';

@Component({
  selector: 'app-recommendation',
  templateUrl: './recommendation.component.html',
  styleUrls: ['./recommendation.component.scss']
})
export class RecommendationComponent {
  accomodationList!: AccommodationRecommendation[];
  acc!: AccommodationRecommendation;
  user!: User | null;
  dataSourceAcc = new MatTableDataSource<AccommodationRecommendation>();
  displayedColumns = ['name', 'avg', 'host'];
  constructor(private accService: AccommodationService, private authService: AuthService) {
    this.user = this.authService.getUser()
  }
  ngOnInit() {
    this.accService.getRecommendedAccommodation(this.user?.email??'').subscribe((response: any) => {
      this.accomodationList = response;
      this.dataSourceAcc.data = this.accomodationList
      console.log(response)
    })
    
  }
}
