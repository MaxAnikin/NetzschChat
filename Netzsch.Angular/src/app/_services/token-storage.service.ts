import { Injectable } from '@angular/core';

const TOKEN_KEY = 'auth-token';
const USER_KEY = 'auth-user';

@Injectable({
  providedIn: 'root'
})
export class TokenStorageService {
  public isLoggedIn: boolean = false;

  constructor() {
    window.sessionStorage.clear();
  }

  signOut(): void {
    this.isLoggedIn = false;
    window.sessionStorage.clear();
  }

  public saveToken(token: string): void {
    console.log("save token: " + token);
    window.sessionStorage.removeItem(TOKEN_KEY);
    window.sessionStorage.setItem(TOKEN_KEY, token);
    this.isLoggedIn = true;
  }

  public getToken(): string | null {
    return window.sessionStorage.getItem(TOKEN_KEY);
  }

  public saveUser(email: string) {
    window.sessionStorage.setItem(USER_KEY, email);
  }

  public getUser(): string | null {
    return window.sessionStorage.getItem(USER_KEY);
  }
}
