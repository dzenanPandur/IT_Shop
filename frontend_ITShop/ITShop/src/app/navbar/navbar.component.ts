import {Component, OnInit, ElementRef, ViewChild, Input,Output} from '@angular/core';
import {Globals} from "../globals";
import { CookieService } from 'ngx-cookie';
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {LoggedUserInfo} from "../View models/LoggedUserInfo";
import {TranslateService} from "@ngx-translate/core";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  public loggedUser: LoggedUserInfo;
  public fullName: any;
  tableData:any;
  subscribedUser_data: any;
  is_subscribed: boolean = false;

  constructor(
    public globals: Globals,
    public _cookieService: CookieService,
    public router: Router,
    private httpClient: HttpClient,
    public translate: TranslateService
  ) {
    this.loggedUser= new LoggedUserInfo();
    translate.addLangs(['en','bs'])
    translate.setDefaultLang('en')
  }
  switchLang(lang:string){
    this.translate.use(lang)
  }

  ngOnInit(): void {
    this.fullName =this._cookieService.getObject('auth');
    this.loadData();

    this.loggedUser={
      userID: this.fullName.userId,
      isSubscribed: true
    }
    //this.getSubscriptionById()
    console.log("This is the status of the subscription: " + this.is_subscribed);
    console.log("User subscription: "+ this.subscribedUser_data)
  }

  loadData() {
    this.httpClient.get(this.globals.serverAddress + "/ProductCategory")
      .subscribe({
        next: (value: any) => {
          this.tableData = value.data;
          console.log(this.tableData);
          //if(this.subscribedUser_data.data != null)
          this.getSubscriptionById()
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }});
  }

  logout() {
    this._cookieService.remove('auth');
    this.globals.isAuthenticated=false;
    //window.location.reload();
    this.router.navigate(['']).then(()=>window.location.reload());
  }
  cc(){
    console.log(this._cookieService.get('auth'));
    console.log(this.globals.isAuthenticated);
    console.log(this.globals.authData);
  }
  roles(role: string): boolean{
    return this.globals.authData.roles.filter(((x:string) => role === x))[0] !== undefined;

  }

  CartItems() {
    this.router.navigateByUrl("cart")
  }



  getSubscriptionById(){
    this.httpClient.get(this.globals.serverAddress + "/Subscription/get-subscriptions-by-id?id=" + this.fullName.userId)
      .subscribe(data=>{
        this.subscribedUser_data=data;
        console.log(this.subscribedUser_data);
      })
  }


}
