import { Globals } from 'src/app/globals';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs/internal/Observable';
import { finalize } from 'rxjs/internal/operators/finalize';
import { tap } from 'rxjs/internal/operators/tap';
import { Injectable } from '@angular/core';
import {Toast, ToastrService} from "ngx-toastr";

@Injectable()
export class ITSHOPInterceptor implements HttpInterceptor {
  constructor( public globals: Globals, private toastService: ToastrService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      tap(
        // Succeeds when there is a response; ignore other events
        event => {
          if (event instanceof HttpResponse) {
            if (event.body.status === 201) {
              console.log('Successfully saved');
              alert("Successfully saved");
            }
            else if(event.body.status===200)
            {
              //alert("Success");
              console.log("Success");
            }
          }
        },
        // Operation failed; error is an HttpErrorResponse
        error => {
          // Wrong username or password


        }
      ),
      // Log when response observable either completes or errors
      finalize(() => {

      })
    );
  }
}
