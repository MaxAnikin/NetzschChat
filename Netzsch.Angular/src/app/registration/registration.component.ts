import { Component } from '@angular/core';
import {TokenStorageService} from "../_services/token-storage.service";
import {ChatService, User} from "../_services/chat.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent {
  form: any = {
    email: null,
    password: null,
    repeatPassword: null,
    name: null
  };
  errorMessage: any;
  isRegistrationFailed: any;

  constructor(private tokenStorage: TokenStorageService, private chatService: ChatService, private router: Router) {
  }

  onSubmit(): void {
    if(this.form.password != this.form.repeatPassword)
      this.errorMessage = "Passwords do not match";

    const user: User = { Email: this.form.email, Password: this.form.password, Name: this.form.name, Online: false };
    this.chatService.Register(user).subscribe(
      data => {

        console.log("Registered: " + JSON.stringify(user));
        this.chatService.Login(user.Email, user.Password).subscribe(
          data => {
            const token = JSON.stringify(data).slice(1, -1);
            console.log("Logged in: " + user.Email + " " + token);
            this.tokenStorage.saveUser(user.Email);
            this.tokenStorage.saveToken(token);

            this.router.navigate(['/home']);
          },
          err => {
            this.router.navigate(['/login']);
          }
        );
      },
      err => {
        this.isRegistrationFailed = true;
        this.errorMessage = err.message;
      }
    );
  }
}
