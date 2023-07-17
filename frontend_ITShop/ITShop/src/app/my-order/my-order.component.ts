
import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {ProductSnimiVM} from "../_models/ProductSnimiVM";
import {ProductGetVM} from "../_models/ProductGetVM";
import { HttpClient } from '@angular/common/http';
import {Globals} from "../globals";
import {Router} from "@angular/router";
import {IndividualConfig} from "ngx-toastr";
import { SignalrService } from '../signalr.service';

@Component({
  selector: 'app-my-order',
  templateUrl: './my-order.component.html',
  styleUrls: ['./my-order.component.css']
})
export class MyOrderComponent implements OnInit {
  tableData?: any;
  p: number=1;
  odabrani: any;
  showModal: boolean = false;
  itemsPerPage: number=6;
  totalItems: any;


  openModal() {
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  constructor(private httpClient: HttpClient, public globals: Globals, private route: Router,public signalrService:SignalrService) { }

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.httpClient.get(this.globals.serverAddress + "/Orders/api/Orders/GetAllForUser?items_per_page=" + this.itemsPerPage + "&page_number="+this.p)
      .subscribe({
        next: (value: any) => {
          this.tableData = value.data;
          this.totalItems= value.data.totalCount;
          console.log(this.tableData);
          var message="Orders have loaded successfully";
          this.signalrService.toastr.success('', message, {
            timeOut: 2500,
            progressBar: true,
            closeButton: true,
          } as IndividualConfig);
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }});
  }
  izbrisi(id: number) {
    if (!confirm("Are you sure you want to remove this record?"))
      return;
    this.httpClient.delete(this.globals.serverAddress + '/Orders/'+id)
      .subscribe({
        next: (value: any) => {
          var message="Order has been removed";
          this.signalrService.toastr.success('', message, {
            timeOut: 2500,
            progressBar: true,
            closeButton: true,
          } as IndividualConfig);
          this.loadData();
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }
      });
  }
  refundPayment(id: number, paymentIntentId: string){
    console.log(paymentIntentId);
    if (!confirm("Da li ste sigurni da zelite pobrisati ovaj zapis?"))
      return;
    this.httpClient.post<any>(this.globals.serverAddress + '/payment/refund', {paymentIntentId} ).subscribe(
      (response) => {
        console.log('Refund successful:', response);
        this.httpClient.delete(this.globals.serverAddress + '/Orders/'+id)
          .subscribe({
            next: (value: any) => {
              this.loadData();
            },
            error: (err: any) => {
              alert("error");
              console.log(err);
            }
          });
      },
      (error) => {
        console.error('Refund error:', error);
        // Handle error response here
      }
    );
  }
  cc() {
    this.loadData();
    //console.log(this.p);
  }

  productDetails(x: any) {
    this.route.navigateByUrl("/my-order-details/"+x.id);
  }
}
