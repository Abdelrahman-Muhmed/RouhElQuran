import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class InstructorCoursesServiceService {

  constructor(private _HttpClient : HttpClient) { }

  getAllInstructorCourses() {
    return this._HttpClient.get('/api/InstructorCourses/GetAll');
  }
}

