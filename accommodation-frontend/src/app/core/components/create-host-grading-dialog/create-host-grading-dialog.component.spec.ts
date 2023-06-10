import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateHostGradingDialogComponent } from './create-host-grading-dialog.component';

describe('CreateHostGradingDialogComponent', () => {
  let component: CreateHostGradingDialogComponent;
  let fixture: ComponentFixture<CreateHostGradingDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateHostGradingDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateHostGradingDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
