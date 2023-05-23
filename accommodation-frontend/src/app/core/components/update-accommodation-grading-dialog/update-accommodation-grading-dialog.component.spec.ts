import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateAccommodationGradingDialogComponent } from './update-accommodation-grading-dialog.component';

describe('UpdateAccommodationGradingDialogComponent', () => {
  let component: UpdateAccommodationGradingDialogComponent;
  let fixture: ComponentFixture<UpdateAccommodationGradingDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UpdateAccommodationGradingDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateAccommodationGradingDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
