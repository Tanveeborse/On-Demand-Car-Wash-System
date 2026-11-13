import { TestBed } from '@angular/core/testing';

import { CustomerOrderaddonService } from './customer-orderaddon.service';

describe('CustomerOrderaddonService', () => {
  let service: CustomerOrderaddonService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CustomerOrderaddonService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
