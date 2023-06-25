import {Injectable} from "@angular/core";

@Injectable()
export class Globals {
  public loggedUser: any;
  public authData: any;
  public isAuthenticated: boolean;
  public serverAddress = 'https://localhost:7093/api';
  public readonly stripeApiPublishableKey = 'pk_test_51N7gLcAQDwep3kcFybqnBC3Zki2l1qgsJxyZaSVLnTgqZXwe6DTCa2TTin7c1Y4uW3iJEYzCHV4pylfZmkhlUT8f002b5K8Gxa';
  public constructor(){

    this.isAuthenticated=false;
  }

}


