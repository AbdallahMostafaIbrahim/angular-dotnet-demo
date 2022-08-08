import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  creds: { email: string; password: string } = { email: '', password: '' };

  constructor(private auth: AuthService) {}

  submit() {
    this.auth.login(this.creds);
  }

  ngOnInit(): void {
    console.log(this.auth.isUserLoggedIn);
  }
}
