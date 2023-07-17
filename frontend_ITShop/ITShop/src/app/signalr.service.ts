import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import {ToastrService} from "ngx-toastr";
import {Router} from "@angular/router";
import {Observable, Subject} from "rxjs";
import {Globals} from "./globals";

export class User {
  public id!: string;
  public name!: string;
  public connId!: string;//signalrID
  public msgs!:Array<Message>
}
export class Message{
  constructor(public content:string, public mine:boolean) {
  }
}
@Injectable({ providedIn: 'root' })
export class SignalrService {

  constructor(public toastr: ToastrService, public router: Router, public globals: Globals ) {
  }
  userData!: User;
  hubConnection!: signalR.HubConnection;
  startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.globals.signalrAdresss, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log("hubConnectionStart");
        this.newProductNotificationListener();
      })
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  newProductNotificationListener() {
    console.log("newProductNotificationListenerStart");

    this.hubConnection.on("newProductNotification", (name) => {
      console.log("newProductNotification.listener");
      this.toastr.success(`A new product is available: ${name}!`);
    })
  }
}

