import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {ProductSnimiVM} from "../_models/ProductSnimiVM";
import {ProductGetVM} from "../_models/ProductGetVM";
import { HttpClient } from '@angular/common/http';
import {Globals} from "../globals";
import {Router} from "@angular/router";
import {ProductProducerGetVM} from "../_models/ProductProducerGetVM";
import {ProdutCategoryGetVM} from "../_models/ProdutCategoryGetVM";
import {ProductDiscountGetVM} from "../_models/ProductDiscountGetVM";
import {ProductInventoryGetVM} from "../_models/ProductInventoryGetVM";



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
  cmbDataCategory?: ProdutCategoryGetVM[];
  cmbDataProducer?:ProductProducerGetVM[];
  minPrice: number = 0;
  maxPrice: number = 10000;
  category: string ="All";
  producer: string ="All";
  isDownloadChecked: boolean = false;
  isInvalidPriceRange: boolean = false;
  currentDate = new Date();
  report_name: string="";


  //totalProduct:any;
  //@Output() loadCategories = new EventEmitter<void>();

  constructor(private httpClient: HttpClient, public globals: Globals, private route: Router) { }

  ngOnInit(): void {
    this.loadData();
    this.loadProducers();
    this.loadCategories();
  }//
  isButtonDisabled: boolean = false;




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
    this.httpClient.delete(`${this.globals.serverAddress}/Product/${id}`)
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
      inventoryID:1,
      producerID:1
    }
  }

  otvoriSlike(id: number) {
    this.route.navigateByUrl(`product-pictures/${id}`);

  }

  cc() {
    this.loadData();
    //console.log(this.p);
  }

  generateReport() {
    this.isButtonDisabled=true;
    // Define the report parameters
    const parameters = {
      category: this.category,
      producer: this.producer,
      minPrice: this.minPrice,
      maxPrice: this.maxPrice
    };
    if(this.isInvalidPriceRange)
      return;
    else {
      // Make an HTTP POST request to the server to generate the report
      this.httpClient.post(this.globals.serverAddress + '/Report/product-report', parameters, {responseType: 'blob'})
        .subscribe(response => {
          // Create a Blob URL from the response
          const blob = new Blob([response], {type: 'application/pdf'});
          const url = window.URL.createObjectURL(blob);

          // Create a download link for the report
          const link = document.createElement('a');
          link.href = url;
          link.download = this.report_name + "_" + this.currentDate.getDate() + '.' + (this.currentDate.getMonth()+1) + '.' + this.currentDate.getFullYear() + '_report.pdf';
          if(this.isDownloadChecked)
            link.click();
          else
            window.open(url, '_blank');
          this.isButtonDisabled=false;
          // Clean up the Blob URL after the download
          window.URL.revokeObjectURL(url);
        }, error => {
          console.log('An error occurred while generating the report:', error);
          // Handle the error as needed
          this.isButtonDisabled=false;
        });
    }
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

  loadProducers() {
    this.httpClient.get(this.globals.serverAddress + "/ProductProducer")
      .subscribe({
        next: (value: any) => {
          this.cmbDataProducer = value.data;
          console.log(this.cmbDataProducer);
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }});
  }
  validatePriceRange() {
    if (this.minPrice !== null && this.maxPrice !== null && this.minPrice > this.maxPrice) {
      this.isInvalidPriceRange = true;
    } else {
      this.isInvalidPriceRange = false;
      // Proceed with further actions
    }
  }

  setValue() {
    this.minPrice=0;
    this.maxPrice=10000;
    this.category="All";
    this.producer="All";
    this.report_name="";

  }
}
