import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { LoginInput, RegisterInput } from '../../_models/AuthInput';
import { API_ENDPOINT } from '../../_models/constants';
import { User } from '../../_models/user';

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

  login(body: LoginInput) {
    return this.httpClient.post<any>(`${API_ENDPOINT}Auth/login`, body).pipe(
      map((user) => {
        localStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user);
        return user;
      })
    );
  }

  register(body: RegisterInput) {
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
    return this.httpClient.get(`${API_ENDPOINT}Auth/me`);
  }
}
