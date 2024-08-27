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

export function getErrorMessage(controlName: string, form: FormGroup): string {
  const control = form.get(controlName);
  if (control && control.errors) {
    if (control.errors['required']) {
      return 'This field is required';
    } else if (control.errors['minlength']) {
      return `Minimum length is ${control.errors['minlength'].requiredLength}`;
    } else if (control.errors['maxlength']) {
      return `Maximum length is ${control.errors['maxlength'].requiredLength}`;
    } else if (control.errors['minimumAge']) {
      return `Minimum age is 16`;
    } else if (control.errors['range']) {
      return 'Value out of range';
    } else if (control.errors['minValue']) {
      return 'Value must be greater than or equal to 0';
    } else if (control.errors['ageFromLessThanAgeTo']) {
      return 'Age From must be less than Age To';
    }
  }
  return '';
}
