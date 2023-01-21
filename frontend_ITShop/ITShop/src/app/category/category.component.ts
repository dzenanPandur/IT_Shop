import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MojConfig as MyConfig } from '../moj-config';
import { ProdutCategoryGetVM } from '../_models/ProdutCategoryGetVM';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit {

  tableData?: ProdutCategoryGetVM[];

  constructor(private httpClient: HttpClient) { }

  ngOnInit(): void {
    this.loadData();
  }
  loadData() {
    this.httpClient.get(MyConfig.API_URL + "/ProductCategory" )
    .subscribe({
      next: (value: any) => {
        this.tableData = value.data;
        console.log(this.tableData);
      },
      error: (err: any) => {
        alert("error");
        console.log(err);
      }})
  }

}
