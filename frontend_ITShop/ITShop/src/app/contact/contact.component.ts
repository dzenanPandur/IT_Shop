import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Globals} from "../globals";
import {RegisterUser} from "../View models/RegisterUser";
import {IndividualConfig} from "ngx-toastr";
import {SignalrService} from "../signalr.service";
declare function porukaSuccess(a: string): any;
declare function porukaError(a: string): any;

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent implements OnInit {

  public contactInfo: any;

  public _name: any;
  public fname: any;
  public lname: any;
  public _email: any;
  public message: any;
  public subject: any;

  constructor(
    public http: HttpClient,
    public globals: Globals,
    public signalrService: SignalrService
  ) {
  }

  ngOnInit(): void {

  }

  submit() {
    this.message="Message has been sent";
    this.signalrService.toastr.success('', this.message, {
      timeOut: 2500,
      progressBar: true,
      closeButton: true,
    } as IndividualConfig);
    this._name=this.fname+' '+this.lname;
    this.http.post<any>(this.globals.serverAddress + '/SendGrid?name='+this._name+'&email='+this._email+'&message='+this.message+'&subject='+this.subject, null ).subscribe(data=>{



      // error: (err: any) => {
      //   alert("error");
      //   porukaError("Message not sent, please check that you entered all the data.")
      //   console.log(err);
      // }

    })
    window.location.reload();

  }
}

