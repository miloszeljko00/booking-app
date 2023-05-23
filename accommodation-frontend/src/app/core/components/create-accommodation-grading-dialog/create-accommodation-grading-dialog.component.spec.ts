import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateAccommodationGradingDialogComponent } from './create-accommodation-grading-dialog.component';

describe('CreateAccommodationGradingDialogComponent', () => {
  let component: CreateAccommodationGradingDialogComponent;
  let fixture: ComponentFixture<CreateAccommodationGradingDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateAccommodationGradingDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateAccommodationGradingDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
