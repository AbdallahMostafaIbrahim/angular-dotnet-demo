import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs';
import { AlertService } from 'src/app/_services/alert/alert.service';
import { AuthService } from '../../_services/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup = this.formBuilder.group({
    email: ['', Validators.required],
    password: ['', Validators.required],
  });
  returnUrl: string = this.route.snapshot.queryParams['returnUrl'] || '/';
  loading = false;
  submitted = false;

  constructor(
    private auth: AuthService,
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private alertService: AlertService
  ) {}

  ngOnInit() {}

  get f() {
    return this.loginForm.controls;
  }

  submit() {
    this.submitted = true;

    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;
    this.auth
      .login({
        email: this.f['email'].value,
        password: this.f['password'].value,
      })
      .pipe(first())
      .subscribe({
        error: (error) => {
          this.alertService.error(error.error);
          this.loading = false;
        },
        next: () => {
          this.router.navigate([this.returnUrl]);
        },
      });
  }
}
