import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CoursesService {


  constructor(private _HttpClient: HttpClient) { }
  private apiUrl = environment.apiBaseUrl;
  getAllCourses() {
    // return this._HttpClient.get('/api/Courses/GetAll');

     return this._HttpClient.get(`${this.apiUrl}/api/Instructor/GetAll`);
  }

  bookFreeCourse(courseId: number)
  {
    const url = `/api/FreeSession?CourseID=${courseId}`;
    return this._HttpClient.post(url, null);
  }
}
