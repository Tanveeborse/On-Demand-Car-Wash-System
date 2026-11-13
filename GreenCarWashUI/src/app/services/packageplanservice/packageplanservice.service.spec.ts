import { TestBed } from '@angular/core/testing';

import { PackageplanserviceService } from './packageplanservice.service';

describe('PackageplanserviceService', () => {
  let service: PackageplanserviceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PackageplanserviceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
