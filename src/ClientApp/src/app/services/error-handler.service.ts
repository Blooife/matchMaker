import { Injectable, EventEmitter } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import {ErrorDetails} from "../dtos/shared/ErrorDetails";

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService {
  errorOccurred = new EventEmitter<any>();

  handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred!';
    console.log("error")
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else if(error.error){
      const errorDetails: ErrorDetails = error.error;
      errorMessage = `Error Code: ${error.status}\nMessage: ${errorDetails.ErrorMessage}`;
    }else{
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }

    this.errorOccurred.emit({ message: errorMessage, status: error.status });
    return throwError(errorMessage);
  }
}
