import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpResponse,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { AuthService } from '../_services/auth/auth.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private auth: AuthService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      map((value) => {
        if (value instanceof HttpResponse) {
          if (value.body.status === 200) return value;
          else if (value.body.status === 401) {
            this.auth.logout();
            throw new HttpErrorResponse({
              error: 'Unauthorized',
              headers: value.headers,
              status: 401,
              statusText: 'Warning',
              url: value.url || '',
            });
          } else {
            throw new HttpErrorResponse({
              error: value.body.message || 'Server Error',
              headers: value.headers,
              status: value.body.status || 500,
              statusText: 'Warning',
              url: value.url || '',
            });
          }
        }
        return value;
      })
    );
  }
}
