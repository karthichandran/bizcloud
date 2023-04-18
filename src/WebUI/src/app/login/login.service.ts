/** Angular Imports */
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpEvent } from '@angular/common/http';

/** rxjs Imports */
import { Observable,of, Observer, Subject } from 'rxjs';
import { map } from 'rxjs/operators';
import { isNullOrUndefined } from '@swimlane/ngx-datatable';
import { AuthenticationInterceptor } from 'app/core/authentication/authentication.interceptor';
import { UserService } from '../user/user.service';
import * as CryptoJS from 'crypto-js';
const key = "Repro";
/**
 * Accounting service.
 */
@Injectable({
  providedIn: 'root'
})
export class LoginService {

  UpdateUserDestail$: Subject<any>=new Subject<any>();
  /**
   * @param {HttpClient} http Http Client to send requests.
   */
  constructor(private http: HttpClient, private authInt: AuthenticationInterceptor, private userService: UserService) { }


  /**
   * @returns {Observable<any>} Loan products data.
   */
  
  loginWithSavedCredentials(): Observable<any> {
    var credentials = JSON.parse(localStorage.getItem("Credentials"));
    var userName = CryptoJS.AES.decrypt(credentials.userName, key).toString(CryptoJS.enc.Utf8);
    var password = CryptoJS.AES.decrypt(credentials.password, key).toString(CryptoJS.enc.Utf8);
   return  this.login(userName, password,true);    
  }

  login(userName: string, password:string,rememberMe:boolean): Observable<any> {   
    return this.http.post('/authenticate/login', { 'userName': userName, 'password': password })
      .pipe(map((data: any) => {
        localStorage.setItem('auth_token', JSON.stringify(data));

        if (rememberMe) {
          var enUserName = CryptoJS.AES.encrypt(userName, key).toString();
          var enPwd = CryptoJS.AES.encrypt(password, key).toString();
          var credentials = { 'userName': enUserName, 'password': enPwd };
          localStorage.setItem('Credentials', JSON.stringify(credentials));
        }

        this.authInt.setAuthorizationNewToken(data.token);
        this.startRefreshTokenTimer();
        this.getUserProfile(userName);
        return of(data);
      }));    
  }

  getUserProfile(userName:string) {
    this.userService.getUserProfile(userName).subscribe(response => {
      localStorage.setItem('UserProfile',JSON.stringify(response));
      this.UpdateUserDestail$.next(response);
    });
  }

  refreshToken(): void {
    var tokenObj :any= JSON.parse(localStorage.getItem("auth_token"));
    this.http.post('/authenticate/refresh-token', tokenObj )
      .subscribe((data:any) => {
        localStorage.setItem('auth_token', JSON.stringify(data));
        this.authInt.setAuthorizationNewToken(data.token);
        this.startRefreshTokenTimer();
      });    
  }
  private refreshTokenTimeout;

  private startRefreshTokenTimer() {   
    // set a timeout to refresh the token a minute before it expires
    const timeout = 2* (60 * 1000);
    this.refreshTokenTimeout = setTimeout(() => this.refreshToken(), timeout);
  }
   stopRefreshTokenTimer() {
    clearTimeout(this.refreshTokenTimeout);
  }
  logOut() {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('UserProfile');
    localStorage.removeItem('Credentials');
    this.stopRefreshTokenTimer();
    this.authInt.removeAuthorization();   
  }

  forgotPassword(model: any): Observable<any> {
    return this.http.post('/forgotPassword', model );
  }
}
