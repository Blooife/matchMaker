import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProfileService } from '../../../services/profile-service.service';
import { CreateProfileDto } from '../../../dtos/profile/CreateProfileDto';
import { CityDto } from '../../../dtos/city/CityDto';
import { CountryDto } from '../../../dtos/country/CountryDto';
import { Gender } from '../../../constants/gender';
import { CountrySelectComponent } from '../country-select/country-select.component';
import { CitySelectComponent } from '../city-select/city-select.component';
import { GoalSelectComponent } from '../goal-select/goal-select.component';
import { SliderComponent } from '../slider/slider.component';
import { MatButton } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatSliderModule } from '@angular/material/slider';
import {NgForOf} from "@angular/common";

@Component({
  selector: 'app-create-profile',
  templateUrl: './create-profile.component.html',
  styleUrls: ['./create-profile.component.css'],
  standalone: true,
  imports: [
    MatInputModule,
    MatSelectModule,
    MatSliderModule,
    SliderComponent,
    CountrySelectComponent,
    CitySelectComponent,
    GoalSelectComponent,
    MatButton,
    ReactiveFormsModule,
    NgForOf
  ]
})
export class CreateProfileComponent implements OnInit {
  profileForm: FormGroup;
  genders = Object.keys(Gender)
    .filter(key => isNaN(Number(key)))
    .map(key => ({ key, value: Gender[key as keyof typeof Gender] }));
  userId: string = '';
  selectedCountryId: number | null = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private profileService: ProfileService
  ) {
    this.profileForm = this.fb.group({
      name: ['', Validators.required],
      lastName: [''],
      birthDate: ['', Validators.required],
      gender: ['', Validators.required],
      bio: [''],
      height: [''],
      showAge: [true, Validators.required],
      ageFrom: [16, Validators.required],
      ageTo: [100, Validators.required],
      maxDistance: [0, Validators.required],
      preferredGender: ['', Validators.required],
      goalId: [''],
      cityId: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    console.log(this.genders)
    this.route.paramMap.subscribe(params => {
      this.userId = params.get('userId')!;
    });
  }

  onSubmit() {
    if (this.profileForm.valid) {
      const createProfileDto: CreateProfileDto = {
        ...this.profileForm.value,
        birthDate: new Date(this.profileForm.value.birthDate).toISOString(),
        gender: Number(this.profileForm.value.gender),
        preferredGender: Number(this.profileForm.value.preferredGender),
        userId: this.userId
      };


      this.profileService.createProfile(createProfileDto).subscribe({
        next: () => {
          this.router.navigate(['/profile']);
        }
      });
    }
  }

  onCountrySelected(countryId: number) {
    this.selectedCountryId = countryId;
    this.profileForm.patchValue({ cityId: '' });
  }

  onCitySelected(cityId: number) {
    this.profileForm.patchValue({ cityId });
  }

  onGoalSelected(goalId: number) {
    this.profileForm.patchValue({ goalId });
  }
}
