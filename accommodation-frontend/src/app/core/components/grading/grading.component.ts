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

@Component({
  selector: 'app-grading',
  templateUrl: './grading.component.html',
  styleUrls: ['./grading.component.scss']
})
export class GradingComponent {
  dataSourceHostGrading = new MatTableDataSource<HostGrading>();
  displayedColumnsHostGrading = ['guest', 'host', 'date', 'grade', 'averageGrade', 'change', 'delete'];
                            
  gradeList!: HostGrading[];
  grade!: HostGrading;
  user!: User | null;

  constructor(public dialog: MatDialog,private gradingService: GradingService, private toastr : ToastrService, private authService: AuthService) {
    this.user = this.authService.getUser()
  }

  ngOnInit() {
    this.gradingService.getHostGrading().subscribe((response: any) => {
      this.gradeList = response;
      this.dataSourceHostGrading.data = this.gradeList
    })
  }

  openDialog(){
    const dialogRef = this.dialog.open(CreateHostGradingDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if(result){
        let hostGrade : CreateHostGrading = { hostEmail: result.host, guestEmail: this.user?.email ?? '', grade: result.grade}
        this.gradingService.createHostGrading(hostGrade).subscribe(() => {
          this.gradingService.getHostGrading().subscribe((response: any) => {
            this.gradeList = response;
            this.dataSourceHostGrading.data = this.gradeList
          })
        })
      }
    });
  }

}
