import {Component, OnInit, ViewChild, AfterViewInit, Input} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Globals} from "../globals";
import {ActivatedRoute, Router} from "@angular/router";
import {CookieService} from "ngx-cookie";
import {NgxStarsComponent} from "ngx-stars";





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
  public cartItem: any;

  currentSlideIndex: number = 0;
  discount: any;
  discountAmount: any;
  reviewText:string="";
  rating:any;
  showModal: boolean = false;
  review:any;
  showReviews: boolean = false;
  toggleReviews() {
    this.showReviews = !this.showReviews;
  }
  openModal() {
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }
  sum:any;
  reviews:any;

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

  constructor(private httpClient: HttpClient, public globals: Globals,private router: Router, private route: ActivatedRoute,
              public _cookieService: CookieService ) {

  }
  @ViewChild(NgxStarsComponent)

  ratingDisplay:any;
  totalRating: any;

  calculateAverageRating() {
    if (this.product && this.product.reviews && this.product.reviews.length > 0) {
      const totalValue = this.product.reviews.reduce((sum:any, review:any) => sum + review.reviewValue, 0);
      const averageRating = totalValue / this.product.reviews.length;
      this.totalRating = averageRating;
    } else {
      this.totalRating = "There are no reviews of this product";
    }
  }


  onRatingSet(rating: number): void {
    this.ratingDisplay = rating;
    console.warn(`User set rating to ${rating}`);
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
          this.product.discountID = value.data.discountID;
          //this.totalProduct=value.data.length();
          console.log(this.product);

          const discountID = this.product.discountID;
          if (discountID) {
            this.httpClient.get(this.globals.serverAddress + '/Discount/' + discountID).subscribe(data => {
              this.discount = data;
              this.discountAmount=this.discount.data.discountPercent;
              if (this.discountAmount) {
                this.product.discountPercent = this.discountAmount; // Add the discountPercent property to the product
              }
            });
          }
          this.calculateAverageRating();
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }});
  }
  roles(role: string): boolean{
    return this.globals.authData.roles.filter(((x:string) => role === x))[0] !== undefined;

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
            this.showSuccessToast();
          },
          error: (err: any) => {
            alert("error");
            console.log(err);
          }});
  }
  showSuccessToast(): void {
    const toast = document.getElementById('successToast');
    if (toast) {
      toast.classList.add('show');
      setTimeout(() => {
        toast.classList.remove('show');
      }, 3000); // Hide the toast after 3 seconds (adjust the delay as needed)
    }
  }

  showSuccesToast(): void {
    const toast = document.getElementById('succesToast');
    if (toast) {
      toast.classList.add('show');
      setTimeout(() => {
        toast.classList.remove('show');
      }, 3000); // Hide the toast after 3 seconds (adjust the delay as needed)
    }
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
  calculateDiscountedPrice(price: number, discountPercent: number): number {
    if (discountPercent) {
      const discountAmount = price * (discountPercent / 100);
      return Math.round(price - discountAmount);
    }
    return price;
  }

  createReview() {
    this.review={
      id:0,
      reviewText:this.reviewText,
      reviewValue:this.ratingDisplay,
      productID:this.id
    }
    this.httpClient.post(this.globals.serverAddress +`/Review`,this.review)
      .subscribe({
        next: (value: any) => {
          console.log("Uspjesno ste ocjenili proizvod");
          this.showSuccesToast();
        },
        error: (err: any) => {
          const errorMessage = err.info || 'Nemate pravo ocijeniti ovaj proizvod jer nije sadržan u vašoj narudžbi.';
          alert(errorMessage);
          console.log(err);
        }});
    this.loadData();
  }
}
