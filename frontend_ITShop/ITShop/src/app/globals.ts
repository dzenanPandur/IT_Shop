import {Injectable} from "@angular/core";

@Injectable()
export class Globals {
  public loggedUser: any;
  public authData: any;
  public isAuthenticated: boolean;
  public serverAddress = 'https://localhost:7093/api';
  public constructor(){

    this.isAuthenticated=false;
  }

}


