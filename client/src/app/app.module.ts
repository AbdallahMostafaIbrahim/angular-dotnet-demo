import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatIconModule } from '@angular/material/icon';
import { HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './home/home.component';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { httpInterceptorProviders } from './http-interceptors';
import { LoginComponent } from './_components/login/login.component';
import { TodosComponent } from './_components/todos/todos.component';
import { PostsComponent } from './_components/posts/posts.component';
import { RegisterComponent } from './_components/register/register.component';
import { AuthService } from './_services/auth/auth.service';
import { LogoutComponent } from './_components/logout/logout.component';
import { AlertComponent } from './_components/alert/alert.component';

@NgModule({
  declarations: [
    AppComponent,
    TodosComponent,
    HomeComponent,
    PostsComponent,
    LoginComponent,
    RegisterComponent,
    LogoutComponent,
    AlertComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MatIconModule,
    MatButtonModule,
    MatInputModule,
    MatCardModule,
    ReactiveFormsModule,
  ],
  providers: [httpInterceptorProviders, AuthService],
  bootstrap: [AppComponent],
})
export class AppModule {}
