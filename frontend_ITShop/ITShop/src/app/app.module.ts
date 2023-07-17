import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule,RouterOutlet, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import { NavbarComponent } from './navbar/navbar.component';
import { ShopComponent } from './shop/shop.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { CategoryComponent } from './category/category.component';
import { CategorySnimiComponent } from './category-snimi/category-snimi.component';
import { LoginComponent } from './login/login.component';
import {AuthInterceptor} from "./Interceptor/auth.interceptor";
import {ITSHOPInterceptor} from "./Interceptor/itshop.interceptor";
import {AuthGuard} from "./Guards/AuthGuard";
import {AdminGuard} from "./Guards/AdminGuard"
import {Globals} from "./globals";
import {CookieModule} from "ngx-cookie";
import { RegisterComponent } from './register/register.component';
import { AdminComponent } from './admin/admin.component';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import {EmployeeGuard} from "./Guards/EmployeeGuard";
import { ProductComponent } from './product/product.component';
import { ProductSnimiComponent } from './product-snimi/product-snimi.component';
import { ProductPictureComponent } from './product-picture/product-picture.component';
import {ProductPictureSnimiComponent} from "./product-picture-snimi/product-picture-snimi.component";
import {NgxPaginationModule} from "ngx-pagination";
import { ProductPrikazComponent } from './product-prikaz/product-prikaz.component';
import { SearchComponent } from './search/search.component';
import { ProducersComponent } from './producers/producers.component';
import { ProducersSnimiComponent } from './producers-snimi/producers-snimi.component';
import {NgMultiSelectDropDownModule} from "ng-multiselect-dropdown";
import { NgxSliderModule } from '@angular-slider/ngx-slider';
import { Ng5SliderModule } from 'ng5-slider';
import { CartComponent } from './cart/cart.component';
import { ContactComponent } from './contact/contact.component';
import { ProblemReportComponent } from './problem-report/problem-report.component';
import { OwlModule} from "ngx-owl-carousel";
import { CarouselModule } from 'ngx-bootstrap/carousel';
import {HttpClient} from "@angular/common/http";
import {TranslateLoader,TranslateModule} from '@ngx-translate/core';
import {TranslateHttpLoader} from '@ngx-translate/http-loader';
import { MyOrderComponent } from './my-order/my-order.component';
import { MyOrderDetailsComponent } from './my-order-details/my-order-details.component';
import { AllOrdersComponent } from './all-orders/all-orders.component';
import { ProfileComponent } from './profile/profile.component';
import { NgxStarsModule } from 'ngx-stars';
import { EmployeeComponent } from './employee/employee.component';
import { AboutUsComponent } from './about-us/about-us.component';





const routes: Routes = [
  {path: 'admin', component: AdminComponent, canActivate: [AdminGuard]},
  {path: 'employee', component: EmployeeComponent, canActivate: [EmployeeGuard]},
  {path: 'categories', component: CategoryComponent, canActivate: [EmployeeGuard]},
  {path: 'producers', component: ProducersComponent, canActivate: [EmployeeGuard]},
  {path: 'all-orders', component: AllOrdersComponent, canActivate: [EmployeeGuard]},
  {path: 'contact',component: ContactComponent},
  {path: 'about-us',component: AboutUsComponent},
  {path: 'my-order',component: MyOrderComponent, canActivate: [AuthGuard]},
  {path: 'products', component: ProductComponent, canActivate: [EmployeeGuard]},
  {path: 'product-pictures/:id', component: ProductPictureComponent, canActivate: [EmployeeGuard]},
  {path: 'product-prikaz/:id', component: ProductPrikazComponent},
  {path: 'my-order-details/:id', component: MyOrderDetailsComponent, canActivate: [AuthGuard]},
  {path: 'cart', component: CartComponent, canActivate: [AuthGuard]},
  {path: 'search', component: SearchComponent},



  {path: 'problem', component: ProblemReportComponent, canActivate: [AuthGuard]},
  {path: 'profile', component: ProfileComponent, canActivate: [AuthGuard]},
  {path: 'shop/:category_id/:category', component: ShopComponent},
  {path: '', pathMatch: 'full', component: ShopComponent},
  {path: '**', component: NotFoundComponent},

];

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    NotFoundComponent,
    ShopComponent,
    CategoryComponent,
    CategorySnimiComponent,
    LoginComponent,
    RegisterComponent,
    AdminComponent,
    ProductComponent,
    ProductSnimiComponent,
    ProductPictureComponent,
    ProductPictureSnimiComponent,
    ProductPrikazComponent,
    SearchComponent,
    ProducersComponent,
    ProducersSnimiComponent,
    CartComponent,
    ContactComponent,
    ProblemReportComponent,
    MyOrderComponent,
    MyOrderDetailsComponent,
    AllOrdersComponent,
    ProfileComponent,
    EmployeeComponent,
    AboutUsComponent,


  ],
    imports: [
        BrowserModule,
        RouterModule.forRoot(routes),
        FormsModule,
        RouterOutlet,
        ReactiveFormsModule,
        HttpClientModule,
        CookieModule.forRoot(),
        CommonModule,
        BrowserAnimationsModule,
        ToastrModule.forRoot(),
        NgxPaginationModule,
        NgMultiSelectDropDownModule,
        NgxSliderModule,
        Ng5SliderModule,
        OwlModule,
        NgxStarsModule,
        CarouselModule.forRoot(),
        ToastrModule.forRoot({
        enableHtml: true,
        timeOut: 10000,
        positionClass: 'toast-top-right',
        preventDuplicates: false,
      }),
        TranslateModule.forRoot({
          loader:{
            provide:TranslateLoader,
            useFactory:httpTranslateLoader,
            deps:[HttpClient]
          }
        }),

    ],
  providers: [Globals,
    {
      provide:HTTP_INTERCEPTORS,
      useClass:AuthInterceptor,
      multi:true
    },
    {
      provide:HTTP_INTERCEPTORS,
      useClass:ITSHOPInterceptor,
      multi:true
    },
    AuthGuard,
    AdminGuard,
    EmployeeGuard],
  exports: [ RouterModule ],
  bootstrap: [AppComponent]
})
export class AppModule { }

export function httpTranslateLoader(http:HttpClient){
  return new TranslateHttpLoader(http);
}
