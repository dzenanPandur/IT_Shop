import { Component, OnInit } from '@angular/core';
import {Auth} from "../View models/Auth";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {Globals} from "../globals";
import {Router} from "@angular/router";
import { CookieService } from 'ngx-cookie';
import {AuthData} from "../View models/AuthData";
import {SignalrService} from "../signalr.service";
import {IndividualConfig, ToastrService} from "ngx-toastr";
import * as signalR from "@aspnet/signalr";
import {interval} from "rxjs";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  public authRequest: Auth;
  public TFAStatus: any;
  message:any;
  errorResponse: HttpErrorResponse | null = null;
  hubConnection!: signalR.HubConnection;
  timer: number = 0;
  intervalSubscription: any;
  codeExpired: boolean = false;
  isButtonDisabled: boolean = false;
  login(){
    this.signalrService.toastr.info("Logging in attempt...")
    this.codeExpired = false;
    this.http.post<AuthData>(this.globals.serverAddress + '/Auth', this.authRequest).subscribe(
      data=>{
      this.globals.authData=data;
      this.globals.isAuthenticated=true;
      const expireDate = new Date(new Date().getTime()+(1000*(data.tokenExpireDate-60)));
      this._cookieService.putObject('auth', data, {expires: expireDate})

      //this.globals.loggedUser=data.userId;this.SendSignInNotification();
      if(data.roles.filter(((x:string) => 'Admin' === x))[0]!==undefined)
      {
        this.router.navigate(['admin']).then(()=>window.location.reload());
      }
      else if(data.roles.filter(((x:string) => 'Zaposlenik' === x))[0]!==undefined)
      {
        this.router.navigate(['/employee']).then(()=>window.location.reload());
      }
      else if(data.roles.filter(((x:string) => 'Kupac' === x))[0]!==undefined){
        this.router.navigate(['/']).then(()=>window.location.reload());
      }

      //window.location.reload();
    },
      (error: HttpErrorResponse) => {
        this.errorResponse = error;
        this.codeExpired = false;
    }
    )
  }
  logout() {

      this._cookieService.remove('auth');
      this.globals.isAuthenticated = false;
      //window.location.reload();
      this.router.navigate(['']).then(() => window.location.reload());

  }
  startTimer() {
    if (!this.intervalSubscription) {
      this.timer = 60;

      this.intervalSubscription = interval(1000).subscribe(() => {
        this.timer--;
        if (this.timer === 0) {
          this.intervalSubscription.unsubscribe();
          this.intervalSubscription = null;
          this.codeExpired = true;
        }
      });
    }
  }


  resetTimer() {
    if(this.timer>0) {
      this.intervalSubscription.unsubscribe();
      this.intervalSubscription = null;
    }
    this.timer = 60;

    this.intervalSubscription = interval(1000).subscribe(() => {
      this.timer--;
      if (this.timer === 0) {
        this.intervalSubscription.unsubscribe();
        this.intervalSubscription = null;
        this.codeExpired = true;
      }
    });
  }

  constructor(
    public http: HttpClient,
    public globals: Globals,
    public router: Router,
    public _cookieService: CookieService,
    public signalrService: SignalrService,
    private toastr: ToastrService,
  ) {
    this.authRequest=new Auth();
  }

 /* async SendSignInNotification() {
    console.log("SendSignInNotification");
    this.message="Signing in"

        this.toastr.success('', this.message, {
          timeOut: 2500,
          progressBar: true,
          closeButton: true,
        } as IndividualConfig);
        console.log();



    console.log("This is the final prompt");
  }*/
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
    this.codeExpired = false;
    this.isButtonDisabled = true;
    setTimeout(() => {
      this.isButtonDisabled = false;
    }, 5000);
    this.http.post(this.globals.serverAddress + '/Auth/generate-new-code?username=' + username, null).subscribe(data=> {

    })
  }
}
