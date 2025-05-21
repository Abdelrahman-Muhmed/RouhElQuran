import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.prod';


@Injectable({
  providedIn: 'root'
})
export class InstructorCoursesServiceService {

  constructor(private _HttpClient : HttpClient) { }

   private apiUrl = environment.apiBaseUrl;

 //For Production
  // getAllInstructorCourses() {
  //   return this._HttpClient.get('/api/InstructorCourses/GetAll');
  // }

 getAllInstructorCourses() {
    return this._HttpClient.get(`${this.apiUrl}/api/InstructorCourses/GetAll`);
  }
}

