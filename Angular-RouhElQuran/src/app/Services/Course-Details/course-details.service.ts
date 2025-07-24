import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class CourseDetailsService {

 constructor(private _HttpClint : HttpClient) { }
  private apiUrl = environment.apiBaseUrl;
     
  
       getCourseDataById(CourseId: number){
        return this._HttpClint.get(`${this.apiUrl}/api/Courses/${CourseId}`);
   
      }
}
