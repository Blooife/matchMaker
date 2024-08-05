import {Component, OnDestroy, OnInit} from '@angular/core';
import {NavigationEnd, Router, RouterOutlet} from '@angular/router';
import {HeaderComponent} from "./components/header/header.component";
import {HomeComponent} from "./components/home/home.component";
import {ErrorModalComponent} from "./components/error-modal/error-modal.component";
import {NgClass, NgIf} from "@angular/common";
import {SliderComponent} from "./components/profile/slider/slider.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, HomeComponent, ErrorModalComponent, NgIf, SliderComponent, NgClass],
  templateUrl: 'app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  showHeader = true;

  constructor(private router: Router) {

  }

  ngOnInit() {
    this.router.events.subscribe(
      (val) =>{
        if(val instanceof NavigationEnd){
          if(val.url == '/login' || val.url == '/register' || val.url == '/' || val.url == '/home'){
            this.showHeader = false;
          }
        }else{
          this.showHeader = true;
        }
      }
    )
  }
}
