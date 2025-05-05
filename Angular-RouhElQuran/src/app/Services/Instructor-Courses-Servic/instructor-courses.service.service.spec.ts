import { TestBed } from '@angular/core/testing';

import { InstructorCoursesServiceService } from './instructor-courses.service.service';

describe('InstructorCoursesServiceService', () => {
  let service: InstructorCoursesServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(InstructorCoursesServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
