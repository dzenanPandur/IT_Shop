import {Component, OnInit, ElementRef, ViewChild} from '@angular/core';
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {Globals} from "../globals";
import {ActivatedRoute, Router} from "@angular/router";
import {
  StripeCardElement,
  StripeElements,
  StripeElementsOptions,
  StripeCardElementOptions,
  loadStripe
} from '@stripe/stripe-js';
import {CookieService} from "ngx-cookie";
import {IndividualConfig} from "ngx-toastr";
import {SignalrService} from "../signalr.service";
interface PaymentResponse {
  paymentIntentId: string;
  charge_id: string;
}

interface Receipt{
  receiptUrl: string;
}
@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})

export class CartComponent implements OnInit {
  stripe: any;
  elements!: StripeElements;
  cardElement!: StripeCardElement;
  paymentIntentId: string="";
  charge_id: string="";
  public fullName: any;
  subscribedUser_data: any;
  orderSub: boolean = false;
  stripeAmount: number = 0;
  errorResponse: HttpErrorResponse | null = null;
  @ViewChild('cardElement') cardElementRef!: ElementRef;
  constructor(private httpClient: HttpClient, public globals: Globals, private router: Router, private route: ActivatedRoute, public _cookieService: CookieService,public signalrService:SignalrService) {

  }

  ngOnInit(): void {

    this.fullName =this._cookieService.getObject('auth');
    this.getSubscriptionById();

    const stripePromise = loadStripe(this.globals.stripeApiPublishableKey);
    stripePromise.then(stripe => {
      this.stripe = stripe;
      const elementsOptions: StripeElementsOptions = {
        fonts: [{ cssSrc: 'https://fonts.googleapis.com/css?family=Open+Sans' }]
      };
      this.elements = this.stripe.elements(elementsOptions);
      const cardElementOptions: StripeCardElementOptions = {
        style: {

          base: {
            fontFamily: 'Arial, sans-serif',
            fontSize: '22px',
            color: '#32325d',
            '::placeholder': {
              color: '#aab7c4',
            },
          },
          invalid: {
            color: '#dc3545',
          },
        },
        classes: {
          base: 'stripe-input-base',
        },
      };
      this.cardElement = this.elements.create('card', cardElementOptions);
      this.cardElement.mount(this.cardElementRef.nativeElement);
    });
    this.loadData();
  }

  itemsPerPage: number = 2;
  totalItems: any;
  tableData: any;
  p: number = 1;
  totalTotalPrice: any;
  order:any;
  shippingAdress:string="";
  showModal: boolean = false;
  discount: any;
  discountAmount: any;
  discountMemberAmount: any;
  totalDiscountedPrice: any;
  isButtonDisabled: boolean=false;
  openModal() {
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }




  loadData() {
    this.httpClient.get(this.globals.serverAddress + "/CartItems?items_per_page=" + this.itemsPerPage + "&page_number=" + this.p)
      .subscribe({
        next: (value: any) => {
          var message="Cart loaded successfully";
          this.signalrService.toastr.success('', message, {
            timeOut: 2500,
            progressBar: true,
            closeButton: true,
          } as IndividualConfig);
          this.tableData = value.data;

          this.totalItems = this.tableData.length;
          this.totalTotalPrice = this.calculateTotalPriceWithDiscounts();

          this.tableData.forEach((item: any) => {
            const discountID = item.product.discountID;
            if (discountID) {
              this.httpClient.get(this.globals.serverAddress + '/Discount/' + discountID).subscribe(data => {
                this.discount = data;
                this.discountAmount=this.discount.data.discountPercent;
                if (this.discountAmount) {
                  item.product.discountPercent = this.discountAmount; // Add the discountPercent property to the product
                  item.totalPrice = this.calculateDiscountedPrice(item.product.price, this.discountAmount); // Calculate discounted total price
                  this.totalTotalPrice = this.calculateTotalPriceWithDiscounts();
                  this.discountAmount = this.totalTotalPrice/10;
                  this.discountMemberAmount = this.totalTotalPrice/10;
                  this.totalDiscountedPrice = this.totalTotalPrice - this.discountAmount;
                }
              });
            }
          });
          this.discountAmount = this.totalTotalPrice/10;
          this.discountMemberAmount = this.totalTotalPrice/10;
          this.totalDiscountedPrice = this.totalTotalPrice - this.discountAmount;
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }
      });

  }

