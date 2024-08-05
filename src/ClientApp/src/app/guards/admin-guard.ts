import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import {AuthService} from "../services/auth-service.service";
import {roles} from "../constants/roles";

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {

  }

  canActivate(): boolean {
    const userRoles = this.authService.getCurrentUserRoles()
    if (userRoles?.includes("Admin") || userRoles?.includes("Moderator")) {
      return true;
    } else {
      this.router.navigate(['']);
      return false;
    }
  }
}
