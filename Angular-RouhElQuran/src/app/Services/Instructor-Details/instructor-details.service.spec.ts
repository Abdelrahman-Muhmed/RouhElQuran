import { TestBed } from '@angular/core/testing';

import { InstructorDetailsService } from './instructor-details.service';

describe('InstructorDetailsService', () => {
  let service: InstructorDetailsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(InstructorDetailsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
