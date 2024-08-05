import {Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import { ProfileService } from "../../../services/profile-service.service";
import { NgForOf, NgIf } from "@angular/common";
import {GoalDto} from "../../../dtos/goal/GoalDto";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-goal-select',
  templateUrl: './goal-select.component.html',
  styleUrls: ['./goal-select.component.css'],
  imports: [
    NgIf,
    NgForOf,
    FormsModule
  ],
  standalone: true
})
export class GoalSelectComponent implements OnInit, OnChanges{
  @Input() selectedGoalId: number | null = null;
  @Output() goalSelected = new EventEmitter<number>();

  goals: GoalDto[] = [];
  isLoading = true;

  constructor(private profileService: ProfileService) {}

  ngOnInit() {
    this.loadGaols();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['selectedGoalId'] && this.selectedGoalId !== null) {

    }
  }

  async loadGaols() {
    this.isLoading = true;
    this.profileService.getAllGoals().subscribe(
      {next:(result) => {
          this.goals =result;
          this.isLoading = false;
        }}
    );
  }

  onGoalChange(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    const goalId = Number(selectElement.selectedIndex);
    this.goalSelected.emit(goalId);
  }
}
