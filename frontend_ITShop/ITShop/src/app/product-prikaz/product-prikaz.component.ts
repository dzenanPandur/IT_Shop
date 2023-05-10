import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Globals} from "../globals";
import {ActivatedRoute, Router} from "@angular/router";
import {ProductGetVM} from "../_models/ProductGetVM";

@Component({
  selector: 'app-product-prikaz',
  templateUrl: './product-prikaz.component.html',
  styleUrls: ['./product-prikaz.component.css']
})
export class ProductPrikazComponent implements OnInit {
  tableDataPictures: any;

  product:any;
  id:any;
  constructor(private httpClient: HttpClient, public globals: Globals,private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {

    this.route.params.subscribe(x=> {
        this.id = +x["id"];
        this.loadData();
        this.loadData2()
      }
    )
  }

  loadData() {
    this.httpClient.get(this.globals.serverAddress + "/Product/"+this.id)
      .subscribe({
        next: (value: any) => {
          this.product = value.data;

          //this.totalProduct=value.data.length();
          console.log(this.product);
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }});
  }
  loadData2() {
    this.httpClient.get(`${this.globals.serverAddress}/ProductPicture/${this.id}`)
      .subscribe({
        next: (value: any) => {
          this.tableDataPictures = value.data;
          console.log(this.tableDataPictures);
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }});
  }

}
