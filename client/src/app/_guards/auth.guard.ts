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
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  async canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return (
      (await this.authService
        .me()
        .pipe(
          map((result) => {
            if (state.url == '/login' || state.url == '/register') {
              if (result.status == 200) {
                this.router.navigate(['/todos']);
              }
              return true;
            }
            this.authService.isUserLoggedIn = result.status === 200;
            console.log(this.authService.isUserLoggedIn);
            if (this.authService.isUserLoggedIn === true) {
              return true;
            } else {
              this.router.navigate(['/login']);
              return false;
            }
          })
        )
        .toPromise()) || false
    );
  }
}
