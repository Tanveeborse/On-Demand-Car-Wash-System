import { TestBed } from '@angular/core/testing';

import { EditProfileServiceService } from './edit-profile-service.service';

describe('EditProfileServiceService', () => {
  let service: EditProfileServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EditProfileServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
