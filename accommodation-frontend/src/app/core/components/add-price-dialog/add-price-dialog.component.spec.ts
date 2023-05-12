import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddPriceDialogComponent } from './add-price-dialog.component';

describe('AddPriceDialogComponent', () => {
  let component: AddPriceDialogComponent;
  let fixture: ComponentFixture<AddPriceDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddPriceDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddPriceDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
