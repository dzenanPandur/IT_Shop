import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MyConfig } from '../my-config';
import { ProductCategorySnimiVM } from '../_models/ProductCategorySnimiVM';

@Component({
  selector: 'app-category-snimi',
  templateUrl: './category-snimi.component.html',
  styleUrls: ['./category-snimi.component.css']
})
export class CategorySnimiComponent implements OnInit {
  @Input() odabrani: ProductCategorySnimiVM = {};
  @Input() prikazi?: boolean;

  @Output() closeModal = new EventEmitter<void>();
  @Output() reloadData = new EventEmitter<void>();
  
  constructor(private httpClient: HttpClient) { }

  ngOnInit(): void {
  }

  snimi() {
    this.httpClient.post(`${MyConfig.API_URL}/ProductCategory`, this.odabrani)
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
