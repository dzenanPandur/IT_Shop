import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {ProductCategorySnimiVM} from "../_models/ProductCategorySnimiVM";

import {ProductSnimiVM} from "../_models/ProductSnimiVM";
import {HttpClient} from "@angular/common/http";
import {Globals} from "../globals";
import {ProdutCategoryGetVM} from "../_models/ProdutCategoryGetVM";
import {ProductDiscountGetVM} from "../_models/ProductDiscountGetVM";
import {ProductDiscountSnimiVM} from "../_models/ProductDiscountSnimiVM";
import {ProductInventoryGetVM} from "../_models/ProductInventoryGetVM";

@Component({
  selector: 'app-product-snimi',
  templateUrl: './product-snimi.component.html',
  styleUrls: ['./product-snimi.component.css']
})
export class ProductSnimiComponent implements OnInit {
  @Input() odabrani: ProductSnimiVM = {};
  @Input() prikazi?: boolean;

  @Output() closeModal = new EventEmitter<void>();
  @Output() reloadData = new EventEmitter<void>();
  cmbDataCategory?: ProdutCategoryGetVM[];
  cmbDataDiscount?: ProductDiscountGetVM[];
  cmbDataInventory?: ProductInventoryGetVM[];
  constructor(private httpClient: HttpClient, public globals: Globals) { }

  ngOnInit(): void {
    this.loadCategories();
    this.loadDiscounts();
    this.loadInventories();
  }
  snimi() {
    this.httpClient.post(this.globals.serverAddress +`/Product`, this.odabrani)
      .subscribe({
        next: (value: any) => {
          this.close();
          this.reloadData.emit();
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }});
  }
  close() {
    this.closeModal.emit();
  }
  loadCategories() {
    this.httpClient.get(this.globals.serverAddress + "/ProductCategory")
      .subscribe({
        next: (value: any) => {
          this.cmbDataCategory = value.data;
          console.log(this.cmbDataCategory);
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }});
  }
  loadDiscounts() {
    this.httpClient.get(this.globals.serverAddress + "/Discount")
      .subscribe({
        next: (value: any) => {
          this.cmbDataDiscount = value.data;
          console.log(this.cmbDataDiscount);
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }});
  }
  loadInventories() {
    this.httpClient.get(this.globals.serverAddress + "/ProductInventory")
      .subscribe({
        next: (value: any) => {
          this.cmbDataInventory = value.data;
          console.log(this.cmbDataInventory);
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }});
  }
}
