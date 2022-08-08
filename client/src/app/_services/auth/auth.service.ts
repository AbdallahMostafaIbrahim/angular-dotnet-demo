import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { API_ENDPOINT } from '../../lib/interfaces/constants';
import { NetworkResult } from '../../lib/interfaces/network';
import { User } from '../../lib/interfaces/user';

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

  constructor(private httpClient: HttpClient, private router: Router) {
    this.token = localStorage.getItem('token') || undefined;
  }

  login(body: LoginParams) {
    this.httpClient
      .post<NetworkResult>(`${API_ENDPOINT}Auth/login`, body)
      .subscribe((result) => {
        if (result.status === 200) {
          this.token = result['token'];
          localStorage.setItem('token', result['token']);
          this.isUserLoggedIn = true;
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
          this.token = result['token'];
          localStorage.setItem('token', result['token']);
          this.router.navigate(['/todos']);
        }
      });
  }

  logout() {
    console.log('logout');
    localStorage.removeItem('token');
    this.isUserLoggedIn = false;
    this.token = undefined;
    this.router.navigate(['/login']);
  }

  me() {
    return this.httpClient.get<NetworkResult>(`${API_ENDPOINT}Auth/me`);
  }
}
