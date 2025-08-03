import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {

  constructor(private _HttpClint:HttpClient) { }
  private apiUrl = environment.apiBaseUrl;

  addReview(CourseId:number){
    //  return this._HttpClint.get(`${this.apiUrl}/api/Reviews/${CourseId}`);

  }
}
