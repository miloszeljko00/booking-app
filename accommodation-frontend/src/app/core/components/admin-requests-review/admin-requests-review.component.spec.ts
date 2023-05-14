import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminRequestsReviewComponent } from './admin-requests-review.component';

describe('AdminRequestsReviewComponent', () => {
  let component: AdminRequestsReviewComponent;
  let fixture: ComponentFixture<AdminRequestsReviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminRequestsReviewComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminRequestsReviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
