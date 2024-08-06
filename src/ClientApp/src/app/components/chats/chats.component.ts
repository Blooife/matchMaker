import {Component, OnInit, HostListener, OnDestroy, ChangeDetectorRef} from '@angular/core';
import {ChatDto} from "../../dtos/chat/ChatDto";
import {MatchService} from "../../services/match-service.service";
import {DatePipe, NgClass, NgForOf, NgIf, NgStyle} from "@angular/common";
import {ChatMessagesComponent} from "./messages/messages.component";
import {FormsModule} from "@angular/forms";
import {ProfileService} from "../../services/profile-service.service";
import {Subscription} from "rxjs";
import {ProfileDto} from "../../dtos/profile/ProfileDto";
import {ActivatedRoute, Router} from "@angular/router";
import {CreateChatDto} from "../../dtos/chat/CreateChatDto";
import {InfiniteScrollDirective} from "ngx-infinite-scroll";
import {ProfileCardComponent} from "../profile/profile-card/profile-card.component";

@Component({
  selector: 'app-chats',
  templateUrl: './chats.component.html',
  styleUrls: ['./chats.component.css'],
  imports: [
    NgForOf,
    ChatMessagesComponent,
    NgIf,
    FormsModule,
    DatePipe,
    NgClass,
    InfiniteScrollDirective,
    ProfileCardComponent,
    NgStyle,
  ],
  standalone: true
})
export class ChatsComponent implements OnInit, OnDestroy {
  chats: ChatDto[] = [];
  pageNumber: number = 1;
  pageSize: number = 12;
  profileId: string = '';
  isLoading: boolean = false;
  pagination: any = {};
  selectedChat?: ChatDto;
  profile: ProfileDto | null = null;
  selectedProfile: ProfileDto | null = null;
  showContextMenu = false;
  contextMenuPosition = { x: 0, y: 0 };
  selectedChatId?: string;
  private subscriptions: Subscription[] = [];

  constructor(private matchService: MatchService,
              private profileService:ProfileService,
              private route: ActivatedRoute) {

  }

  ngOnInit(): void {
    const sub = this.profileService.profile$.subscribe(
      (profileDto) =>{
        this.profile = profileDto;
        this.profileId = profileDto!.id;
      }
    )
    this.subscriptions.push(sub);

    this.route.queryParams.subscribe(params => {
      const firstProfileId = params['firstProfileId'];
      const secondProfileId = params['secondProfileId'];
      if (firstProfileId && secondProfileId) {
        this.startOrCreateChat(firstProfileId, secondProfileId);
      }
    });
    this.loadChats();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub=>sub.unsubscribe());
  }

  loadChats(): void {
    this.isLoading = true;
    this.matchService.getPaginatedChats(this.pageSize, this.pageNumber, this.profile!.id).subscribe({
      next: (response) => {
        this.chats.push(...response.chats);
        this.pagination = response.pagination;
      },
      error: () => {
        this.isLoading = false;
      },
      complete:() => {
        this.isLoading = false;
      }
    });
  }

  openChat(chat: ChatDto): void {
    this.closeModal();
    this.selectedChat = chat;
  }

  closeModal(): void {
    this.selectedChat = undefined;
    this.selectedProfile = null;
  }

  startOrCreateChat(firstProfileId: string, secondProfileId: string): void {
    this.matchService.getChatByProfilesIds(firstProfileId, secondProfileId).subscribe({
      next: (chat) => {
        this.selectedChat = chat;
      },
      error: () => {
        const createChatDto: CreateChatDto = {
          firstProfileId: firstProfileId,
          secondProfileId: secondProfileId,
        };

        this.matchService.createChat(createChatDto).subscribe({
          next: (newChat) => {
            this.selectedChat = newChat;
          }
        });
      }
    });
  }

  onScrollDown(): void {
    console.log(!this.isLoading && this.pagination.HasNext)
    if (!this.isLoading && this.pagination.HasNext) {
      this.pageNumber++;
      this.loadChats();
    }
  }

  viewProfile(profileId: string): void {
    this.profileService.getProfileById(profileId).subscribe({
        next: (profile) => {
          this.selectedProfile = profile
        }
      }
    )
  }

  closeProfileModal(){
    this.selectedProfile = null;
  }

  deleteChat(chatId: string){
    this.matchService.deleteChatById(chatId).subscribe(
      () => {
        this.chats = this.chats.filter(chat => chat.id !== chatId);
      }
    )
  }

  onRightClick(event: MouseEvent, chat: ChatDto): void {
    event.preventDefault();
    this.selectedChatId = chat.id;
    this.contextMenuPosition = { x: event.clientX, y: event.clientY };
    this.showContextMenu = true;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    this.showContextMenu = false;
  }
}
