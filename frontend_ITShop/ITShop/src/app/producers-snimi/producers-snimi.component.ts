import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MyConfig } from '../my-config';

import {Globals} from "../globals";
import {ProductProducerSnimiVM} from "../_models/ProductProducerSnimiVM";

@Component({
  selector: 'app-producers-snimi',
  templateUrl: './producers-snimi.component.html',
  styleUrls: ['./producers-snimi.component.css']
})
export class ProducersSnimiComponent implements OnInit {
  @Input() odabrani: ProductProducerSnimiVM = {};
  @Input() prikazi?: boolean;

  @Output() closeModal = new EventEmitter<void>();
  @Output() reloadData = new EventEmitter<void>();

  constructor(private httpClient: HttpClient, public globals: Globals) { }

  ngOnInit(): void {
  }

  snimi() {
    this.httpClient.post(this.globals.serverAddress +`/ProductProducer`, this.odabrani)
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
}

