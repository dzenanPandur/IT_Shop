import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

import {ProductSnimiVM} from "../_models/ProductSnimiVM";
import {HttpClient} from "@angular/common/http";
import {Globals} from "../globals";
import {ProductPictureSnimiVM} from "../_models/ProductPictureSnimiVM";
@Component({
  selector: 'app-product-picture-snimi',
  templateUrl: './product-picture-snimi.component.html',
  styleUrls: ['./product-picture-snimi.component.css']
})
export class ProductPictureSnimiComponent {
  @Input() odabrani: ProductPictureSnimiVM = {};
  @Input() prikazi?: boolean;

  fileName = '';

  @Output() closeModal = new EventEmitter<void>();
  @Output() reloadData = new EventEmitter<void>();
  private formData: FormData;
  imageSrc: string | ArrayBuffer | null | undefined;

  constructor(private httpClient: HttpClient, public globals: Globals) {
    this.formData = new FormData();
  }

  snimi() {
    this.httpClient.post(this.globals.serverAddress +`/ProductPicture/${this.odabrani.productID}`, this.formData)
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
  onFileSelected(event: any) {
    const file:File = event.target.files[0];

    if (file) {
      this.formData.set("product_image", file);

      const reader = new FileReader();
      reader.onload = e => this.imageSrc = reader.result;

      reader.readAsDataURL(file);
    }
  }

  close() {
    this.closeModal.emit();
  }
}
