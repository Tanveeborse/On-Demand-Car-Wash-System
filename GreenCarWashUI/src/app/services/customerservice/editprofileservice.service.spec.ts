import { TestBed } from '@angular/core/testing';

import { EditprofileserviceService } from './editprofileservice.service';

describe('EditprofileserviceService', () => {
  let service: EditprofileserviceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EditprofileserviceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
