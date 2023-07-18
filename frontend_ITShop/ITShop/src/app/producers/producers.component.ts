import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ProductProducerGetVM } from '../_models/ProductProducerGetVM';
import {Globals} from "../globals";


@Component({
  selector: 'app-producers',
  templateUrl: './producers.component.html',
  styleUrls: ['./producers.component.css']
})
export class ProducersComponent implements OnInit {
  tableData?: ProductProducerGetVM[];

  odabrani: any;
  showModal: boolean = false;

  constructor(private httpClient: HttpClient, public globals: Globals) { }

  ngOnInit(): void {
    this.loadData();
  }
  loadData() {
    this.httpClient.get(this.globals.serverAddress + "/ProductProducer")
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

    this.httpClient.delete(`${this.globals.serverAddress}/ProductProducer/${id}`)
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
