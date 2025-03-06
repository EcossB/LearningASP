import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpEvent, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private cookie: CookieService){}

  //esto va a interceptar todos los http request y tratara de darle el token.
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    if(this.shouldInterceptRequest(req)){
      // clone the request
      const authRequest = req.clone({
        setHeaders: {
          'authorization': this.cookie.get('authorization')
        }
      });

      return next.handle(authRequest)
    }

    return next.handle(req);
  }

  private shouldInterceptRequest(request: HttpRequest<any>): boolean {
    //aqui le estamos diciendo que intercepte solo las llamadas que contengan eso en su url.
    return request.urlWithParams.indexOf('addAuth=true',0) > -1 ? true : false;
  }
}
