import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Globals} from "../globals";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {

  constructor(private httpClient: HttpClient, public globals: Globals, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadData();
  }
  itemsPerPage: number = 2;
  totalItems: any;
  tableData: any;
  p: number = 1;
  totalTotalPrice:any;

  loadData() {
    this.httpClient.get(this.globals.serverAddress + "/CartItems?items_per_page=" + this.itemsPerPage + "&page_number=" + this.p)
      .subscribe({
        next: (value: any) => {
          /* if(this.filter_productName!=null){

             this.itemsPerPage=this.totalItems;
             this.loadData();
           }*/
          this.tableData = value.data;

          this.totalItems = this.tableData.length;
          this.totalTotalPrice = this.tableData.reduce((total: number, product: any) => total + product.totalPrice, 0);
          console.log(this.tableData);
          console.log(this.totalItems);
          console.log(this.totalTotalPrice);
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }
      });

  }

  cc() {
    /*if(this.filter_productName!=null){

      this.itemsPerPage=this.totalItems;
      this.loadData();
    }*/
    this.loadData();
  }


  UkloniIzkorpe(id:number) {
    if(!confirm("Da li ste sigurni da zelite ukloniti proizvod iz korpe?"))
      return;

    this.httpClient.delete(this.globals.serverAddress + '/CartItems/'+id)
      .subscribe({
        next: (value: any) => {
          this.loadData();
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }});
  }

  UkloniteSve() {
    if(!confirm("Da li ste sigurni da zelite ukloniti sve proizvode iz korpe?"))
      return;
    this.tableData.forEach((item: { id: number; }) => {
      // Perform operations on each item
      this.httpClient.delete(this.globals.serverAddress + '/CartItems/'+item.id)
        .subscribe({
          next: (value: any) => {
            this.loadData();
          },
          error: (err: any) => {
            alert("error");
            console.log(err);
          }});
    });

  }
}
