import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import {firstValueFrom, Observable, throwError} from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import {UserRequestDto} from "../dtos/auth/userRequestDto";
import {GeneralResponseDto} from "../dtos/shared/generalResponseDto";
import {LoginResponseDto} from "../dtos/auth/loginResponseDto";
import {RefreshRequestDto} from "../dtos/auth/refreshRequestDto";
import {jwtDecode} from "jwt-decode";
import {ErrorDetails} from "../dtos/shared/ErrorDetails";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:5001/api/auth'; // Укажите ваш API URL

  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private httpClient: HttpClient) { }

  async register(model: UserRequestDto)  {
    let response = await firstValueFrom(this.httpClient.post<GeneralResponseDto>(`${this.apiUrl}/register`, model, this.httpOptions)
      .pipe(
        retry(2),
        catchError(this.handleError)
      ));
    return response;
  }

  async login(model: UserRequestDto): Promise<boolean> {
    let response = await firstValueFrom(this.httpClient.post<LoginResponseDto>(`${this.apiUrl}/login`, model, this.httpOptions)
      .pipe(
        retry(2),
        catchError(this.handleError)
      )
    );

    if (response.refreshToken){
      this.setTokens(response);

      return true;
    }

    return false;
  }

  async refreshToken(): Promise<boolean> {

    let refreshToken = this.getRefreshToken();

    if(!refreshToken){
      return false;
    }

    let model: RefreshRequestDto = {
      refreshToken: refreshToken
    };
    let response =  await firstValueFrom(this.httpClient.post<LoginResponseDto>(`${this.apiUrl}/refresh`, model, this.httpOptions)
      .pipe(
        retry(2),
        catchError(this.handleError)
      ));

    if (response.refreshToken){
      this.setTokens(response);

      return true;
    }

    return false;
  }

  logOut() {
    localStorage.removeItem('access_token');
    localStorage.removeItem('refresh_token');
    localStorage.removeItem('access_expires_at');
  }

  getAccessToken() {
    return localStorage.getItem('access_token');
  }

  getRefreshToken() {
    return localStorage.getItem('refresh_token');
  }

  getExpiresAt() {
    return localStorage.getItem('access_expires_at');
  }

  isLogged() {
    const expiration = this.getExpiresAt();

    if (expiration) {
      const expiresAt = new Date(JSON.parse(expiration));
      const currentTime = new Date(Date.now());

      return currentTime < expiresAt;
    }

    return false;
  }

  getCurrentUserId(){
    if(!this.isLogged()){
      return null;
    }

    const accessToken = this.getAccessToken();

    if(accessToken){
      const payload = jwtDecode(accessToken);

      return payload.sub;
    }

    return null;
  }

    getCurrentUserRoles(): Array<string> | null{
      if(!this.isLogged()){
        return null;
      }

      const accessToken = this.getAccessToken();

      if(accessToken){
        const payload = jwtDecode<{ [key: string]: any }>(accessToken);

        return payload['role'];
      }

      return null;
    }

  checkRights(role: string): boolean {
    const userRoles = this.getCurrentUserRoles();

    if(userRoles){
      return userRoles?.includes(role);
    }

    return false;

  }

  setTokens(loginDto: LoginResponseDto){
    const payload = jwtDecode(loginDto.jwtToken);

    if(payload.exp){
      localStorage.setItem('access_token', loginDto.jwtToken);
      localStorage.setItem('refresh_token', loginDto.refreshToken!);
      localStorage.setItem('access_expires_at', JSON.stringify(new Date(payload.exp * 1000)));
    }
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      errorMessage = error.error.message;
    } else if (error.error) {
      const errorDetails: ErrorDetails = error.error;
      errorMessage = `${errorDetails.ErrorMessage}`;
    } else {
      errorMessage = error.message;
    }

    console.log(errorMessage);

    return throwError(()=> errorMessage);
  }
}
