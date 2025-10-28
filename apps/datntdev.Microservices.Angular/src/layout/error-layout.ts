import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'error-layout',
  imports: [RouterModule],
  templateUrl: './error-layout.html',
  host: { "class": "d-flex flex-column flex-root" }
})
export class ErrorLayout { }
