import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WasherorderHistoryComponent } from './washerorder-history.component';

describe('WasherorderHistoryComponent', () => {
  let component: WasherorderHistoryComponent;
  let fixture: ComponentFixture<WasherorderHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WasherorderHistoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WasherorderHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
