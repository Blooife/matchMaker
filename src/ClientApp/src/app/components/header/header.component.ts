import { Component } from '@angular/core';
import {Router, RouterLink} from '@angular/router';
import {AuthService} from "../../services/auth-service.service";
import {AsyncPipe, NgIf} from "@angular/common";
import {Observable} from "rxjs";
import {roles} from "../../constants/roles";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  imports: [
    NgIf,
    RouterLink,
    AsyncPipe
  ],
  standalone: true
})
export class HeaderComponent {
  isLoggedIn$: boolean = false;
  currentUserId$: string | null = '';
  isMenuVisible = true;

  constructor(private authService: AuthService, private router: Router) {
    this.authService.isLoggedIn$.subscribe((isLoggedIn) =>{
      this.isLoggedIn$ = isLoggedIn;
    });

    this.authService.currentUserId$.subscribe((id) =>{
      this.currentUserId$ = id;
    });
  }

  toggleMenu() {
    this.isMenuVisible = !this.isMenuVisible;
  }

  logOut() {
    this.authService.logOut();
    this.router.navigate(['']);
  }

  isAdmin(){
    return this.authService.checkRights(roles.admin);
  }

  isModerator(){
    return this.authService.checkRights(roles.moderator);
  }

  isUser(){
    return this.authService.checkRights(roles.user);
  }

  deleteUserById(){

  }
}
