import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class Login {

  email = '';
  password = '';
  isRegister = false;
  name = '';
  loading = false;
  errorMsg = '';

  constructor(private http: HttpClient, private router: Router) {}

  login() {
    this.loading = true;
    this.errorMsg = '';
    const body = { email: this.email, password: this.password };
    this.http.post('https://localhost:7177/api/auth/login', body)
      .subscribe({
        next: (res: any) => {
          localStorage.setItem('token', res.token);
          this.router.navigate(['/home']);
        },
        error: () => {
          this.errorMsg = 'Invalid credentials. Please try again.';
          this.loading = false;
        }
      });
  }

  register() {
    this.loading = true;
    this.errorMsg = '';
    const body = { name: this.name, email: this.email, password: this.password };
    this.http.post('https://localhost:7177/api/auth/register', body)
      .subscribe({
        next: () => {
          this.isRegister = false;
          this.loading = false;
          this.errorMsg = '';
        },
        error: () => {
          this.errorMsg = 'Registration failed. Try again.';
          this.loading = false;
        }
      });
  }

  toggle() {
    this.isRegister = !this.isRegister;
    this.errorMsg = '';
  }

  submit() {
    if (this.isRegister) this.register();
    else this.login();
  }
}
