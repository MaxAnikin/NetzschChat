import {Component, OnInit} from '@angular/core';
import {TokenStorageService} from "./_services/token-storage.service";
import {Router} from "@angular/router";


class User {
  public email: string = "";
  public name: string = "";
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  title: string = "Netzsch chat";
  email: string = "";
  password: string = "";
  error: string = "";
  tokenStorage: TokenStorageService;

  constructor(tokenStorage: TokenStorageService, private router: Router) {
    this.tokenStorage = tokenStorage;
  }

  ngOnInit(): void {
  }

  logout() {
    this.tokenStorage.signOut();
    void this.router.navigate(['/login']);
  }
}
