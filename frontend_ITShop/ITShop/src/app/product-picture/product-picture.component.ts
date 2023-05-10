import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {ProductSnimiVM} from "../_models/ProductSnimiVM";
import {ProductGetVM} from "../_models/ProductGetVM";
import { HttpClient } from '@angular/common/http';
import {Globals} from "../globals";
import {ActivatedRoute, Router} from "@angular/router";
import {ProductPictureGetVM} from "../_models/ProductPictureGetVM";
import {ProductPictureSnimiVM} from "../_models/ProductPictureSnimiVM";

@Component({
  selector: 'app-product-picture',
  templateUrl: './product-picture.component.html',
  styleUrls: ['./product-picture.component.css']
})
export class ProductPictureComponent implements OnInit {
  tableData?: ProductPictureGetVM[];

  odabrani: any;
  showModal: boolean = false;

  productId?: number;

  constructor(private httpClient: HttpClient, public globals: Globals, private router: Router, private route: ActivatedRoute) {
    this.route.params.subscribe({next:(value)=>{
      this.productId = value['id'];
      this.loadData();
    }})
  }

  ngOnInit(): void { }

  loadData() {
    this.httpClient.get(`${this.globals.serverAddress}/ProductPicture/${this.productId}`)
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
  izbrisi(id: number) {
    if (!confirm("Da li ste sigurni da zelite pobrisati ovaj zapis?"))
      return;
    this.httpClient.delete(`${this.globals.serverAddress}/ProductPicture/${id}`)
      .subscribe({
        next: (value: any) => {
          this.loadData();
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }
      });
  }

  novaSlikaDugme() {
    this.showModal=true;
    this.odabrani = {
      productID: this.productId
    }
  }
}
