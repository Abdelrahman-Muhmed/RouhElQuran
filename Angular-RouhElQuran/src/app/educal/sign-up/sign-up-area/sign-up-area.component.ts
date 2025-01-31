import { AuthService } from './../../../Services/auth.service';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormControlOptions, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'app-sign-up-area',
    templateUrl: './sign-up-area.component.html',
    styleUrls: ['./sign-up-area.component.scss'],
    standalone: false,



})
export class SignUpAreaComponent implements OnInit {

  constructor(private _authService: AuthService) { }
  isLoading = false;
  ResponseMssage: any;

  registerForm: FormGroup = new FormGroup({
    firstName: new FormControl(null, [Validators.required, Validators.minLength(2), Validators.maxLength(20)]),
    lastName: new FormControl(null, [Validators.required, Validators.minLength(2), Validators.maxLength(20)]),
    country: new FormControl(null, [Validators.required, Validators.minLength(3)]),
    email: new FormControl(null, [Validators.required, Validators.email]),
    password: new FormControl(null, [Validators.required]),
    confirmPassword: new FormControl(null, [Validators.required]),
    phoneNumber: new FormControl(null, [Validators.required]),
    language: new FormControl(null, [Validators.required]),
    personalImage: new FormControl(null),
  } );



  ngOnInit(): void {
    this.registerForm.get('password')?.valueChanges.subscribe(() => {
      this.validatePassword();
    });

    this.registerForm.get('confirmPassword')?.valueChanges.subscribe(() => {
      this.confirmPassword();
    });
  }


//Custome Validation For Confirm Password
validatePassword(): void {
  const passwordControl = this.registerForm.get('password');
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
confirmPassword(){
  var password = this.registerForm.get('password')?.value;
  var confirmPassword = this.registerForm.get('confirmPassword')?.value;

  if(confirmPassword == '')
      this.registerForm.get('confirmPassword')?.setErrors({required:true});
  else if(confirmPassword != password)
    this.registerForm.get('confirmPassword')?.setErrors({mismatch:true});

}

//Register 
  RegisterData() {
    if (this.registerForm.valid) {
      this.isLoading = true;
      // console.log(this.registerForm.value);

      // debugger;
      this._authService.RegisterAccount(this.registerForm.value).subscribe({
        next: (data) => {
          this.ResponseMssage = data.message;
          this.isLoading = false;
        },
        error: (err) => {
          this.ResponseMssage = err.error;
          console.log(err.error);

          this.isLoading = false;
        },
      });
    }
    this.registerForm.markAllAsTouched();
  }

}

