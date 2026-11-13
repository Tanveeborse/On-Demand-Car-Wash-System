import { TestBed } from '@angular/core/testing';

import { WashersService } from './washers.service';

describe('WashersService', () => {
  let service: WashersService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WashersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
