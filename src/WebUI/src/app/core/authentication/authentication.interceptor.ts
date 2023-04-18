/** Angular Imports */
import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';

/** rxjs Imports */
import { Observable } from 'rxjs';

/** Environment Configuration */
import { environment } from '../../../environments/environment';
//import { isUndefined } from 'util';

/** Http request options headers. */
const httpOptions = {
  headers: {
       // 'Content-Type': 'application/json; charset=utf-8'
  }
};

/** Authorization header. */
const authorizationHeader = 'Authorization';
/** Two factor access token header. */
//const twoFactorAccessTokenHeader = 'Fineract-Platform-TFA-Token';

/**
 * Http Request interceptor to set the request headers.
 */
@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {

  constructor() {}

  /**
   * Intercepts a Http request and sets the request headers.
   */
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.setAuthorizationToken();
    request = request.clone({ setHeaders: httpOptions.headers });
    return next.handle(request);
  }

  /**
   * Sets the basic/oauth authorization header depending on the configuration.
   * @param {string} authenticationKey Authentication key.
   */
  setAuthorizationToken() {
    if (httpOptions.headers[authorizationHeader]===undefined || httpOptions.headers[authorizationHeader] == "") {
      let tokenObj:any = {};
      //if (sessionStorage.getItem("auth_token") != "" && sessionStorage.getItem("auth_token") != null)
      //  token = sessionStorage.getItem("auth_token");
      if (localStorage.getItem("auth_token") != "" && localStorage.getItem("auth_token")!=null)
        tokenObj = JSON.parse(localStorage.getItem("auth_token")); 
      httpOptions.headers[authorizationHeader] = `Bearer ${tokenObj.token}`;
    }
  }
  setAuthorizationNewToken(token: string) {
    httpOptions.headers[authorizationHeader] = `Bearer ${token}`;
  }


  /**
   * Sets the two factor access token header.
   * @param {string} twoFactorAccessToken Two factor access token.
   */
  //setTwoFactorAccessToken(twoFactorAccessToken: string) {
  //  httpOptions.headers[twoFactorAccessTokenHeader] = twoFactorAccessToken;
  //}

  /**
   * Removes the authorization header.
   */
  removeAuthorization() {
    delete httpOptions.headers[authorizationHeader];
  }

  /**
   * Removes the two factor access token header.
   */
  //removeTwoFactorAuthorization() {
  //  delete httpOptions.headers[twoFactorAccessTokenHeader];
  //}
}
