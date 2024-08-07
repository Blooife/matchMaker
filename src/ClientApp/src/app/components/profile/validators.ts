import { AbstractControl, FormGroup } from '@angular/forms';

export function minimumAge(minAge: number) {
  return (control: AbstractControl) => {
    const birthDate = new Date(control.value);
    const today = new Date();
    const age = today.getFullYear() - birthDate.getFullYear();

    if (age < minAge) {
      return { minimumAge: true };
    }
    return null;
  };
}

export function rangeValidator(min: number, max: number) {
  return (control: AbstractControl) => {

    if (control.value < min || control.value > max) {
      return { range: true };
    }
    return null;
  };
}

export function minValue(min: number) {
  return (control: AbstractControl) => {

    if (control.value < min) {
      return { minValue: true };
    }
    return null;
  };
}

export function ageFromLessThanOrEqualAgeTo(ageFromKey: string, ageToKey: string) {
  return (formGroup: FormGroup) => {
    const ageFrom = formGroup.controls[ageFromKey];
    const ageTo = formGroup.controls[ageToKey];

    if (ageFrom.value > ageTo.value) {
      ageTo.setErrors({ ageFromLessThanAgeTo: true });
    } else {
      ageTo.setErrors(null);
    }
  };
}
