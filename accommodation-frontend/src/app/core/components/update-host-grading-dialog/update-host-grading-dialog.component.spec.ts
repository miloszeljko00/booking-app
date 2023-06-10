import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateHostGradingDialogComponent } from './update-host-grading-dialog.component';

describe('UpdateHostGradingDialogComponent', () => {
  let component: UpdateHostGradingDialogComponent;
  let fixture: ComponentFixture<UpdateHostGradingDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UpdateHostGradingDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateHostGradingDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
