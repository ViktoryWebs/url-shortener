import { DOCUMENT } from '@angular/common';
import { Component, Inject, Renderer2 } from '@angular/core';


enum THEME {
  light = 'light',
  dark = 'dark'
}

@Component({
  selector: 'app-header',
  template: `
    <nav class="nav-bar">
      <div class="container px-2 h-20 text-center flex items-center justify-between">
        <button class="icon-btn"><i class="material-icons">apps</i></button>
        <a class="text-4xl font-medium" href="/">Minify URL</a>
        <button class="icon-btn" (click)="toggleTheme()" title="Toggle Theme">
          @if(theme === 'light') {
            <i class="material-icons">dark_mode</i>
          }
          @else {
            <i class="material-icons">light_mode</i>
          }
        </button>
      </div>
    </nav>
  `,
})
export class HeaderComponent {
  THEME = THEME; // Expose enum to the template
  private _theme: THEME;

  constructor(
    private renderer: Renderer2,
    @Inject(DOCUMENT) private document: Document
  ) {
    this._theme = (localStorage.getItem('theme') as THEME) || THEME.light;
    this.applyTheme();
  }

  get theme(): THEME {
    return this._theme;
  }

  private set theme(value: THEME) {
    this._theme = value;
    localStorage.setItem('theme', value);
    this.applyTheme();
  }

  toggleTheme() {
    this.theme = this.theme === THEME.light ? THEME.dark : THEME.light;
  }

  private applyTheme() {
    this.renderer.setAttribute(this.document.body, 'data-theme', this._theme);
  }
}
