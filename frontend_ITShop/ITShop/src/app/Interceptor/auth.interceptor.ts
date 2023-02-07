import { Observable } from "rxjs";
import { Globals } from "src/app/globals";
import {Injectable} from "@angular/core";
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {CookieService} from "ngx-cookie";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(public globals: Globals, private cookieService: CookieService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let request: Observable<HttpEvent<any>>;
    if (this.globals.isAuthenticated && this.cookieService.get('auth') !== undefined) {
      const authHeader = 'Bearer ' + this.globals.authData.token;
      const authReq = req.clone({ setHeaders: { Authorization: authHeader } });
      request = next.handle(authReq);
    }
    else {
      request = next.handle(req);
    }
    return request;
  }
}
