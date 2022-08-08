import { Component } from '@angular/core';
import { AuthService } from './_services/auth/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  title = 'Application';

  constructor(public auth: AuthService) {}

  ngOnInit() {
    this.auth.me();
  }
}
