import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MyConfig } from '../my-config';
import { ProductCategorySnimiVM } from '../_models/ProductCategorySnimiVM';
import { ProdutCategoryGetVM } from '../_models/ProdutCategoryGetVM';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit {
  tableData?: ProdutCategoryGetVM[];

  odabrani: any;
  showModal: boolean = false;

  constructor(private httpClient: HttpClient) { }

  ngOnInit(): void {
    this.loadData();
  }
  loadData() {
    this.httpClient.get(MyConfig.API_URL + "/ProductCategory")
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
    if(!confirm("Da li ste sigurni da zelite pobrisati ovaj zapis?"))
      return;

    this.httpClient.delete(`${MyConfig.API_URL}/ProductCategory/${id}`)
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
