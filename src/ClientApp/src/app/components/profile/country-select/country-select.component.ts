import {Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {NgForOf, NgIf} from "@angular/common";
import {CountryDto} from "../../../dtos/country/CountryDto";
import {ProfileService} from "../../../services/profile-service.service";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-country-select',
  templateUrl: './country-select.component.html',
  styleUrls: ['./country-select.component.css'],
  imports: [
    NgIf,
    NgForOf,
    FormsModule
  ],
  standalone: true
})
export class CountrySelectComponent implements OnInit, OnChanges {
  @Input() selectedCountryId: number | null = null;
  @Output() countrySelected = new EventEmitter<number>();

  countries: CountryDto[] = [];
  isLoading = true;

  constructor(private profileService: ProfileService) {}

  ngOnInit(): void {
    this.loadCountries();
    console.log("init")
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['selectedCountryId'] && this.selectedCountryId !== null) {

    }
  }

  async loadCountries() {
    this.profileService.getAllCountries().subscribe(
        {
          next:(result) =>{
            this.countries = result;
            this.isLoading = false;
          }
        }
      );
  }

  onCountryChange(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    const countryId = selectElement.selectedIndex;
    this.countrySelected.emit(countryId);
  }
}
