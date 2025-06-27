import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class InstructorService {

  constructor(private _HttpClint : HttpClient) { }
  private apiUrl = environment.apiBaseUrl;
  // getInstructorData(){
  //    return this._HttpClint.get('/api/Instructor/GetAll');

  //  }
    getInstructorData(){
     return this._HttpClint.get(`${this.apiUrl}/api/Instructor/GetAll`);

   }
}
