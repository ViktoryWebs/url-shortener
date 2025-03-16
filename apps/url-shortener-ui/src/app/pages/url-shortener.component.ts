import { Component } from '@angular/core';

@Component({
  selector: 'app-url-shortener',
  template: `
    <div class="card space-y-5">
      <h3 class="text-3xl font-medium">Shorten a long URL</h3>
      <div>
        <label class="text-gray-600 dark:text-gray-300 text-xl font-medium" for="url">Enter your link here...</label>
        <input id="url" class="form-control w-full" placeholder="https://www.example.com/a-very-long-url">
      </div>
      <button class="flat-btn-primary font-bold">Generate Short URL</button>
    </div>
  `,
})
export class UrlShortenerComponent {}
