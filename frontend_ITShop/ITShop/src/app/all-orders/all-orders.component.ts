import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {ProductSnimiVM} from "../_models/ProductSnimiVM";
import {ProductGetVM} from "../_models/ProductGetVM";
import { HttpClient } from '@angular/common/http';
import {Globals} from "../globals";
import {Router} from "@angular/router";
@Component({
  selector: 'app-all-orders',
  templateUrl: './all-orders.component.html',
  styleUrls: ['./all-orders.component.css']
})
export class AllOrdersComponent implements OnInit {

  tableData?: any;
  p: number=1;
  odabrani: any;
  showModal: boolean = false;
  itemsPerPage: number=6;
  totalItems: any;
  minPrice: number = 0;
  maxPrice: number = 10000;
  isDownloadChecked: boolean = false;
  isInvalidPriceRange: boolean = false;
  currentDate = new Date();
  report_name: string="";
  fromDate: Date;
  toDate: Date;
  openModal() {
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  constructor(private httpClient: HttpClient, public globals: Globals, private route: Router) {
    this.fromDate = new Date("2020-06-30T17:06:50.930Z");
    this.toDate = new Date();
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.httpClient.get(this.globals.serverAddress + "/Orders?items_per_page=" + this.itemsPerPage + "&page_number="+this.p)
      .subscribe({
        next: (value: any) => {
          this.tableData = value.data;
          this.totalItems= value.data.totalCount;
          console.log(this.tableData);
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }});
  }


  cc() {
    this.loadData();
    //console.log(this.p);
  }

  productDetails(x: any) {
    this.route.navigateByUrl("/my-order-details/"+x.id);
  }

  generateReport() {
    // Define the report parameters
    const parameters = {

      minPrice: this.minPrice,
      maxPrice: this.maxPrice,
      fromDate: this.fromDate,
      toDate: this.toDate
    };
    if(this.isInvalidPriceRange)
      return;
    else {
      // Make an HTTP POST request to the server to generate the report
      this.httpClient.post(this.globals.serverAddress + '/Report/order-report', parameters, {responseType: 'blob'})
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

          // Clean up the Blob URL after the download
          window.URL.revokeObjectURL(url);
        }, error => {
          console.log('An error occurred while generating the report:', error);
          // Handle the error as needed
        });
    }
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
    this.fromDate = new Date("2020-06-30T17:06:50.930Z");
    this.toDate = new Date();
    this.report_name="";

  }
}
