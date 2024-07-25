import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders, HttpErrorResponse, HttpRequest} from '@angular/common/http';
import {firstValueFrom, Observable, throwError} from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { UserRequestDto } from "../dtos/auth/userRequestDto";
import { GeneralResponseDto } from "../dtos/shared/generalResponseDto";
import { ErrorDetails } from "../dtos/shared/ErrorDetails";
import {UserResponseDto} from "../dtos/auth/userResponseDto";
import {AssignRoleRequestDto} from "../dtos/auth/assignRoleRequestDto"; // Импортируйте DTO для пагинированного списка

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'https://localhost:5001/api/users'; // Укажите ваш API URL

  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private httpClient: HttpClient) { }

  async getPaginatedUsers(pageSize: number, pageNumber: number): Promise<{ users: UserResponseDto[], pagination: any }> {
    const response = await firstValueFrom(this.httpClient.get<Array<UserResponseDto>>(`${this.apiUrl}/paginated?pageSize=${pageSize}&pageNumber=${pageNumber}`,
      {...this.httpOptions,
      observe: 'response'})
      .pipe(
        retry(2),
      catchError(this.handleError)
    )
    );

    const pagination = JSON.parse(response.headers.get('X-Pagination')!);
    console.log(response);
    return {
    users: response.body || [],
    pagination: pagination
    };
  }

  async deleteUserById(userId: string) {
    await firstValueFrom(this.httpClient.delete<GeneralResponseDto>(`${this.apiUrl}/${userId}`, this.httpOptions)
      .pipe(
        catchError(this.handleError)
      ));
  }

  async getUserById(userId: string) {
    return await firstValueFrom(this.httpClient.get<UserResponseDto>(`${this.apiUrl}/${userId}`, this.httpOptions)
      .pipe(
        retry(2),
        catchError(this.handleError)
      ));
  }

  async assignRole(model: AssignRoleRequestDto) {
    return await firstValueFrom(this.httpClient.post<GeneralResponseDto>(`https://localhost:5001/api/roles/assignment`, model, this.httpOptions)
      .pipe(
        retry(2),
        catchError(this.handleError)
      ));
  }

  async removeFromRole(email:string, role: string) {
    const options = {
      headers: this.httpOptions.headers,
      body: {
        email,
        role
      },
    };

    return await firstValueFrom(this.httpClient.delete<GeneralResponseDto>(`https://localhost:5001/api/roles/removal`, options)
      .pipe(
        retry(2),
        catchError(this.handleError)
      ));
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = '';
    if (error.error) {
      const errorDetails: ErrorDetails = error.error;
      errorMessage = `${errorDetails.ErrorMessage}`;
    } else {
      errorMessage = error.message;
    }

    console.log(errorMessage);

    return throwError(() => errorMessage);
  }
}
