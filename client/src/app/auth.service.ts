import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { API_ENDPOINT } from './lib/interfaces/constants';
import { NetworkResult } from './lib/interfaces/network';
import { User } from './lib/interfaces/user';

interface LoginParams {
  email: string;
  password: string;
}

interface RegisterParams extends LoginParams {
  username: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  isUserLoggedIn?: boolean;
  user?: User;
  token?: string;

  constructor(private httpClient: HttpClient, private router: Router) {}

  login(body: LoginParams) {
    this.httpClient
      .post<NetworkResult>(`${API_ENDPOINT}Auth/login`, body)
      .subscribe((result) => {
        if (result.status === 200) {
          this.isUserLoggedIn = true;
          localStorage.setItem('token', result['token']);
          this.router.navigate(['/todos']);
        }
      });
  }

  register(body: RegisterParams) {
    return this.httpClient
      .post<NetworkResult>(`${API_ENDPOINT}Auth/register`, body)
      .subscribe((result) => {
        if (result.status === 200) {
          this.isUserLoggedIn = true;
        }
        localStorage.setItem('token', result['token']);
      });
  }

  me() {
    this.httpClient
      .get<NetworkResult>(`${API_ENDPOINT}Auth/me`)
      .subscribe((result) => {
        this.isUserLoggedIn = result.status === 200;
      });
  }
}
