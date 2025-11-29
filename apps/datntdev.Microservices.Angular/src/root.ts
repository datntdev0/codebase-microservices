import { Component } from '@angular/core';

@Component({
  selector: 'root-element',
  template: `<router-outlet /><app-toast />`,
  standalone: false,
  host: { "class": "d-flex flex-column flex-root" }
})
export class RootComponent {
}
