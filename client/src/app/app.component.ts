import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  // New way to inject services in Angular.
  http = inject(HttpClient);
  title = 'Dating App';
  users: any;

  ngOnInit(): void {
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
}
