import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Globals} from "../globals";
import {ActivatedRoute, Router} from "@angular/router";
import {CookieService} from "ngx-cookie";




import {ProductProducerSnimiVM} from "../_models/ProductProducerSnimiVM";
import {CartItemsSnimiVM} from "../_models/CartItemsSnimiVM";

@Component({
  selector: 'app-product-prikaz',
  templateUrl: './product-prikaz.component.html',
  styleUrls: ['./product-prikaz.component.css']
})
export class ProductPrikazComponent implements OnInit{

  tableDataPictures:any;
  quantity:number=1;
  product:any;
  id:any;
  public user:any;
  totalPrice: number = 0;
  public cartItem: any; //= { quantity:this.quantity,totalPrice:this.totalPrice,productID:this.product};

  currentSlideIndex: number = 0;

  // ...

  prevSlide() {
    if (this.currentSlideIndex > 0) {
      this.currentSlideIndex--;
    } else {
      this.currentSlideIndex = this.tableDataPictures.length - 1;
    }
  }

  nextSlide() {
    if (this.currentSlideIndex < this.tableDataPictures.length - 1) {
      this.currentSlideIndex++;
    } else {
      this.currentSlideIndex = 0;
    }
  }

  constructor(private httpClient: HttpClient, public globals: Globals,private router: Router, private route: ActivatedRoute,public _cookieService: CookieService ) {

  }



  ngOnInit(): void {
    this.user=this._cookieService.getObject('auth');
    console.log(this.user);
    this.route.params.subscribe(x=> {
        this.id = +x["id"];
        this.loadData();
        this.loadData2()
      }
    )
  }

  loadData() {
    this.httpClient.get(this.globals.serverAddress + "/Product/"+this.id)
      .subscribe({
        next: (value: any) => {
          this.product = value.data;

          //this.totalProduct=value.data.length();
          console.log(this.product);

        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }});
  }
  loadData2() {
    this.httpClient.get(`${this.globals.serverAddress}/ProductPicture/${this.id}`)
      .subscribe({
        next: (value: any) => {
          this.tableDataPictures = value.data;
          console.log(this.tableDataPictures);
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }});
  }

  AddToCart() {

    this.cartItem = {
      id: 0,
      quantity: this.quantity,
      totalPrice: this.product.price*this.quantity,
      productID:this.id
    }

      this.httpClient.post(this.globals.serverAddress +`/CartItems`,this.cartItem)
        .subscribe({
          next: (value: any) => {
            console.log("Proizvod je uspješno dodan u korpu");
          },
          error: (err: any) => {
            alert("error");
            console.log(err);
          }});
  }

  brojacminus() {
    if(this.quantity <= 1){
      console.log("Brojac ne moze ispod 0");
      return;
    }

    this.quantity--;
    this.totalPrice=this.product.price*this.quantity;

    console.log(this.totalPrice);
  }

  brojacplus() {
    //u sure da mozes totalprice ovako , nije sad to vazno, mislim sad kad imas dva razlicita producta samo ce overwrite prethodni totalprice kad ubacis novi product, to
    if(this.quantity>=3){
      console.log("Brojac ne moze iznad 3");
      return;
    }

    this.quantity++;
    this.totalPrice=this.product.price*this.quantity;

    console.log(this.totalPrice);
  }
}
