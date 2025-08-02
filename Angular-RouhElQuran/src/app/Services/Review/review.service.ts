import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {

  constructor(private _HttpClint:HttpClient) { }
  private apiUrl = environment.apiBaseUrl;

  addReview(reviewData:any){
     return this._HttpClint.post(`${this.apiUrl}/api/Reviews/AddReview`, reviewData);

  }
}
