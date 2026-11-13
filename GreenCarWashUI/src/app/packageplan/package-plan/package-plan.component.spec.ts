import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PackagePlanComponent } from './package-plan.component';

describe('PackagePlanComponent', () => {
  let component: PackagePlanComponent;
  let fixture: ComponentFixture<PackagePlanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PackagePlanComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PackagePlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