  cc() {
    /*if(this.filter_productName!=null){

      this.itemsPerPage=this.totalItems;
      this.loadData();
    }*/
    this.loadData();
  }
  getSubscriptionById(){
    this.httpClient.get(this.globals.serverAddress + "/Subscription/get-subscriptions-by-id?id=" + this.fullName.userId)
      .subscribe(data=>{
        this.subscribedUser_data=data;
        console.log(this.subscribedUser_data);
      })
  }

  UkloniIzkorpe(id: number) {
    if (!confirm("Da li ste sigurni da zelite ukloniti proizvod iz korpe?"))
      return;

    this.httpClient.delete(this.globals.serverAddress + '/CartItems/' + id)
      .subscribe({
        next: (value: any) => {
          this.loadData();
          var message="This item has been removed";
          this.signalrService.toastr.success('', message, {
            timeOut: 2500,
            progressBar: true,
            closeButton: true,
          } as IndividualConfig);
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }
      });
  }

  UkloniteSve() {
    if (!confirm("Da li ste sigurni da zelite ukloniti sve proizvode iz korpe?"))
      return;
    this.tableData.forEach((item: { id: number; }) => {
      // Perform operations on each item
      this.httpClient.delete(this.globals.serverAddress + '/CartItems/' + item.id)
        .subscribe({
          next: (value: any) => {
            this.loadData();
            var message="All items have been removed";
            this.signalrService.toastr.success('', message, {
              timeOut: 2500,
              progressBar: true,
              closeButton: true,
            } as IndividualConfig);
          },
          error: (err: any) => {
            alert("error");
            console.log(err);
          }
        });
    });

  }
  onSubmit() {
    this.isButtonDisabled = true;
    this.stripe.createPaymentMethod({
      type: 'card',
      card: this.cardElement
    })
      .then((result:any) => {
        if (result.error) {
          (error: HttpErrorResponse) => {
            this.errorResponse = error.error;
          }//ovo sam ja dodao maloprije za ovaj error "your card is declined"
          // Error occurred while creating payment method
         // console.error(result.error.message);
          this.isButtonDisabled=false;
        } else {
          // Payment method created successfully
          const paymentMethodId = result.paymentMethod.id;

          console.log('Payment method created. Payment Method ID:', paymentMethodId);
          this.processPaymentOnServer(paymentMethodId);
        }
      });
  }
  processPaymentOnServer(paymentMethodId: string)
  {
    if(this.subscribedUser_data.data[0].isSubscribed)
      this.stripeAmount = Math.round(this.totalDiscountedPrice*100);
    else
      this.stripeAmount=this.totalTotalPrice*100;

    const paymentData = {
      paymentMethodId: paymentMethodId,
      amount: this.stripeAmount
    };

    this.httpClient.post(this.globals.serverAddress + '/payment', paymentData)
      .subscribe(
        (response: PaymentResponse | Object) => {
          this.paymentIntentId = (response as PaymentResponse).paymentIntentId;
          this.charge_id = (response as PaymentResponse).charge_id;
          this.httpClient.get(this.globals.serverAddress + "/payment/receipt/"+this.charge_id).subscribe(
            (response: Receipt |Object)=>{
              const receipt_url = (response as Receipt).receiptUrl;
              this.CreateOrder(receipt_url, this.paymentIntentId);
              //console.log(receipt_url);
            }, error => console.log( error)
          );
          //console.log('Payment Intent ID:', this.paymentIntentId);
          //console.log('Charge ID:', this.charge_id);

          // Remove items from cart
          this.tableData.forEach((item: { id: number; }) => {
            // Perform operations on each item
            this.httpClient.delete(this.globals.serverAddress + '/CartItems/' + item.id)
              .subscribe({
                next: (value: any) => {
                  this.loadData();
                  window.location.reload();
                },
                error: (err: any) => {
                  //alert("error");
                  //console.log(err);
                }
              });
          });
          //console.log(response);
        },
        (error: HttpErrorResponse) => {
          this.isButtonDisabled=false;
          this.errorResponse = error.error;
          //console.log("Ovo je error response" + this.errorResponse)
          //console.error(error);
        }
      );
  }
  brojacminus(x: any) {
    if (x.quantity <= 1) {
      console.log("Brojac ne može biti manji od 1");
      return;
    }

    x.quantity--;
    x.totalPrice = x.product.price * x.quantity;
    this.totalTotalPrice = this.tableData.reduce((total: number, product: any) => total + product.totalPrice, 0);

    this.updateCartItem(x); // Call the function to update the cart item

    console.log(x.totalPrice);

    console.log(this.tableData);
  }
  brojacplus(x: any) {
    if (x.quantity >= 10) {
      console.log("Brojac ne može biti veći od 10");
      return;
    }

    x.quantity++;
    x.totalPrice = x.product.price * x.quantity;
    this.totalTotalPrice = this.tableData.reduce((total: number, product: any) => total + product.totalPrice, 0);

    this.updateCartItem(x); // Call the function to update the cart item


    console.log(this.tableData);
  }
  updateCartItem(cartItem: any) {
    const updatedCartItem = {
      id: cartItem.id,
      quantity: cartItem.quantity,
      totalPrice: cartItem.totalPrice,
      productID: cartItem.productID
    };

    this.httpClient.put(this.globals.serverAddress + '/CartItems/' + cartItem.id, updatedCartItem)
      .subscribe({
        next: (value: any) => {
          this.loadData();
          console.log("Korpa je uspješno ažurirana");
          var message="Cart successfully updated";
          this.signalrService.toastr.success('', message, {
            timeOut: 2500,
            progressBar: true,
            closeButton: true,
          } as IndividualConfig);
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }

      });
  }

