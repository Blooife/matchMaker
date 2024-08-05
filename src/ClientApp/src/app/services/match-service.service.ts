import {Injectable} from "@angular/core";
import {HttpClient, HttpContext, HttpHeaders} from "@angular/common/http";
import {BehaviorSubject, Observable} from "rxjs";
import {
  chatsEndpoints,
  likesEndpoints,
  matchesEndpoints, messagesEndpoints,
  profilesEndpoints,
  usersEndpoints
} from "../constants/api-endpoints";
import {map, retry, tap} from "rxjs/operators";
import {AddLikeDto} from "../dtos/like/AddLikeDto";
import {LikeDto} from "../dtos/like/LikeDto";
import {ProfileDto} from "../dtos/profile/ProfileDto";
import {UpdateLocationDto} from "../dtos/match/UpdateLocationDto";
import {ChatDto} from "../dtos/chat/ChatDto";
import {MessageDto} from "../dtos/message/MessageDto";
import {CreateChatDto} from "../dtos/chat/CreateChatDto";
import {_IGNORED_STATUSES} from "../constants/http-context";

@Injectable({
  providedIn: 'root'
})
export class MatchService {
  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  private recsLoadingSubject = new BehaviorSubject<boolean>(true);
  recsLoading$ = this.recsLoadingSubject.asObservable();

  constructor(private httpClient: HttpClient) { }

  getRecs(profileId: string): Observable<ProfileDto[]> {
    this.recsLoadingSubject.next(true);

    return this.httpClient.get<ProfileDto[]>(`${profilesEndpoints.recs(profileId)}`,
      this.httpOptions).pipe(
      retry(2),
      tap(() => {
        this.recsLoadingSubject.next(false);
      })
    );
  }

  addLike(model: AddLikeDto){
    return this.httpClient.post<LikeDto>(`${likesEndpoints.likes}`, model, this.httpOptions)
      .pipe(
        retry(2),
      );
  }

  updateLocation(model: UpdateLocationDto){
    return this.httpClient.patch(`${profilesEndpoints.location}`, model, this.httpOptions)
      .pipe(
        retry(2),
      );
  }

  getPaginatedMatches(pageSize: number, pageNumber: number, profileId: string): Observable<{ matches: ProfileDto[], pagination: any }> {
    return this.httpClient.get<ProfileDto[]>(`${matchesEndpoints.paged(pageSize.toString(), pageNumber.toString(), profileId)}`,
      {
        ...this.httpOptions,
        observe: 'response'
      }).pipe(
      retry(2),
      map(response => {
        const pagination = JSON.parse(response.headers.get('X-Pagination')!);
        return {
          matches: response.body || [],
          pagination: pagination
        };
      }),
    );
  }

  getPaginatedChats(pageSize: number, pageNumber: number, profileId: string): Observable<{ chats: ChatDto[], pagination: any }> {
    return this.httpClient.get<ChatDto[]>(`${chatsEndpoints.paged(pageSize.toString(), pageNumber.toString(), profileId)}`,
      {
        ...this.httpOptions,
        observe: 'response'
      }).pipe(
      retry(2),
      map(response => {
        const pagination = JSON.parse(response.headers.get('X-Pagination')!);

        return {
          chats: response.body || [],
          pagination: pagination
        };
      }),
    );
  }

  getPaginatedMessages(pageSize: number, pageNumber: number, chatId: string): Observable<{ messages: MessageDto[], pagination: any }> {
    return this.httpClient.get<MessageDto[]>(`${messagesEndpoints.paged(pageSize.toString(), pageNumber.toString(), chatId)}`,
      {
        ...this.httpOptions,
        observe: 'response'
      }).pipe(
      retry(2),
      map(response => {
        const pagination = JSON.parse(response.headers.get('X-Pagination')!);
        return {
          messages: response.body || [],
          pagination: pagination
        };
      }),
    );
  }

  getChatByProfilesIds(firstProfileId: string, secondProfileId: string){
    return this.httpClient.get<ChatDto>(chatsEndpoints.chatsByIds(firstProfileId, secondProfileId), {
      headers: this.httpOptions.headers,
      context: new HttpContext().set(_IGNORED_STATUSES, true),
    })
      .pipe(
        retry(2),
      );
  }

  createChat(model: CreateChatDto){
    return this.httpClient.post<ChatDto>(chatsEndpoints.chats, model, this.httpOptions)
      .pipe(
        retry(2),
      );
  }
}
