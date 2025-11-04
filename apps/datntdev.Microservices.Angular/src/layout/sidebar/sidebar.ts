import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MENU } from '../../shared/models/menu';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.html',
  imports: [RouterModule],
  host: { class: 'app-sidebar flex-column' },
})
export class Sidebar {
  protected menu = MENU;
}
