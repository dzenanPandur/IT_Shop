import { Component, OnInit } from '@angular/core';
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {Globals} from "../globals";
import {RegisterUser} from "../View models/RegisterUser";
import {AuthData} from "../View models/AuthData";
import {Router} from "@angular/router";
import {CookieService} from "ngx-cookie";
import {SignalrService} from "../signalr.service";
import {IndividualConfig} from "ngx-toastr";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  isPasswordHidden: boolean = true;
  isPasswordConfirmHidden: boolean = true;
  public registerRequest: RegisterUser;
  isButtonDisabled: boolean=true;
  errorResponse: HttpErrorResponse | null = null;
  confirmPassword: string = "";
  register(){
    this.isButtonDisabled = true;
    this.http.post<any>(this.globals.serverAddress + '/User', this.registerRequest).subscribe(data=>{
      console.log(data);
        var message="Successful registration";
        this.signalrService.toastr.success('', message, {
          timeOut: 2500,
          progressBar: true,
          closeButton: true,
        } as IndividualConfig);
      this.login(this.registerRequest.userName, this.registerRequest.password, "")
      console.log(this.registerRequest.userName, this.registerRequest.password)

    },
      (error: HttpErrorResponse) => {
        this.isButtonDisabled = false;
        this.errorResponse = error;
      }
    )
  }
  login(username: string, password: string, verificationCode: string){

    this.http.post<AuthData>(this.globals.serverAddress + '/Auth', {username: username, password: password, verificationCode: verificationCode}).subscribe(data=>{
      this.globals.authData=data;
      this.globals.isAuthenticated=true;
      const expireDate = new Date(new Date().getTime()+(1000*(data.tokenExpireDate-60)));
      this._cookieService.putObject('auth', data, {expires: expireDate})
      window.location.reload();
    })
  }
  checkForm() {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    const firstNameValid = this.registerRequest.firstName.length >= 2;
    const lastNameValid = this.registerRequest.lastName.length >= 2;
    const emailValid = emailRegex.test(this.registerRequest.email);
    const userNameValid = this.registerRequest.userName.length >= 5;
    const phoneNumberValid = this.registerRequest.phoneNumber.length >= 7;
    const passwordValid = this.registerRequest.password.length >= 6;
    const confirmPasswordValid = this.confirmPassword === this.registerRequest.password;

    this.isButtonDisabled = !(firstNameValid && lastNameValid && emailValid && userNameValid && phoneNumberValid && passwordValid && confirmPasswordValid);
  }

  constructor(
    public http: HttpClient,
    public globals: Globals,
    public router: Router,
    public _cookieService: CookieService,
    public signalrService:SignalrService
  ) {

    this.registerRequest= new RegisterUser();

  }

  ngOnInit(): void {

  }

}
