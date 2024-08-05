import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import {AuthService} from "../services/auth-service.service";
import {ProfileService} from "../services/profile-service.service";
import {ProfileDto} from "../dtos/profile/ProfileDto";
import {firstValueFrom} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ProfileCreatedGuard implements CanActivate {
  profile: ProfileDto | null = null;
  constructor(private authService: AuthService, private profileService: ProfileService, private router: Router) {

  }

  async canActivate(): Promise<boolean> {
    if (this.authService.isLogged()) {
      const userId = this.authService.getCurrentUserId();
      if (userId) {
        const profile = await firstValueFrom(this.profileService.getProfileByUserId(userId));
        if (profile) {
          return true;
        }else {
          await this.router.navigate(['/create-profile', this.authService.getCurrentUserId()]);
          return false;
        }
      }
    }
    await this.router.navigate([''],);
    return false;
  }
}
