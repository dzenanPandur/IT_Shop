import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {ProductSnimiVM} from "../_models/ProductSnimiVM";
import {ProductGetVM} from "../_models/ProductGetVM";
import { HttpClient } from '@angular/common/http';
import {Globals} from "../globals";
import {Router} from "@angular/router";


@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {
  tableData?: any;
  p: number=1;
  odabrani: any;
  showModal: boolean = false;
  itemsPerPage: number=6;
  totalItems: any;
  //totalProduct:any;
  //@Output() loadCategories = new EventEmitter<void>();

  constructor(private httpClient: HttpClient, public globals: Globals, private route: Router) { }

  ngOnInit(): void {
    this.loadData();
  }//



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
  izbrisi(id: number) {
    if (!confirm("Da li ste sigurni da zelite pobrisati ovaj zapis?"))
      return;
    this.httpClient.delete(this.globals.serverAddress + '/Product/${id}')
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

  noviProizvodDugme() {
    this.showModal=true;
    this.odabrani={
      id:0,
      discountID:1,
      categoryID:1,
      inventoryID:1
    }
  }

  otvoriSlike(id: number) {
    this.route.navigateByUrl(`product-pictures/${id}`);

  }

  cc() {
    this.loadData();
    //console.log(this.p);
  }
}
