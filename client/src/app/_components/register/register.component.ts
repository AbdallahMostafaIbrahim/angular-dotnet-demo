import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/auth/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  creds: { username: string; email: string; password: string } = {
    username: '',
    email: '',
    password: '',
  };

  constructor(private auth: AuthService) {}

  submit() {
    this.auth.register(this.creds);
  }

  ngOnInit(): void {}
}
