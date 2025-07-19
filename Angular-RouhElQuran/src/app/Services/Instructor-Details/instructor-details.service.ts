import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class InstructorDetailsService {

  constructor(private _HttpClint : HttpClient) { }
 private apiUrl = environment.apiBaseUrl;
    
 
      getInstructorDataById(instructorId: number){
       return this._HttpClint.get(`${this.apiUrl}/api/Instructor/${instructorId}`);
  
     }
     
}
