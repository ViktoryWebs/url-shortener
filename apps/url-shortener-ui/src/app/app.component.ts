import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from './shared/header.component';
import { UrlShortenerComponent } from './pages/url-shortener.component';

@Component({
  imports: [RouterModule, HeaderComponent, UrlShortenerComponent],
  selector: 'app-root',
  template: `
    <app-header></app-header>
    <div class="container mt-4">
      <app-url-shortener></app-url-shortener>
      <router-outlet></router-outlet>
    </div>
  `,
})
export class AppComponent {
  title = 'url-shortener-ui';
}
