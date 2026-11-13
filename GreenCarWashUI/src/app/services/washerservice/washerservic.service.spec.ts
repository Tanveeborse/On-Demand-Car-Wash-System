import { TestBed } from '@angular/core/testing';

import { WasherservicService } from './washerservic.service';

describe('WasherservicService', () => {
  let service: WasherservicService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WasherservicService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
