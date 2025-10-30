import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { filter } from 'rxjs/operators';
import { APPLICATION } from '@shared/models/constants';
import { MENU } from '@shared/models/menu';
import { AuthService } from '@shared/services/auth-service';

@Component({
  selector: 'app-header',
  imports: [CommonModule, PopoverModule],
  templateUrl: './header.html'
})
export class HeaderComponent implements OnInit {
  public avatarUrl: string = 'assets/media/avatars/main.png'
  public emailAddress: string = 'datntdev@outlook.com'
  public fullName: string = 'Dat Nguyen'
  
  public pageTitle: string | undefined
  public pageDescription: string | undefined

  constructor(private router: Router, private authService: AuthService) { }

  protected signOut(): void {
    this.authService.signOut({ post_logout_redirect_uri: window.location.origin });
  }

  protected userProfile = () => this.authService.userSignal()?.profile;

  protected userFullName() {
    return this.authService.userSignal()?.profile.name;
  }

  protected userEmailAddress() {
    return this.authService.userSignal()?.profile.sub;
  }

  ngOnInit(): void {
    // Set initial page info
    this.updatePageInfo(this.router.url);

    // Subscribe to route changes
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => this.updatePageInfo(event.urlAfterRedirects));
  }

  private updatePageInfo(url: string): void {
    // Find matching menu item by URL
    for (const section of MENU) {
      for (const item of section.items) {
        if (item.url && url.includes(item.url)) {
          this.pageTitle = item.title;
          this.pageDescription = item.description;
          this.updateHeadTitle(item.title);
          return;
        }
      }
    }
  }

  private updateHeadTitle(title: string): void {
    document.title = `${APPLICATION.name} - ${title}`;
  }
}
