import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderBookingComponent } from './order-booking.component';

describe('OrderBookingComponent', () => {
  let component: OrderBookingComponent;
  let fixture: ComponentFixture<OrderBookingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrderBookingComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrderBookingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
