import {Component, EventEmitter, OnInit,Output} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Globals} from "../globals";
import {ActivatedRoute, Router} from "@angular/router";
import {ProductProducerGetVM} from "../_models/ProductProducerGetVM";
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { Options, LabelType } from 'ng5-slider';



@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.css']
})
export class ShopComponent implements OnInit {
  timer: any;

  constructor(private httpClient: HttpClient, public globals: Globals, private router: Router, private route: ActivatedRoute) {
    this.route.params.subscribe({
      next: (value) => {
        if ('category_id' in value) {
          this.categoryId = value['category_id'];
        }
        this.loadData();
      }
    })

    this.dropdownSettings = {
      singleSelection: false,
      idField: 'id',
      textField: 'name',
      selectAllText: 'Select All',
      unSelectAllText: 'Unselect All',
      itemsShowLimit: 6,
      allowSearchFilter: true
    };
  }

  minValue: number = 0;
  maxValue: number = 0;
  options: Options = {
    floor: 0,
    ceil: 0,
    translate: this.optionsTranslate
  };


  optionsTranslate(value: number, label: LabelType): string {
    switch (label) {
    case LabelType.Low:
      return '<b>Min price:</b> KM. ' + value;
    case LabelType.High:
      return '<b>Max price:</b> KM. ' + value;
    default:
      return 'KM. ' + value;
    }
  }

  updatePriceSlider(): void {

    let url = this.globals.serverAddress + "/Product/GetMinMaxPrices?" +
      (this.categoryId ? "&categoryId="+this.categoryId : "") +
      (this.filter_productName.trim().length ? "&search="+this.filter_productName : "");

    if(this.selectedItems)
    {
      this.selectedItems
        .forEach((item: any) => url += "&producerIDs=" + item.id )
    }

    this.httpClient.get(url)
      .subscribe({
        next: (value: any) => {
          const data = value.data;
          if(data.length !== 2)
            return;

          this.minValue = data[0];
          this.maxValue = data[1];

          const options = new Options();
          options.floor = this.minValue;
          options.ceil = this.maxValue;
          options.translate = this.optionsTranslate;
          this.options = options;
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }
      });


  }



  dropdownList:any;
  selectedItems:any;
  dropdownSettings: IDropdownSettings;




  filter_productName: string = "";
  tableData: any;
  p: number = 1;
  odabrani: any;
  tableDataProducers:any;
  itemsPerPage: number = 8;
  totalItems: any;
  categoryId?: number;
  discounts: any[] = [];
  discountAmount: any;
  averageRatings: any = {};

  ngOnInit(): void {
    this.loadData();
    this.loadDataProducers();
  }

  calculateAverageRating(): void {
    for (let product of this.tableData) {
      if (product.reviews && product.reviews.length > 0) {
        const totalValue = product.reviews.reduce((sum: any, review: any) => sum + review.reviewValue, 0);
        const averageRating = totalValue / product.reviews.length;
        this.averageRatings[product.id] = averageRating;
      } else {
        this.averageRatings[product.id] = "No reviews";
      }
    }
  }


  filterProizvode(){
    if(this.tableData==null)
      return [];
    return this.tableData;
    /*.filter((x:any)=>
      (!this.filter_productName
        ||
        (x.name).includes(this.filter_productName)
        ||
        (x.name).toLowerCase().includes(this.filter_productName)
      )*/
      /*&&
      (x.price >= this.priceRange[0] && x.price <= this.priceRange[1])
   )*/

  }
  loadDataProducers() {
    this.httpClient.get(this.globals.serverAddress + "/ProductProducer")
      .subscribe({
        next: (value: any) => {
          this.tableDataProducers = value.data;
          console.log(this.tableDataProducers);
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }});
  }
  loadData(update_price_slider : boolean = true) {
    console.log(this.minValue);
    console.log(this.maxValue);
    let url = this.globals.serverAddress + "/Product" +
    "?items_per_page=" + this.itemsPerPage +
    "&page_number=" + this.p +
      (this.categoryId ? "&categoryId="+this.categoryId : "") +
      (this.minValue ? "&priceMin="+this.minValue : "") +
      (this.maxValue ? "&priceMax="+this.maxValue : "") +
      (this.filter_productName.trim().length ? "&search="+this.filter_productName : "");

    if(this.selectedItems)
    {
      this.selectedItems
        .forEach((item: any) => url += "&producerIDs=" + item.id )
    }

    this.httpClient.get(url)
      .subscribe({
        next: (value: any) => {
         /* if(this.filter_productName!=null){

            this.itemsPerPage=this.totalItems;
            this.loadData();
          }*/

          this.tableData = value.data.dataItems;

          this.totalItems = value.data.totalCount;
          this.getDiscounts();
          this.calculateAverageRating();
          console.log(this.tableData);
          console.log(this.totalItems);

          if(update_price_slider)
            this.updatePriceSlider();
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }
      });

  }

  cc(update_price_slider: boolean = true) {
    this.loadData(update_price_slider);
  }

  ProductPrikaz(x: any) {
    this.router.navigateByUrl('product-prikaz/' + x.id);
  }

  cc_delay(update_price_slider : boolean = true) {
    clearTimeout(this.timer);
    this.timer = setTimeout(() => { this.cc(update_price_slider) }, 250);
  }
  getDiscounts() {
    for (let product of this.tableData) { // Replace `productsArray` with your actual array of products
      const discountID = product.discountID;
      if (discountID) {
        this.httpClient.get(this.globals.serverAddress + '/Discount/' + discountID).subscribe(data => {
          this.discountAmount = data;
          //console.log("Ovo je discount" + this.discountAmount);
          //console.log("Ovo je discount: " + this.discountAmount.data.discountPercent);

          // Store the discount in the discounts array
          this.discounts[discountID] = this.discountAmount.data.discountPercent;
        });
      }
    }
  }
  calculateDiscountedPrice(price: number, discountID: number): number {
    const discountPercent = this.discounts[discountID];
    if (discountPercent) {
      const discountAmount = price * (discountPercent / 100);
      return Math.round(price - discountAmount);
    }
    return price;
  }
}
