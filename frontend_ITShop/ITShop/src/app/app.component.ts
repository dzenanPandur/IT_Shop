import {Component} from '@angular/core';
import {CookieService} from "ngx-cookie";
import {Router} from "@angular/router";
import {Globals} from "./globals";
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ITShop';
  public constructor(
    private route: Router,
    private cookieService: CookieService,
    private globals: Globals,
    public translate: TranslateService
  ) {
    translate.addLangs(['en','bs'])
    translate.setDefaultLang('en')
  }
  switchLang(lang:string){
    this.translate.use(lang)
  }
  ngOnInit(): void {
    if(!this.globals.isAuthenticated && this.cookieService.get('auth')!==undefined)
    {

      this.globals.authData = this.cookieService.getObject('auth');
      this.globals.isAuthenticated=true;


    }

  }
}
