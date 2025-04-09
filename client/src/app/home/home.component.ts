import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from '../register/register.component';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  // New way to inject services in Angular.
  http = inject(HttpClient);
  registerMode = false;
  users: any;

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle() {
    this.registerMode = !this.registerMode; // Toggle the register mode.
  }

  getUsers() {
    // Observable is a stream of data that can be subscribed to. It's lazy by default.
    // It will only execute when subscribed to. The subscribe method is used to execute the observable.
    // No need to unsubscribe from http requests as it always completes.
    this.http.get('https://localhost:5029/api/users').subscribe({
      next: (response) => {
        this.users = response;
      },
      error: (error) => {
        console.error(error);
      },
      complete: () => {
        console.log('Request completed');
      },
    });
  }

  cancelRegisterMode($event: boolean) {
    console.log($event); // Log the event emitted from the child component.
    this.registerMode = $event; // Set the register mode based on the event emitted from the child component.
  }
}
