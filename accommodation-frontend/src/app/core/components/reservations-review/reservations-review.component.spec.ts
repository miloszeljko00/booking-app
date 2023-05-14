import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReservationsReviewComponent } from './reservations-review.component';

describe('ReservationsReviewComponent', () => {
  let component: ReservationsReviewComponent;
  let fixture: ComponentFixture<ReservationsReviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReservationsReviewComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReservationsReviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
