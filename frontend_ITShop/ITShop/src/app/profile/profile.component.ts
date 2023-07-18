import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {Globals} from "../globals";
import {Router} from "@angular/router";
import {CookieService} from "ngx-cookie";
import {LoggedUserInfo} from "../View models/LoggedUserInfo";
import {
  loadStripe,
  StripeCardElement,
  StripeCardElementOptions,
  StripeElements,
  StripeElementsOptions
} from "@stripe/stripe-js";
@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  public TFAStatus: any;
  username: string= "";
  subscribedUser_data: any;
  public loggedUser: LoggedUserInfo;
  customerId: string | undefined=undefined;
  public fullName: any;
  is_subscribed: boolean = false;
  public confirmationFlag: boolean = false;
  stripe: any;
  elements!: StripeElements;
  cardElement!: StripeCardElement;
  @ViewChild('cardElement') cardElementRef!: ElementRef;
  isButtonDisabled: boolean=false;
  errorResponse: HttpErrorResponse | null = null;
  constructor(
    public globals: Globals,
    public _cookieService: CookieService,
    public router: Router,
    private httpClient: HttpClient,
  ) {
    this.loggedUser= new LoggedUserInfo();
  }

  ngOnInit(): void {
    this.fullName =this._cookieService.getObject('auth');
    this.username=this.fullName.username;
    this.loggedUser={
      userID: this.fullName.userId,
      isSubscribed: true
    }
    this.check2FA();
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

  check2FA(){
    this.httpClient.get(this.globals.serverAddress + '/Auth/check2FA?username=' + this.username).subscribe(data=> {
      this.TFAStatus=data;
      console.log(this.TFAStatus);
    })
  }
  update2FA(status: boolean){
    if(status) {
      if (!confirm("Are you sure you want to enable 2FA?"))
        return;
    }
    else
    {
      if (!confirm("Are you sure you want to disable 2FA?"))
        return;
    }
    this.httpClient.post(this.globals.serverAddress + '/Auth/update-2fa?username=' + this.username + "&status="+status, null).subscribe(data=> {

    })
    this.TFAStatus = status;
  }
  getSubscriptionById(){
    this.httpClient.get(this.globals.serverAddress + "/Subscription/get-subscriptions-by-id?id=" + this.fullName.userId)
      .subscribe(data=>{
        this.subscribedUser_data=data;
        console.log(this.subscribedUser_data);
      })
  }
  subscribe(){
    this.httpClient.post(this.globals.serverAddress + "/Subscription/create-subscription", this.loggedUser )
      .subscribe(data=>{
        console.log(data);
        window.location.reload();},
        (error: HttpErrorResponse) => {
          this.isButtonDisabled = false;
          this.errorResponse = error;
        }
      )
  }
  onSubmit() {
    this.isButtonDisabled = true;

    this.stripe.createPaymentMethod({
      type: 'card',
      card: this.cardElement
    }).then((result: any) => {
      if (result.error) {
        (error: HttpErrorResponse) => {
          this.isButtonDisabled = false;
          this.errorResponse = error;
        }
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
          this.customerId = response['customerId'];
          this.createSubscription(paymentMethodId);
        },
        (error: HttpErrorResponse) => {
          this.isButtonDisabled = false;
          this.errorResponse = error;
        }
      );
  }

  createSubscription(paymentMethodId: string) {
    const subscriptionData = {
      paymentMethodId,
      customerId: this.customerId,
      priceId: 'price_1N7zQqAQDwep3kcFVNgBeFvl'
    };

    this.httpClient.post(this.globals.serverAddress + '/payment/create-subscription', subscriptionData)
      .subscribe(
        response => {
          console.log(response);
          console.log(this.loggedUser)
          if(this.subscribedUser_data==null)
            this.subscribe()
          else
            this.updateSubscription()

        },
        (error: HttpErrorResponse) => {
          this.isButtonDisabled = false;
          this.errorResponse = error;
        }
      );
  }

  cancelSubscription() {
    this.isButtonDisabled = true;
    setTimeout(() => {
      this.isButtonDisabled = false;
    }, 5000);
    this.httpClient.put(this.globals.serverAddress + "/Subscription/update-subscription", {userID: this.fullName.userId, isSubscribed: false} )
      .subscribe(data=>{
        console.log(data);
        window.location.reload();},
        (error: HttpErrorResponse) => {
          this.isButtonDisabled = false;
          this.errorResponse = error;
        }
      )
  }

  updateSubscription() {
    this.httpClient.put(this.globals.serverAddress + "/Subscription/update-subscription", {userID: this.fullName.userId, isSubscribed: true} )
      .subscribe(data=>{
        console.log(data);
        window.location.reload();},
        (error: HttpErrorResponse) => {
          this.isButtonDisabled = false;
          this.errorResponse = error;
        }
      )
  }
  roles(role: string): boolean{
    return this.globals.authData.roles.filter(((x:string) => role === x))[0] !== undefined;

  }
}
