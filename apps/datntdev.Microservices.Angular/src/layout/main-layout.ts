import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'main-layout',
  imports: [RouterModule],
  templateUrl: './main-layout.html',
  host: { class: 'd-flex flex-column flex-root' }
})
export class MainLayout implements OnInit {
  public ngOnInit(): void {
    document.body.setAttribute('data-kt-app-layout', 'dark-sidebar');
    document.body.setAttribute('data-kt-app-sidebar-fixed', 'true');
  }
}
