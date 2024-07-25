import { Component } from '@angular/core';
import {Router, RouterLink} from '@angular/router';
import {AuthService} from "../../services/auth-service.service";
import {ReactiveFormsModule} from "@angular/forms";
import {NgIf} from "@angular/common";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  imports: [
    NgIf,
    RouterLink
  ],
  standalone: true
})
export class HeaderComponent {

  constructor(private authService: AuthService, private router: Router) {

  }

  isLoggedIn() {
    return this.authService.isLogged();
  }

  logOut() {
    this.authService.logOut();
    this.router.navigate(['']);
  }
}
