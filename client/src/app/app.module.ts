import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatIconModule } from '@angular/material/icon';
import { HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './_components/home/home.component';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { httpInterceptorProviders } from './_helpers/interceptors';
import { LoginComponent } from './_components/login/login.component';
import { TodosComponent } from './_components/todos/todos.component';
import { PostsComponent } from './_components/posts/posts.component';
import { RegisterComponent } from './_components/register/register.component';
import { AuthService } from './_services/auth/auth.service';

@NgModule({
  declarations: [
    AppComponent,
    TodosComponent,
    HomeComponent,
    PostsComponent,
    LoginComponent,
    RegisterComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MatIconModule,
    MatButtonModule,
    MatInputModule,
    MatCardModule,
    MatProgressSpinnerModule,
  ],
  providers: [httpInterceptorProviders, AuthService],
  bootstrap: [AppComponent],
})
export class AppModule {}
