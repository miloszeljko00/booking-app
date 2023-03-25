import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewFlightsComponent } from './review-flights.component';

describe('ReviewFlightsComponent', () => {
  let component: ReviewFlightsComponent;
  let fixture: ComponentFixture<ReviewFlightsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReviewFlightsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReviewFlightsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
