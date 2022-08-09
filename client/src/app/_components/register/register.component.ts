import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/_services/auth/auth.service';
import { RegisterInput } from '../../_models/AuthInput';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss', '../../../assets/form.scss'],
})
export class RegisterComponent implements OnInit {
  registerData: RegisterInput = { email: '', password: '', username: '' };
  err: string = '';
  returnUrl: string = this.route.snapshot.queryParams['returnUrl'] || '/';
  loading = false;

  constructor(
    private auth: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {}

  submit() {
    this.loading = true;
    this.auth.register(this.registerData).subscribe({
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
