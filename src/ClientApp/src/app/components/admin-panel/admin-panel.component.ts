import { Component, OnInit } from '@angular/core';
import {UserResponseDto} from "../../dtos/auth/userResponseDto";
import {UserService} from "../../services/user-service.service";
import {AssignRoleRequestDto} from "../../dtos/auth/assignRoleRequestDto";
import {NgForOf, NgIf} from "@angular/common";
import { roles } from '../../constants/roles';
import {FormsModule} from "@angular/forms";
import {AuthService} from "../../services/auth-service.service";

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css'],
  imports: [
    NgIf,
    NgForOf,
    FormsModule
  ],
  standalone: true
})
export class AdminPanelComponent implements OnInit {
  users: UserResponseDto[] = [];
  errorMessage: string = '';
  pageSize: number = 2;
  pageNumber: number = 1;
  totalPages: number = 0;
  roles = Object.values(roles);
  selectedRole: string = '';

  constructor(private userService: UserService, private authService: AuthService) { }

  ngOnInit(): void {
    this.loadUsers();
  }

  async loadUsers() {
      await this.userService.getPaginatedUsers(this.pageSize, this.pageNumber).then(
        result => {
          this.users = result.users
          this.totalPages = result.pagination.TotalPages;
        }
      ).catch(
        error => {
          this.errorMessage = error
        }
      );


  }

  async deleteUser(userId: string) {
    await this.userService.deleteUserById(userId).catch(
      error => {
        this.errorMessage = error
      }
    );
    await this.loadUsers();
  }

  async assignRole(email: string, role: string) {
    const model: AssignRoleRequestDto = { email, role };
    await this.userService.assignRole(model).catch(
      error => {
        this.errorMessage = error;
      }
    );
    await this.loadUsers();

  }

  async removeRole(email: string, role: string) {
    await this.userService.removeFromRole(email, role).catch(
      error => {
        this.errorMessage = error;
      }
    );
    await this.loadUsers();
  }

  nextPage() {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.loadUsers();
    }
  }

  previousPage() {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadUsers();
    }
  }

  isModerator(){
    return this.authService.checkRights(roles.moderator);
  }

  isAdmin(){
    return this.authService.checkRights(roles.admin);
  }
}
