import {Component, OnInit} from '@angular/core';
import {TokenStorageService} from "../_services/token-storage.service";
import {ChatService} from "../_services/chat.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  form: any = {
    email: null,
    password: null
  };
  isLoggedIn = false;
  isLoginFailed = false;
  errorMessage: any;

  constructor(private tokenStorage: TokenStorageService, private chatService: ChatService, private router: Router) {
  }

  ngOnInit(): void {
    if (this.tokenStorage.getToken()) {
      this.isLoggedIn = true;
    }}

  onSubmit(): void {
    const { email, password } = this.form;
    console.log("Login: ");

    this.chatService.Login(email, password).subscribe(
      data => {

        const token = JSON.stringify(data).slice(1, -1);
        console.log("Logged in: " + email + " " + token);
        this.tokenStorage.saveUser(email);
        this.tokenStorage.saveToken(token);


        this.isLoginFailed = false;
        this.router.navigate(['/home']);
      },
      err => {
        this.errorMessage = err.message;
        this.isLoginFailed = true;
      }
    );
  }

  reloadPage(): void {
    window.location.reload();
  }
}
