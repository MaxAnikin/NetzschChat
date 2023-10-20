import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http'
import {Observable} from "rxjs";
import {TokenStorageService} from "./token-storage.service";

export class Message{
  constructor(public createdDate: Date, public fromEmail: string,
              public toEmail: string,
              public text: string)
  { }
}

export class User{
  constructor(public Email: string, public Password: string, public Name: string, public Online: boolean = false)
  { }
}


class TokenRequest {
  public email: string = "";
  public password: string = "";
}

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  usersUrl:string = "https://localhost:7147/api/users";
  messagesUrl:string = "https://localhost:7147/api/messages";
  tokenUrl:string = "https://localhost:7147/api/token";

  constructor(private http: HttpClient, private tokenService: TokenStorageService) {
  }

  public Login(email: string, password: string): Observable<any> {
    const tokenRequest: TokenRequest = { email: email, password: password };
    return this.http.post(this.tokenUrl, tokenRequest, {responseType: 'text'});
  }

  GetMessages(from: string, to: string): Observable<any> {
    let params = new HttpParams().append('from', from).append('to', to);

    console.log("GetMessages: " + new Date() + ": " + JSON.stringify(params));
    return this.http.get(this.messagesUrl, { params: params });
  }

  GetUsers(): Observable<any> {
    let params = new HttpParams().append('exceptEmail', this.tokenService.getUser() ?? "");

    console.log("GetUsers: " + new Date() + ": " + JSON.stringify(params));
    return this.http.get(this.usersUrl, { params: params});
  }

  SendMessage(fromEmail: string, toEmail: string, message: string) {
    let messageBody = { toEmail: toEmail, fromEmail: fromEmail, text: message };
    console.log("Send Message: " + new Date() + ": " + JSON.stringify(messageBody));
    return this.http.post(this.messagesUrl, messageBody);
  }

  Register(user: User) {
    let userBody = { email: user.Email, password: user.Password, name: user.Name, online: false };
    console.log("Send Message: " + new Date() + ": " + JSON.stringify(userBody));
    return this.http.post(this.usersUrl, userBody);
  }
}
