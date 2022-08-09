import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, map, Observable } from 'rxjs';
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
  private currentUserSubject: BehaviorSubject<User | undefined>;
  public currentUser: Observable<User | undefined>;
  token?: string;

  constructor(private httpClient: HttpClient, private router: Router) {
    const value = JSON.parse(localStorage.getItem('currentUser') || '{}');
    this.currentUserSubject = new BehaviorSubject<User | undefined>(
      Object.keys(value).length === 0 ? undefined : value
    );
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User | undefined {
    return this.currentUserSubject.value;
  }

  login(body: LoginParams) {
    return this.httpClient.post<any>(`${API_ENDPOINT}Auth/login`, body).pipe(
      map((user) => {
        localStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user);
        return user;
      })
    );
  }

  register(body: RegisterParams) {
    return this.httpClient.post<any>(`${API_ENDPOINT}Auth/register`, body).pipe(
      map((user) => {
        localStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user);
        return user;
      })
    );
  }

  logout() {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(undefined);
  }

  me() {
    return this.httpClient.get<NetworkResult>(`${API_ENDPOINT}Auth/me`);
  }
}
