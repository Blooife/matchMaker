import { Component } from '@angular/core';
import {ErrorHandlerService} from "../../services/error-handler.service";
import {NgIf} from "@angular/common";

@Component({
  selector: 'app-error-modal',
  templateUrl: './error-modal.component.html',
  styleUrls: ['./error-modal.component.css'],
  imports: [
    NgIf
  ],
  standalone: true
})
export class ErrorModalComponent {
  showModal: boolean = false;
  errorMessage: string = '';
  errorCode: number | null = null;

  constructor(private errorHandler: ErrorHandlerService) {
    this.errorHandler.errorOccurred.subscribe((error: any) => {
      this.showModal = true;
      this.errorMessage = error.message;
      this.errorCode = error.status;
    });
  }

  closeModal() {
    this.showModal = false;
  }
}
