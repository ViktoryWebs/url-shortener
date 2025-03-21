import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from './shared/header/header.component';

@Component({
  imports: [RouterModule, HeaderComponent],
  selector: 'app-root',
  template: `
    <app-header></app-header>
    <div class="container my-5">
      <router-outlet></router-outlet>
    </div>
  `,
})
export class AppComponent {
  title = 'url-shortener-ui';
}
