import { EventEmitter, Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root',

})
export class AuthService {
  constructor(private _HttpClient: HttpClient) { }
  UserData: any;
  authStatus = new EventEmitter<boolean>();

   private apiUrl = environment.apiBaseUrl;
  //Register
  RegisterAccount(registerForm: any):Observable<any>{

    return this._HttpClient.get(`${this.apiUrl}/api/Account/Register`, {params: registerForm});

  // return this._HttpClient.post(`${this.BaseUrl}Account/Register`,{registerForm});
  // return this._HttpClient.post('/api/Account/Register', registerForm);
  }
//Login
  Login(loginForm:any):Observable<any>{
    return this._HttpClient.get(`${this.apiUrl}/api/Account/Login`, {params: loginForm});

    // return this._HttpClient.post('/api/Account/Login',loginForm );

  }

  async SaveUserLoginData() {
    if (localStorage.getItem('token') != null) {
      let token: any = await localStorage.getItem('token');
      this.UserData = await jwtDecode(token);
      this.authStatus.emit(true);
    }
  }
}


