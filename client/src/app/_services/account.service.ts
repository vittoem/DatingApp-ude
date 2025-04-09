import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { map } from 'rxjs';
import { User } from '../_models/user';

// Singleton created when the application starts.
@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private http = inject(HttpClient);
  baseUrl = 'http://localhost:5028/api/';
  currentUser = signal<User | null>(null); // Signal to track the current user.

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((user) => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user)); // Store the user in local storage.
          this.currentUser.set(user); // Set the current user signal.
        }
      })
    );
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map((user) => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user)); // Store the user in local storage.
          this.currentUser.set(user); // Set the current user signal.
        }
        return user;
      })
    );
  }

  logout() {
    localStorage.removeItem('user'); // Remove the user from local storage.
    this.currentUser.set(null); // Reset the current user signal.
  }
}