  CreateOrder(receipt_url: any, payment_intend_id: any) {
    if(!this.tableData)
      return;

    const isSubscribed = this.subscribedUser_data && this.subscribedUser_data.data[0].isSubscribed;
    const membershipDiscount = isSubscribed ? 0.1 : 0; // 10% discount if subscribed, otherwise 0% discount
    if(isSubscribed)
      this.orderSub = true;

    this.order = {

      quantity: this.totalItems,
      totalTotalPrice: Math.round(this.totalTotalPrice-(this.totalTotalPrice*membershipDiscount)),
      payment_intent_id: payment_intend_id,
      isSubscribed: this.orderSub,
      shippingAdress: this.shippingAdress,
      receipt_url: receipt_url,
      orderItems: this.tableData.map((item: any) => {
        const productPrice = item.product.price;
        const discountedPrice = this.calculateDiscountedPrice(productPrice, item.product.discountPercent);
        const totalPrice = discountedPrice * item.quantity;
        return {
          quantity: item.quantity,
          productID: item.productID,
          totalPrice: totalPrice,
          orderID: item.orderID
        };
      })
    }
    this.httpClient.post(this.globals.serverAddress +`/Orders`,this.order)
      .subscribe({
        next: (value: any) => {
          console.log("Narudžba je uspješno izvršena");
          var message="Order created successfully";
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
  showSuccessToast(): void {
    const toast = document.getElementById('successToast');
    if (toast) {
      toast.classList.add('show');
      setTimeout(() => {
        toast.classList.remove('show');
      }, 3000); // Hide the toast after 3 seconds (adjust the delay as needed)
    }
  }
  calculateDiscountedPrice(price: number, discountPercent: number): number {
    if (discountPercent) {
      const discountAmount = price * (discountPercent / 100);
      return Math.round(price - discountAmount);
    }
    return price;
  }
  calculateTotalPriceWithDiscounts(): number {
    let totalPrice = 0;

    if (this.tableData && this.tableData.length > 0) {
      for (const item of this.tableData) {
        const productPrice = item.product.price;
        const discountedPrice = this.calculateDiscountedPrice(productPrice, item.product.discountPercent);
        totalPrice += discountedPrice * item.quantity;
      }
    }
    return totalPrice;
  }

}
