import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Globals} from "../globals";
import {RegisterUser} from "../View models/RegisterUser";
import {AuthData} from "../View models/AuthData";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  isPasswordHidden: boolean = true;
  public registerRequest: RegisterUser;

  register(){
    this.http.post<any>(this.globals.serverAddress + '/User', this.registerRequest).subscribe(data=>{
      console.log(data);
      window.location.reload();


    })
  }

  constructor(
    public http: HttpClient,
    public globals: Globals
  ) {

    this.registerRequest= new RegisterUser();

  }

  ngOnInit(): void {
  }

}
