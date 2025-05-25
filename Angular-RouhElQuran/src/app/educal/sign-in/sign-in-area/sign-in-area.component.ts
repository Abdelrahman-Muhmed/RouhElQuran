import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/Services/auth.service';

@Component({
    selector: 'app-sign-in-area',
    templateUrl: './sign-in-area.component.html',
    styleUrls: ['./sign-in-area.component.scss'],
    standalone: false
})
export class SignInAreaComponent implements OnInit {
  constructor(private _authService: AuthService, private _router: Router) { }

  isLoading = false;
  ResponseMssage: any;

loginForm: FormGroup = new FormGroup({
  email: new FormControl(null, [Validators.required, Validators.email]),
  password: new FormControl(null , [Validators.required])
})

  ngOnInit(): void {
    this.loginForm.get('password')?.valueChanges.subscribe(() => {
      this.validatePassword();
    })
  }


  //Custome Validation
  validatePassword(): void {
    const passwordControl = this.loginForm.get('password');
    const value = passwordControl?.value;

    if (!value) {
      return;
    }

    const errors: any = {};

    if (!/(?=.*[a-z])/.test(value)) {
      errors.lowercase = 'Password must contain at least one lowercase letter';
    }
    if (!/(?=.*[A-Z])/.test(value)) {
      errors.uppercase = 'Password must contain at least one uppercase letter';
    }
    if (!/(?=.*\d)/.test(value)) {
      errors.number = 'Password must contain at least one number';
    }
    if (!/(?=.*[^\da-zA-Z])/.test(value)) {
      errors.special = 'Password must contain at least one special character';
    }
    if (!/.{6,20}/.test(value)) {
      errors.length = 'Password must be between 6 and 20 characters long';
    }

    if (Object.keys(errors).length > 0) {
      passwordControl?.setErrors(errors);
    } else {
      passwordControl?.setErrors(null);
    }
  }


  //Login
   LoginData() {
    this.isLoading = true;
    if(this.loginForm.valid) {
      this._authService.Login(this.loginForm.value).subscribe({
        next: async (data) => {
          this.ResponseMssage = data;
          localStorage.setItem('token', this.ResponseMssage.token)
          await this._authService.SaveUserLoginData();
          this._router.navigate(['/']);
          this.isLoading = false;
        },
        error: (err) => {
          this.ResponseMssage = err.message;
          this.isLoading = false;
        }
       });
    }
   this.loginForm.markAllAsTouched();
  }




}
