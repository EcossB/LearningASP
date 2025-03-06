import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../features/auth/services/auth.service';
import { userModel } from '../../../features/auth/Models/user.models';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit{

  user?: userModel;

  constructor(private auth: AuthService,
              private router: Router
  ) {
  }

  ngOnInit(): void {
    this.auth.user().subscribe({
      next: (data) => {
        this.user = data;
      }
    });

   this.user = this.auth.getUser();
  }

  logOut(): void {
    this.auth.logOut();
    this.router.navigateByUrl('/');
  }


}
