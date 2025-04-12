import { Component, inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  private accountService = inject(AccountService); // Inject the AccountService to handle user registration.
  private toastr = inject(ToastrService);
  // @Input() usersFromHome: any;
  usersFromHome = input.required<any>(); // input signal to receive data from the parent component.
  // @Output() cancelRegister = new EventEmitter();
  cancelRegister = output<boolean>(); // output signal to emit events to the parent component.
  model: any = {};

  register() {
    console.log(this.model);
    this.accountService.register(this.model).subscribe({
      next: (response) => {
        console.log(response);
        this.cancel(); // Call the cancel method to emit the cancel event.
      },
      error: (error) => {
        this.toastr.error(error.error);
      },
    });
  }

  cancel() {
    this.cancelRegister.emit(false); // Emit the cancel event to the parent component.
  }
}
