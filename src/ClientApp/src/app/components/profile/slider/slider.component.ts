import { Component, forwardRef, Input } from '@angular/core';
import {ControlValueAccessor, FormsModule, NG_VALUE_ACCESSOR} from '@angular/forms';
import {MatSlider, MatSliderThumb} from "@angular/material/slider";

@Component({
  selector: 'app-slider',
  templateUrl: './slider.component.html',
  styleUrls: ['./slider.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SliderComponent),
      multi: true
    }
  ],
  imports: [
    MatSlider,
    MatSliderThumb,
    FormsModule
  ],
  standalone: true
})
export class SliderComponent implements ControlValueAccessor {
  @Input() min: number = 0;
  @Input() max: number = 100;
  @Input() step: number = 1;

  private _value: number = 0;
  get value(): number {
    return this._value;
  }
  set value(value: number) {
    this._value = value;
    this.onChange(value);
    this.onTouched();
  }

  onChange = (value: number) => {};
  onTouched = () => {};

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    // Handle disabled state
  }

  writeValue(value: number): void {
    this.value = value;
  }

  handleSliderChange(event: any): void {
    this.value = event.value;
  }
}
