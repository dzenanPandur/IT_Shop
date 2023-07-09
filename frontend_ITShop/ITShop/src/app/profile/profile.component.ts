import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Globals} from "../globals";
import {Router} from "@angular/router";
import {CookieService} from "ngx-cookie";
@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  public TFAStatus: any;
  fullName: any;
  username: string= "";
  constructor(
    public http: HttpClient,
    public globals: Globals,
    public router: Router,
    public _cookieService: CookieService
  ) { }

  ngOnInit(): void {
    this.fullName =this._cookieService.getObject('auth');
    this.username=this.fullName.username;
    console.log((this.fullName));
    this.check2FA();
  }

  check2FA(){
    this.http.get(this.globals.serverAddress + '/Auth/check2FA?username=' + this.username).subscribe(data=> {
      this.TFAStatus=data;
      console.log(this.TFAStatus);
    })
  }
  update2FA(status: boolean){
    this.http.post(this.globals.serverAddress + '/Auth/update-2fa?username=' + this.username + "&status="+status, null).subscribe(data=> {

    })
    this.TFAStatus = status;
  }
}
