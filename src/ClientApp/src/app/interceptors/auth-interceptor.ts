import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError, from } from 'rxjs';
import { catchError, switchMap, take } from 'rxjs/operators';
import { AuthService } from '../services/auth-service.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService, private router: Router) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    let accessToken = this.authService.getAccessToken();

    if (accessToken) {
      if (!this.authService.isLogged()) {
        return from(this.refresh(req, next)).pipe(
          switchMap((newReq) => next.handle(newReq)),
          catchError((error) => {
            this.authService.logOut();
            this.goToLogIn();
            return throwError(() => error);
          })
        );
      } else {
        req = this.setAuthorizationHeader(req, accessToken);
      }
    }

    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          this.authService.logOut();
          this.goToLogIn();
        }
        return throwError(() => error);
      })
    );
  }

  async refresh(req: HttpRequest<any>, next: HttpHandler): Promise<HttpRequest<any>> {
    const refreshed = await this.authService.refreshToken();

    if (refreshed) {
      const accessToken = this.authService.getAccessToken();
      req = this.setAuthorizationHeader(req, accessToken!);
      return req;
    } else {
      this.authService.logOut();
      this.goToLogIn();
      throw new Error('Token refresh failed');
    }
  }

  setAuthorizationHeader(
    req: HttpRequest<any>,
    accessToken: string
  ): HttpRequest<any> {
    return req.clone({
      setHeaders: {
        Authorization: `Bearer ${accessToken}`,
      },
    });
  }

  private goToLogIn() {
    this.router.navigate(['/login']).catch();
  }
}
