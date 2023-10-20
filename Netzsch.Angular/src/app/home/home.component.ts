import {Component, OnDestroy, OnInit} from '@angular/core';
import {ChatService, User} from "../_services/chat.service";
import {TokenStorageService} from "../_services/token-storage.service";
import {Router} from "@angular/router";

const INTERVAL = 2000;  // <-- poll every 5 seconds
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, OnDestroy {
  users: any;
  selectedUser: any;
  messages: any;
  errorMessage: any;
  selectedUserEmail: any;
  message: any;

  constructor(public tokenStorage: TokenStorageService, private chatService: ChatService, private router: Router) {
  }

  onUserChange() {
    this.RefreshMessages();
  }

  ngOnInit(): void {
    this.RefreshUsers();

    const timer = setInterval(() => {
      this.RefreshMessages()
    }, INTERVAL);
  }

  private RefreshUsers() {
    this.chatService.GetUsers().subscribe({
      next: (v) => {
        this.users = v;
        this.selectedUserEmail = this.users.length > 0 ? this.users[0].email : null;
        this.RefreshMessages();
      },
      error: (e) => {
        this.errorMessage = e.error.message;
      },
    });
  }

  private RefreshMessages() {

    console.log("Selected user: " + JSON.stringify(this.selectedUserEmail));
    this.chatService.GetMessages(this.tokenStorage.getUser() ?? "", this.selectedUserEmail).subscribe({
      next: (v) => {
        this.messages = v;
      },
      error: (e) => {
        this.errorMessage = e.error.message;
      },
    });
  }

  SendMessage() {
    this.chatService.SendMessage(this.tokenStorage.getUser() ?? "", this.selectedUserEmail, this.message).subscribe({
      next: (v) => {
        this.RefreshMessages();
      },
      error: (e) => {
        this.errorMessage = e.error.message;
      },
    });
  }

  ngOnDestroy(): void {
  }
}


