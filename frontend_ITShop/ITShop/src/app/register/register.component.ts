import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Globals} from "../globals";
import {RegisterUser} from "../View models/RegisterUser";
import {AuthData} from "../View models/AuthData";
import {Router} from "@angular/router";
import {CookieService} from "ngx-cookie";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  isPasswordHidden: boolean = true;
  public registerRequest: RegisterUser;

  register(){
    this.http.post<any>(this.globals.serverAddress + '/User', this.registerRequest).subscribe(data=>{
      console.log(data);

      this.login(this.registerRequest.userName, this.registerRequest.password)
      console.log(this.registerRequest.userName, this.registerRequest.password)


    })
  }
  login(username: string, password: string){

    this.http.post<AuthData>(this.globals.serverAddress + '/Auth', {username: username, password: password}).subscribe(data=>{
      this.globals.authData=data;
      this.globals.isAuthenticated=true;
      const expireDate = new Date(new Date().getTime()+(1000*(data.tokenExpireDate-60)));
      this._cookieService.putObject('auth', data, {expires: expireDate})
      window.location.reload();
    })
  }


  constructor(
    public http: HttpClient,
    public globals: Globals,
    public router: Router,
    public _cookieService: CookieService
  ) {

    this.registerRequest= new RegisterUser();

  }

  ngOnInit(): void {

  }

}
