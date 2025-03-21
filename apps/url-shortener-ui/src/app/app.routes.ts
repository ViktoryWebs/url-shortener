import { Route } from '@angular/router';
import { UrlShortenerComponent } from './pages/url-shortener/url-shortener.component';
import { UrlRedirectorComponent } from './pages/url-redirector/url-redirector.component';

export const appRoutes: Route[] = [
  {
    path: '',
    component: UrlShortenerComponent
  },
  {
    path: ':shortCode',
    component: UrlRedirectorComponent
  }
];
