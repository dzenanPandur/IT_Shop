import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Globals} from "../globals";
import {ActivatedRoute, Router} from "@angular/router";
import {CookieService} from "ngx-cookie";

@Component({
  selector: 'app-my-order-details',
  templateUrl: './my-order-details.component.html',
  styleUrls: ['./my-order-details.component.css']
})
export class MyOrderDetailsComponent implements OnInit {

  constructor(private httpClient: HttpClient,
              public globals: Globals,private router: Router,
              private route: ActivatedRoute,
              public _cookieService: CookieService) { }


  id:any;
  public user:any;
  order:any;
  orderItems:any;


  ngOnInit(): void {
    this.user=this._cookieService.getObject('auth');
    console.log(this.user);
    this.route.params.subscribe(x=> {
        this.id = +x["id"];
        this.loadData();

        }
    )
  }




   loadData() {
    this.httpClient.get(this.globals.serverAddress + "/Orders/"+this.id)
      .subscribe({
        next: (value: any) => {
          this.order = value.data;
          this.orderItems=this.order.orderItems;
          //this.totalProduct=value.data.length();
          console.log(this.order);
          console.log(this.orderItems);
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }});
  }


}
