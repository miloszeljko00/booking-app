import { Component } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { HostGrading } from 'src/app/api/model/hostGrading';
import { User } from '../../keycloak/model/user';
import { MatDialog } from '@angular/material/dialog';
import { GradingService } from 'src/app/api/api/grading.service';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../keycloak/auth.service';
import { DialogComponent } from '../dialog/dialog.component';
import { CreateHostGradingDialogComponent } from '../create-host-grading-dialog/create-host-grading-dialog.component';
import { CreateHostGrading } from 'src/app/api/model/createHostGrading';
import { UpdateHostGradingDialogComponent } from '../update-host-grading-dialog/update-host-grading-dialog.component';
import { UpdateHostGrading } from 'src/app/api/model/updateHostGrading';
import { AccommodationGrading } from 'src/app/api/model/accommodationGrading';
import { CreateAccommodationGradingDialogComponent } from '../create-accommodation-grading-dialog/create-accommodation-grading-dialog.component';
import { CreateAccommodationGrading } from 'src/app/api/model/createAccommodationGrading';
import { UpdateAccommodationGradingDialogComponent } from '../update-accommodation-grading-dialog/update-accommodation-grading-dialog.component';
import { UpdateAccommodationGrading } from 'src/app/api/model/updateAccommodationGrading';

@Component({
  selector: 'app-grading',
  templateUrl: './grading.component.html',
  styleUrls: ['./grading.component.scss']
})
export class GradingComponent {
  dataSourceHostGrading = new MatTableDataSource<HostGrading>();
  displayedColumnsHostGrading = ['guest', 'host', 'date', 'grade', 'averageGrade', 'change', 'delete'];

  dataSourceAccGrading = new MatTableDataSource<HostGrading>();
  displayedColumnsAccGrading = ['accGuest', 'accommodation', 'accDate', 'accGrade', 'accAverageGrade', 'accChange', 'accDelete'];
                            
  gradeList!: HostGrading[];
  grade!: HostGrading;
  accGradeList!: AccommodationGrading[];
  accGrade!: AccommodationGrading;
  user!: User | null;

  constructor(public dialog: MatDialog,private gradingService: GradingService, private toastr : ToastrService, private authService: AuthService) {
    this.user = this.authService.getUser()
  }

  ngOnInit() {
    this.getAllHostGrades();
    this.getAllAccommodationGrades();
  }

  openCreateHostGradeDialog(){
    const dialogRef = this.dialog.open(CreateHostGradingDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if(result){
        let hostGrade : CreateHostGrading = { hostEmail: result.host, guestEmail: this.user?.email ?? '', grade: result.grade}
        this.gradingService.createHostGrading(hostGrade).subscribe(() => {
          this.showSuccess('Successfully created grade for host');
          this.getAllHostGrades();
        })
      }
    });
  }

  openCreateAccGradeDialog(){
    const dialogRef = this.dialog.open(CreateAccommodationGradingDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if(result){
        let accGrade : CreateAccommodationGrading = { hostEmail: result.accommodation.hostEmail, accommodationName: result.accommodation.name, guestEmail: this.user?.email ?? '', grade: result.grade}
        this.gradingService.createAccommodationGrading(accGrade).subscribe(() => {
          this.showSuccess('Successfully created grade for accommodation');
          this.getAllAccommodationGrades();
        })
      }
    });
  }

  openChangeHostGradeDialog(grade: HostGrading){
    const dialogRef = this.dialog.open(UpdateHostGradingDialogComponent,
      {
        data: grade.grade,
      });

    dialogRef.afterClosed().subscribe(result => {
      if(result){
        let hostGrade : UpdateHostGrading = { id: grade.id, grade: result}
        this.gradingService.updateHostGrading(hostGrade).subscribe(() => {
          this.showSuccess('Successfully changed grade for host');
          this.getAllHostGrades();
        })
      }
    });
  }

  openChangeAccGradeDialog(grade: AccommodationGrading){
    const dialogRef = this.dialog.open(UpdateAccommodationGradingDialogComponent,
      {
        data: grade.grade,
      });

    dialogRef.afterClosed().subscribe(result => {
      if(result){
        let accGrade : UpdateAccommodationGrading = { id: grade.id, grade: result}
        this.gradingService.updateAccommodationGrading(accGrade).subscribe(() => {
          this.showSuccess('Successfully changed grade for accommodation');
          this.getAllAccommodationGrades();
        })
      }
    });
  }

  deleteHostGrade(grade: HostGrading){
      this.gradingService.deleteHostGrading(grade.id).subscribe(() => {
        this.showSuccess('Successfully deleted grade for host');
        this.getAllHostGrades();
      })
  }

  deleteAccGrade(grade: AccommodationGrading){
    this.gradingService.deleteAccommodationGrading(grade.id).subscribe(() => {
      this.showSuccess('Successfully deleted grade for accommodation');
      this.getAllAccommodationGrades();
    })
}

  private getAllHostGrades(){
    this.gradingService.getHostGrading().subscribe((response: any) => {
      this.gradeList = response;
      this.dataSourceHostGrading.data = this.gradeList;
    });
  }

  private getAllAccommodationGrades(){
    this.gradingService.getAccommodationGrading().subscribe((response: any) => {
      this.accGradeList = response;
      this.dataSourceAccGrading.data = this.accGradeList;
    });
  }

  check(grade: HostGrading){
    if(grade.guestEmail != this.user?.email)
      return true;
    return false;
  }

  checkAccGrade(grade: AccommodationGrading){
    if(grade.guestEmail != this.user?.email)
      return true;
    return false;
  }

  showSuccess(message: string) {
    this.toastr.success(message, 'Booking application');
  }
  showError(message: string) {
    this.toastr.error(message, 'Booking application');
  }

}
