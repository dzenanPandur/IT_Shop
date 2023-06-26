import {Component, OnInit, ElementRef, ViewChild, Input,Output} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Globals} from "../globals";
import {ActivatedRoute, Router} from "@angular/router";
import {
  StripeCardElement,
  StripeElements,
  StripeElementsOptions,
  StripeCardElementOptions,
  loadStripe
} from '@stripe/stripe-js';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})

export class CartComponent implements OnInit {
  stripe: any;
  elements!: StripeElements;
  cardElement!: StripeCardElement;

  @ViewChild('cardElement') cardElementRef!: ElementRef;
  constructor(private httpClient: HttpClient, public globals: Globals, private router: Router, private route: ActivatedRoute) {

  }

  ngOnInit(): void {
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

  loadData() {
    this.httpClient.get(this.globals.serverAddress + "/CartItems?items_per_page=" + this.itemsPerPage + "&page_number=" + this.p)
      .subscribe({
        next: (value: any) => {
          /* if(this.filter_productName!=null){

             this.itemsPerPage=this.totalItems;
             this.loadData();
           }*/
          this.tableData = value.data;

          this.totalItems = this.tableData.length;
          this.totalTotalPrice = this.tableData.reduce((total: number, product: any) => total + product.totalPrice, 0);
          console.log(this.tableData);
          console.log(this.totalItems);
          console.log(this.totalTotalPrice);
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


  UkloniIzkorpe(id: number) {
    if (!confirm("Da li ste sigurni da zelite ukloniti proizvod iz korpe?"))
      return;

    this.httpClient.delete(this.globals.serverAddress + '/CartItems/' + id)
      .subscribe({
        next: (value: any) => {
          this.loadData();
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
          },
          error: (err: any) => {
            alert("error");
            console.log(err);
          }
        });
    });

  }
  onSubmit() {

    this.stripe.createPaymentMethod({
      type: 'card',
      card: this.cardElement
    })
      .then((result:any) => {
        if (result.error) {
          // Error occurred while creating payment method
          console.error(result.error.message);
        } else {
          // Payment method created successfully
          const paymentMethodId = result.paymentMethod.id;

          // Use the paymentMethodId for further processing
          console.log('Payment method created. Payment Method ID:', paymentMethodId);



          // Send the paymentMethodId to your server-side API for additional processing
          this.processPaymentOnServer(paymentMethodId);
        }
      });
  }
  processPaymentOnServer(paymentMethodId: string)
  {
    const paymentData = {
      paymentMethodId: paymentMethodId,
      amount: this.totalTotalPrice * 100
    };

    this.httpClient.post(this.globals.serverAddress + '/payment', paymentData)
      .subscribe(
        response => {
          // Payment successful, handle the response
          // Remove items from cart
          this.tableData.forEach((item: { id: number; }) => {
            // Perform operations on each item
            this.httpClient.delete(this.globals.serverAddress + '/CartItems/' + item.id)
              .subscribe({
                next: (value: any) => {
                  this.loadData();
                },
                error: (err: any) => {
                  alert("error");
                  console.log(err);
                }
              });
          });
          window.location.reload();
          console.log(response);
        },
        error => {
          // Handle any errors that occur during payment processing
          console.error(error);
        }
      );
  }

}
