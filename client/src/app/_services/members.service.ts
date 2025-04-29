import { HttpClient } from '@angular/common/http';
import { inject, Injectable, resource, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { lastValueFrom, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  oldMembers = signal<Member[]>([]);

  /* SIGNAL-RESOURCE APPROACH */
  username = signal<string>('');
  member = resource<Member, string>({
    request: () => this.username(),
    loader: async (): Promise<Member> => {
      const cached = this.members
        .value()
        ?.find((m) => m.username === this.username());
      if (cached) return cached;

      const member = await lastValueFrom(
        this.http.get<Member>(this.baseUrl + 'users/' + this.username())
      );
      return member;
    },
  });

  members = resource<Member[], never>({
    // No request key needed if it never changes
    loader: async (): Promise<Member[]> => {
      return await lastValueFrom(
        this.http.get<Member[]>(this.baseUrl + 'users')
      );
    },
  });

  updateMember(member: Member) {
    // tap sideffect to update the members signal after the update
    return this.http.put(this.baseUrl + 'users', member).pipe(
      tap(() => {
        this.members.update((members) => {
          return members?.map((m) => {
            return m.username === member.username ? member : m;
          });
        });

        if (this.username() === member.username) {
          this.member.update(() => member);
        }
      })
    );
  }

  /* SIGNAL-RESOURCE APPROACH */

  /* PRE-19 APPROACH */
  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'users').subscribe({
      next: (members) => this.oldMembers.set(members),
    });
  }

  getMember(username: string) {
    const member = this.oldMembers().find((x) => x.username === username);
    if (member !== undefined) return of(member); // Return the member if found in the local array
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  /* PRE-19 APPROACH */
}
