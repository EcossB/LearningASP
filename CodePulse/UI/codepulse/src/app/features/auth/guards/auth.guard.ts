import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { AuthService } from '../services/auth.service';
import { jwtDecode } from 'jwt-decode';

export const authGuard: CanActivateFn = (route, state) => {
  //injecting cookieService to know if the token is in the header
  const cookie = inject(CookieService);
  const auth = inject(AuthService);
  const router = inject(Router)
  const user = auth.getUser();

  //taking the token
  let token = cookie.get('authorization');

  if(token && user){
    token = token.replace('Bearer ', '');
    const decodedToken: any = jwtDecode(token);

    //check the expire date
    const expiredDate = decodedToken.exp * 1000;
    const currentTime = new Date().getTime();

    if (expiredDate < currentTime){
      auth.logOut();
      //here we send them to the login page, once the user login again it will return to the state that
      //it was before
      return router.createUrlTree(['/login'], {queryParams: {returnUrl : state.url}});
    } else {
      //token still valid
      if (user.roles.includes('Writer')){
        return true;
      } else {
        alert('Unauthorized');
        return false;
      }
    }
  }else {
    auth.logOut();
    //here we send them to the login page, once the user login again it will return to the state that
    //it was before
    return router.createUrlTree(['/login'], {queryParams: {returnUrl : state.url}});
  }

};
