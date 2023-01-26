import {Injectable} from "@angular/core";
import {ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot} from "@angular/router";
import {Globals} from "../globals";
import {Observable} from "rxjs";
import {CookieService} from "ngx-cookie";

class HelperService {
}


@Injectable()
export class AdminGuard implements CanActivate {


  constructor(public globals: Globals)
  { }

  canActivate(

    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    return this.globals.authData.roles.filter((x:string) => 'Admin' === x)[0] !== undefined;
    //return this.globals.isAuthenticated;
  }
}
