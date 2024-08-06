import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import {RegisterComponent} from "./components/register/register.component";
import {HomeComponent} from "./components/home/home.component";
import {AdminPanelComponent} from "./components/admin-panel/admin-panel.component";
import {ProfileComponent} from "./components/profile/profile.component";
import {CreateProfileComponent} from "./components/profile/create-profile/create-profile.component";
import {UpdateLanguagesComponent} from "./components/profile/update-languages/update-languages.component";
import {UpdateInterestsComponent} from "./components/profile/update-interests/update-interests.component";
import {UpdateEducationComponent} from "./components/profile/update-education/update-education.component";
import {UpdateProfileComponent} from "./components/profile/update-profile/update-profile.component";
import {AdminGuard} from "./guards/admin-guard";
import {LoggedInGuard} from "./guards/loggedIn-guard";
import {FindMatchComponent} from "./components/find-match/find-match.component";
import {ProfileCardComponent} from "./components/profile/profile-card/profile-card.component";
import {MatchesComponent} from "./components/matches/matches.component";
import {ProfileCreatedGuard} from "./guards/profileCreated-guard";
import {UpdateImagesComponent} from "./components/profile/update-images/update-images.component";
import {ChatsComponent} from "./components/chats/chats.component";
import {ChatMessagesComponent} from "./components/chats/messages/messages.component";

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'admin-panel', component: AdminPanelComponent, canActivate: [AdminGuard] },
  { path: 'profile', component: ProfileComponent, canActivate: [ProfileCreatedGuard] },
  { path: 'create-profile/:userId', component: CreateProfileComponent },
  { path: 'update-profile/:profileId', component: UpdateProfileComponent },
  { path: 'update-languages/:profileId', component: UpdateLanguagesComponent},
  { path: 'update-images/:profileId', component: UpdateImagesComponent },
  { path: 'update-interests/:profileId', component: UpdateInterestsComponent},
  { path: 'update-education/:profileId', component: UpdateEducationComponent },
  { path: 'profile-card', component: ProfileCardComponent },
  { path: 'find-match', component: FindMatchComponent, canActivate: [ProfileCreatedGuard] },
  { path: 'matches', component: MatchesComponent, canActivate: [ProfileCreatedGuard] },
  { path: 'chats', component: ChatsComponent, canActivate: [ProfileCreatedGuard] },
  { path: 'home', component: HomeComponent, canActivate: [LoggedInGuard] },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
];
