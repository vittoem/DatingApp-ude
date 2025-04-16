import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  imports: [],
  templateUrl: './server-error.component.html',
  styleUrl: './server-error.component.css',
})
export class ServerErrorComponent {
  error: any;

  constructor(private router: Router) {
    // constructor is only place where we can get the extras.
    const navigation = this.router.getCurrentNavigation();
    if (navigation?.extras?.state) {
      this.error = navigation.extras.state['error'];
    } else {
      this.error = null;
    }
  }
}
