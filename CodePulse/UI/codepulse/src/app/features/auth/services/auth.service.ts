import { Injectable } from '@angular/core';
import { loginRequest } from '../Models/login-request.model';
import { loginResponse } from '../Models/login-response.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment.development';
import { BehaviorSubject, Observable } from 'rxjs';
import { userModel } from '../Models/user.models';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  $user =  new BehaviorSubject<userModel | undefined>(undefined);

  constructor(private http: HttpClient,
              private cookies: CookieService
  ) { }

  baseUrl: string = environment.apiBaseUrl;

  login(request: loginRequest): Observable<loginResponse>{
    return this.http.post<loginResponse>(`${this.baseUrl}/api/auth/login`, {
      Email: request.email,
      Password: request.password
    });
  }

  setUser(user: userModel): void{

    this.$user.next(user);
    localStorage.setItem('user-email', user.email);
    localStorage.setItem('user-roles', user.roles.join(','));
  }

  user(): Observable<userModel | undefined>{
    return this.$user.asObservable();
  }

  logOut(): void {
    localStorage.clear();
    this.cookies.delete('authorization', '/');
    this.$user.next(undefined);
  }

  getUser(): userModel | undefined {
    const email = localStorage.getItem('user-email');
    const roles = localStorage.getItem('user-roles');

    if(email && roles){
      const userObject: userModel = {
        email: email,
        roles: roles.split(',')
      };

      return userObject
    }

    return undefined;
  }
}
