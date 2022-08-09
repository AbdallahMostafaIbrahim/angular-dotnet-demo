import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs';
import { LoginInput } from '../../_models/AuthInput';
import { AuthService } from '../../_services/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss', '../../../assets/form.scss'],
})
export class LoginComponent implements OnInit {
  loginData: LoginInput = { email: '', password: '' };
  err: string = '';
  returnUrl: string = this.route.snapshot.queryParams['returnUrl'] || '/';
  loading = false;

  constructor(
    private auth: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {}

  submit() {
    this.loading = true;
    this.auth.login(this.loginData).subscribe({
      error: (error: HttpErrorResponse) => {
        this.err = error.error.message || 'Unknown error';
        this.loading = false;
      },
      next: () => {
        this.router.navigate([this.returnUrl]);
      },
    });
  }
}
