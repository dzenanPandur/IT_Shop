import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MyConfig } from '../my-config';
import { ProductCategorySnimiVM } from '../_models/ProductCategorySnimiVM';
import { ProdutCategoryGetVM } from '../_models/ProdutCategoryGetVM';
import {Globals} from "../globals";
import {IndividualConfig} from "ngx-toastr";
import { SignalrService } from '../signalr.service';


@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit {
  tableData?: ProdutCategoryGetVM[];

  odabrani: any;
  showModal: boolean = false;

  constructor(private httpClient: HttpClient, public globals: Globals,public signalrService:SignalrService) { }

  ngOnInit(): void {
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

  izbrisi(id: number) {
    if(!confirm("Are you sure you want to delete this?"))
      return;

    this.httpClient.delete(`${this.globals.serverAddress}/ProductCategory/${id}`)
    .subscribe({
      next: (value: any) => {

        this.loadData();
      },
      error: (err: any) => {
        alert("error");
        console.log(err);
      }});
    }
}
