import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LabelApiComponent } from './label-api.component';

describe('LabelApiComponent', () => {
  let component: LabelApiComponent;
  let fixture: ComponentFixture<LabelApiComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LabelApiComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LabelApiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
