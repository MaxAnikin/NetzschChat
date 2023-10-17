import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'

export class Message{
  constructor(public from: string,
              public to: string,
              public message: string)
  { }
}

@Injectable({
  providedIn: 'root'
})
export class ChatServiceService {
  usersUrl:string = "https://localhost:7147/users";
  tokenUrl:string = "https://localhost:7147/token";
  token: string = "";

  constructor(private http: HttpClient) {

  }

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  public getMessages(email: string, password: string): Array<String> {
    this.http.post(this.tokenUrl, { email, password }, this.httpOptions);

    let fruits: Array<string>;
    fruits = ['Apple', 'Orange', 'Banana'];
    return fruits;
  }
}
