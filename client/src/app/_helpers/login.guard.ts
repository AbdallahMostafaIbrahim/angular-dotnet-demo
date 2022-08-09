import { Injectable } from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
} from '@angular/router';
import { map } from 'rxjs';
import { AuthService } from '../_services/auth/auth.service';

@Injectable({
  providedIn: 'root',
})
export class LoginGuard implements CanActivate {
  constructor(private auth: AuthService, private router: Router) {}

  async canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const currentUser = this.auth.currentUserValue;
    if (!currentUser) {
      return true;
    }

    this.router.navigate(['/']);
    return false;
  }
}
