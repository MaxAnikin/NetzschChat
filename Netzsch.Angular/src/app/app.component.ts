import { Component } from '@angular/core';
import {ChatServiceService, Message} from "./chat-service.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  email: string = "";
  password: string = "";
  messages: Array<Message>[] = [];

  constructor(private chatService: ChatServiceService) {
  }

  GetMessages() {
    this.chatService.getMessages(this.email, this.password);
  }
}
