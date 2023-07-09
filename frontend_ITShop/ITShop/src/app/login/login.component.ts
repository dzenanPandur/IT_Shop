import { Component, OnInit } from '@angular/core';
import {Auth} from "../View models/Auth";
import {HttpClient} from "@angular/common/http";
import {Globals} from "../globals";
import {Router} from "@angular/router";
import { CookieService } from 'ngx-cookie';
import {AuthData} from "../View models/AuthData";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public authRequest: Auth;
  public TFAStatus: any;

  login(){

    this.http.post<AuthData>(this.globals.serverAddress + '/Auth', this.authRequest).subscribe(data=>{
      this.globals.authData=data;
      this.globals.isAuthenticated=true;
      const expireDate = new Date(new Date().getTime()+(1000*(data.tokenExpireDate-60)));
      this._cookieService.putObject('auth', data, {expires: expireDate})

      //this.globals.loggedUser=data.userId;

      if(data.roles.filter(((x:string) => 'Admin' === x))[0]!==undefined)
      {
        this.router.navigate(['admin']).then(()=>window.location.reload());
      }
      else if(data.roles.filter(((x:string) => 'Zaposlenik' === x))[0]!==undefined)
      {
        this.router.navigate(['/categories']).then(()=>window.location.reload());
      }
      else if(data.roles.filter(((x:string) => 'Kupac' === x))[0]!==undefined){
        this.router.navigate(['/']).then(()=>window.location.reload());
      }
      //window.location.reload();
    })
  }
  logout() {
    this._cookieService.remove('auth');
    this.globals.isAuthenticated=false;
    //window.location.reload();
    this.router.navigate(['']).then(()=>window.location.reload());
  }



  constructor(
    public http: HttpClient,
    public globals: Globals,
    public router: Router,
    public _cookieService: CookieService
  ) {
    this.authRequest=new Auth();
  }

  ngOnInit(): void {

  }
  cc(){
    console.log(this._cookieService.get('auth'));
    console.log(this.globals.isAuthenticated);
  }

  check2FA(){
    this.http.get<AuthData>(this.globals.serverAddress + '/Auth/check2FA?username=' + this.authRequest.username).subscribe(data=> {
      this.TFAStatus=data;
      console.log(this.TFAStatus);
    })
  }

  generateNewCode(username: string) {
    this.http.post(this.globals.serverAddress + '/Auth/generate-new-code?username=' + username, null).subscribe(data=> {
    })
  }
}
