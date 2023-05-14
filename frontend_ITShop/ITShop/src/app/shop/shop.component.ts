import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Globals} from "../globals";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.css']
})
export class ShopComponent implements OnInit {

  constructor(private httpClient: HttpClient, public globals: Globals, private router: Router, private route: ActivatedRoute) {
    this.route.params.subscribe({
      next: (value) => {
        if ('category_id' in value) {
          this.categoryId = value['category_id'];
        }
        this.loadData();
      }
    })
  }

  tableData: any;
  p: number = 1;
  odabrani: any;
  //showModal: boolean = false;
  itemsPerPage: number = 8;
  totalItems: any;
  productId?: number;


  categoryId?: number;


  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.httpClient.get(this.globals.serverAddress + "/Product?items_per_page=" + this.itemsPerPage + "&page_number=" + this.p +
      (this.categoryId ? "&categoryId="+this.categoryId : ""))
      .subscribe({
        next: (value: any) => {

          this.tableData = value.data.dataItems;
          this.totalItems = value.data.totalCount;
          console.log(this.tableData);
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }
      });

  }

  cc() {
    this.loadData();
  }

  ProductPrikaz(x: any) {
    this.router.navigateByUrl('product-prikaz/' + x.id);
  }
}
