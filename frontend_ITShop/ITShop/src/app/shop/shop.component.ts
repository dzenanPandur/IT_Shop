import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Globals} from "../globals";
import {Router} from "@angular/router";
import {ProductPictureGetVM} from "../_models/ProductPictureGetVM";

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.css']
})
export class ShopComponent implements OnInit {

  constructor(private httpClient: HttpClient, public globals: Globals, private route: Router) { }
  tableData?: any;
  tableDataPictures?: any;
  p: number=1;
  odabrani: any;
  //showModal: boolean = false;
  itemsPerPage: number=8;
  totalItems: any;
  productId?: number;


  ngOnInit(): void {
    this.loadData();
    this.loadDataPictures();
  }
  loadDataPictures() {
    this.httpClient.get(`${this.globals.serverAddress}/ProductPicture/${23}`)
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
  loadDataPictures2() {
    for (const product of this.tableData) {
      this.httpClient.get(`${this.globals.serverAddress}/ProductPicture/${product.id}`)
        .subscribe({
          next: (value: any) => {
            product.pictures = value.data;
          },
          error: (err: any) => {
            console.log(`Error loading pictures for product ID ${product.id}:`, err);
          }
        });
    }
  }

  loadData() {
    this.httpClient.get(this.globals.serverAddress + "/Product?items_per_page=" + this.itemsPerPage + "&page_number="+this.p)
      .subscribe({
        next: (value: any) => {
          this.tableData = value.data.dataItems;
          this.totalItems= value.data.totalCount;

          //this.totalProduct=value.data.length();
          console.log(this.tableData);
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }});
  }
  cc() {
    this.loadData();
    this.loadDataPictures();
    //console.log(this.p);
  }

  ProductPrikaz(x: any) {
    this.route.navigateByUrl('product-prikaz/'+x.id);
  }
}
