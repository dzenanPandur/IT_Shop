import {Component, OnInit, ElementRef, ViewChild, Input,Output} from '@angular/core';
import {Globals} from "../globals";
import { CookieService } from 'ngx-cookie';
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {
  StripeCardElement,
  StripeElements,
  StripeElementsOptions,
  StripeCardElementOptions,
  loadStripe
} from '@stripe/stripe-js';
import {LoggedUserInfo} from "../View models/LoggedUserInfo";
import {delay} from "rxjs";
import {TranslateService} from "@ngx-translate/core";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  stripe: any;
  elements!: StripeElements;
  cardElement!: StripeCardElement;
  @ViewChild('cardElement') cardElementRef!: ElementRef;
  public loggedUser: LoggedUserInfo;
  customerId: string | undefined=undefined;
  public fullName: any;
  tableData:any;

  subscribedUser_data: any;

  is_subscribed: boolean = false;

  constructor(
    public globals: Globals,
    public _cookieService: CookieService,
    public router: Router,
    private httpClient: HttpClient,
    public translate: TranslateService
  ) {
    this.loggedUser= new LoggedUserInfo();
    translate.addLangs(['en','de','bs','hr'])
    translate.setDefaultLang('en')
  }
  switchLang(lang:string){
    this.translate.use(lang)
  }

  ngOnInit(): void {
    this.fullName =this._cookieService.getObject('auth');
    this.loadData();

    this.loggedUser={
      userID: this.fullName.userId,
      isSubscribed: true
    }
    //this.getSubscriptionById()

    console.log("This is the status of the subscription: " + this.is_subscribed);
    console.log("User subscription: "+ this.subscribedUser_data)

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
              color: '#aab7c4'
            }
          },
          invalid: {
            color: '#dc3545'
          }
        },
        classes: {
          base: 'stripe-input-base'
        }
      };
      this.cardElement = this.elements.create('card', cardElementOptions);
      this.cardElement.mount(this.cardElementRef.nativeElement);
    });

  }

  loadData() {
    this.httpClient.get(this.globals.serverAddress + "/ProductCategory")
      .subscribe({
        next: (value: any) => {
          this.tableData = value.data;
          console.log(this.tableData);
          //if(this.subscribedUser_data.data != null)
          this.getSubscriptionById()
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }});
  }

  logout() {
    this._cookieService.remove('auth');
    this.globals.isAuthenticated=false;
    //window.location.reload();
    this.router.navigate(['']).then(()=>window.location.reload());
  }
  cc(){
    console.log(this._cookieService.get('auth'));
    console.log(this.globals.isAuthenticated);
    console.log(this.globals.authData);
  }
  roles(role: string): boolean{
    return this.globals.authData.roles.filter(((x:string) => role === x))[0] !== undefined;

  }

  CartItems() {
    this.router.navigateByUrl("cart")
  }

  subscribe(){
    this.httpClient.post(this.globals.serverAddress + "/Subscription/create-subscription", this.loggedUser )
      .subscribe(data=>{
        console.log(data);
        window.location.reload();})

  }
  onSubmit() {
    this.stripe.createPaymentMethod({
      type: 'card',
      card: this.cardElement
    }).then((result: any) => {
      if (result.error) {
        console.error(result.error.message);
      } else {
        const paymentMethodId = result.paymentMethod.id;
        console.log('Payment method created. Payment Method ID:', paymentMethodId);
        this.createCustomer(paymentMethodId);
      }
    });
  }

  createCustomer(paymentMethodId: string) {
    const customerData = {
      paymentMethodId
    };

    this.httpClient.post<any>(this.globals.serverAddress + '/payment/create-customer', customerData)
      .subscribe(
        response  => {
          console.log(response);
          this.customerId = response['customerId']; // Store the customer ID
          this.createSubscription(paymentMethodId);
        },
        error => {
          console.error(error);
        }
      );
  }

  createSubscription(paymentMethodId: string) {
    const subscriptionData = {
      paymentMethodId,
      customerId: this.customerId, // Pass the customer ID
      priceId: 'price_1N7zQqAQDwep3kcFVNgBeFvl'
    };

    this.httpClient.post(this.globals.serverAddress + '/payment/create-subscription', subscriptionData)
      .subscribe(
        response => {
          console.log(response);
          console.log(this.loggedUser)
          // Handle the subscription response as needed
          if(this.subscribedUser_data==null)
            this.subscribe()
          else
            this.updateSubscription()

        },
        error => {
          console.error(error);
        }
      );
  }

  cancelSubscription() {
    this.httpClient.put(this.globals.serverAddress + "/Subscription/update-subscription", {userID: this.fullName.userId, isSubscribed: false} )
      .subscribe(data=>{
        console.log(data);
        window.location.reload();})
  }

  getSubscriptionById(){
    this.httpClient.get(this.globals.serverAddress + "/Subscription/get-subscriptions-by-id?id=" + this.fullName.userId)
      .subscribe(data=>{
        this.subscribedUser_data=data;
        console.log(this.subscribedUser_data);
      })
  }

  updateSubscription() {
    this.httpClient.put(this.globals.serverAddress + "/Subscription/update-subscription", {userID: this.fullName.userId, isSubscribed: true} )
      .subscribe(data=>{
        console.log(data);
        window.location.reload();})
  }
}
