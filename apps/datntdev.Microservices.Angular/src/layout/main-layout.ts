import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { HeaderComponent } from './header/header';
import { Sidebar } from "./sidebar/sidebar";

@Component({
  selector: 'main-layout',
  imports: [RouterModule, PopoverModule,
    HeaderComponent, Sidebar],
  templateUrl: './main-layout.html',
  host: { class: 'd-flex flex-column flex-root' }
})
export class MainLayout implements OnInit {
  public ngOnInit(): void {
    document.body.setAttribute('data-kt-app-layout', 'dark-sidebar');
    document.body.setAttribute('data-kt-app-header-fixed', 'true');
    document.body.setAttribute('data-kt-app-sidebar-fixed', 'true');
    document.body.setAttribute('data-kt-app-sidebar-push-header', 'true');
  }
}
