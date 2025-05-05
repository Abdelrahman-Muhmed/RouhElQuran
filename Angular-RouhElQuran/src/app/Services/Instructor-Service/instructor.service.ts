import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class InstructorService {

  constructor(private _HttpClint : HttpClient) { }

  getInstructorData(){
     return this._HttpClint.get('/api/Instructor/GetAll');

   }
}
