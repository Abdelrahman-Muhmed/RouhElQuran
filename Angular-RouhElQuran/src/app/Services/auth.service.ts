import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
@Injectable({
  providedIn: 'root',

})
export class AuthService {
  constructor(private _HttpClient: HttpClient) { }

  RegisterAccount(registerForm: Object):Observable<any>{

  // return this._HttpClient.post(`${this.BaseUrl}Account/Register`,{registerForm});

  return this._HttpClient.post('/api/Account/Register', registerForm);
  }
}
