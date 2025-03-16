import { Component } from '@angular/core';

@Component({
  selector: 'app-header',
  template: `
    <nav class="nav-bar">
      <div class="container h-20 text-center flex items-center justify-between">
        <button class="icon-btn"><i class="material-icons">apps</i></button>
        <a class="text-4xl font-medium" href="/">Minify URL</a>
        <button class="icon-btn"><i class="material-icons">dark_mode</i></button>
      </div>
    </nav>
  `,
})
export class HeaderComponent {}
