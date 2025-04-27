import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from './nav/nav.component';
import { AccountService } from './_services/account.service';
import { HomeComponent } from './home/home.component';
import { NgxSpinnerComponent } from 'ngx-spinner';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavComponent, HomeComponent, NgxSpinnerComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  private accountService = inject(AccountService);
  title = 'Dating App';
  users: any;

  ngOnInit(): void {
    this.setCurrentUser(); // Set the current user when the component is initialized.
  }

  setCurrentUser() {
    const userString = localStorage.getItem('user'); // Get the user from local storage.
    if (!userString) return;
    const user = JSON.parse(userString); // Parse the user string to an object.
    this.accountService.currentUser.set(user); // Set the current user signal.
  }
}
