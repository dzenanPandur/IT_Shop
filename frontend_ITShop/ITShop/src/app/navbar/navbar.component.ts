import { Component, OnInit } from '@angular/core';
import {Globals} from "../globals";
import { CookieService } from 'ngx-cookie';
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";


@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  public fullName: any;
  tableData:any;

  constructor(
    public globals: Globals,
    public _cookieService: CookieService,
    public router: Router,
    private httpClient: HttpClient
  ) {

  }

  ngOnInit(): void {
    this.fullName =this._cookieService.getObject('auth');
    this.loadData();
  }
  loadData() {
    this.httpClient.get(this.globals.serverAddress + "/ProductCategory")
      .subscribe({
        next: (value: any) => {
          this.tableData = value.data;
          console.log(this.tableData);
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
}
