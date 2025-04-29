import { Component, effect, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { ActivatedRoute } from '@angular/router';
import { Member } from '../../_models/member';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';

@Component({
  selector: 'app-member-detail',
  imports: [TabsModule, GalleryModule],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css',
})
export class MemberDetailComponent implements OnInit {
  private memberService = inject(MembersService);
  private route = inject(ActivatedRoute);
  member?: Member;
  images: GalleryItem[] = [];

  constructor() {
    // Auto-react when member changes
    effect(() => {
      const memberResource = this.memberService.member;

      if (memberResource.isLoading()) {
        console.log('Loading...');
      } else if (memberResource.error()) {
        console.error('Error:', memberResource.error());
      } else {
        this.member = memberResource.value();
        this.member?.photos.map((photo) => {
          this.images.push(new ImageItem({ src: photo.url, thumb: photo.url }));
        });
        console.log('Loaded member:', this.member);
      }
    });
  }

  ngOnInit(): void {
    // this.loadMember();
    const username = this.route.snapshot.paramMap.get('username');
    this.memberService.username.set(username!);
  }

  loadMember() {
    const username = this.route.snapshot.paramMap.get('username');
    if (username) {
      this.memberService.getMember(username).subscribe({
        next: (member) => {
          this.member = member;
          member.photos.map((photo) => {
            this.images.push(
              new ImageItem({ src: photo.url, thumb: photo.url })
            );
          });
        },
      });
    }
  }
}
