import { Component, HostListener, inject, ViewChild } from '@angular/core';
import { Member } from '../../_models/member';
import { AccountService } from '../../_services/account.service';
import { MembersService } from '../../_services/members.service';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-member-edit',
  imports: [TabsModule, FormsModule],
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css',
})
export class MemberEditComponent {
  // Selecting the template reference variable 'editForm' to access the form in the template
  // This allows us to use the form in the component class
  @ViewChild('editForm') editForm?: NgForm;
  // HostListener to listen for the 'beforeunload' event on the window object
  // This is used to show a confirmation dialog when the user tries to leave the page with unsaved changes
  @HostListener('window:beforeunload', ['$event']) notify($event: any) {
    if (this.editForm?.dirty) {
      $event.returnValue = true; // Show confirmation dialog
    }
  }
  member?: Member;
  private accountService = inject(AccountService);
  private memberService = inject(MembersService);
  private toastService = inject(ToastrService);

  ngOnInit() {
    this.loadMember();
  }

  loadMember() {
    const username = this.accountService.currentUser()?.username;
    if (username) {
      this.memberService.getMember(username).subscribe({
        next: (member) => (this.member = member),
        error: (error) => console.error(error),
      });
    }
  }

  updateMember() {
    this.memberService.updateMember(this.editForm?.value).subscribe({
      next: () => {
        this.toastService.success('Member updated successfully', 'Success');
        this.editForm?.reset(this.member); // Reset the form with the updated member data
      },
    });
  }
}
