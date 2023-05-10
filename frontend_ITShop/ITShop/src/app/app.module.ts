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


const routes: Routes = [
  {path: 'admin', component: AdminComponent, canActivate: [AdminGuard]},
  {path: 'categories', component: CategoryComponent, canActivate: [EmployeeGuard]},
  {path: 'products', component: ProductComponent, canActivate: [EmployeeGuard]},
  {path: 'product-pictures/:id', component: ProductPictureComponent, canActivate: [EmployeeGuard]},
  {path: 'product-prikaz/:id', component: ProductPrikazComponent},
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
    NgxPaginationModule
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
