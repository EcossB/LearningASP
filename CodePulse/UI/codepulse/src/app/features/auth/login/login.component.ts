import { Component } from '@angular/core';
import { loginRequest } from '../Models/login-request.model';
import { AuthService } from '../services/auth.service';
import { CookieService } from 'ngx-cookie-service';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  model: loginRequest;

  constructor(private auth: AuthService,
    private cookieService: CookieService,
    private router: Router
  ) {
    this.model = {
      email: '',
      password: ''
    }
  }

  onFormSubmit(): void{
    this.auth.login(this.model).subscribe({
      next: (data) => {
        //setting the cookies
        this.cookieService.set('authorization',`Bearer ${data.token}`,
          undefined,
          '/',
          undefined,
          true,
          'Strict'
        );

        //set the user
        this.auth.setUser({
          email: data.email,
          roles: data.roles
        });

        //navigate to home page
        this.router.navigateByUrl('/');
      }
    })
  }


}
