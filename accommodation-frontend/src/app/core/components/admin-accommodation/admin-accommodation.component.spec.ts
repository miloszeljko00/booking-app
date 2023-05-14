import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAccommodationComponent } from './admin-accommodation.component';

describe('AdminAccommodationComponent', () => {
  let component: AdminAccommodationComponent;
  let fixture: ComponentFixture<AdminAccommodationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminAccommodationComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminAccommodationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
