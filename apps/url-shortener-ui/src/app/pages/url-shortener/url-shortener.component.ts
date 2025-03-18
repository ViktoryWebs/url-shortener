import { CommonModule } from '@angular/common';
import { Clipboard } from '@angular/cdk/clipboard';
import { FormsModule, NgForm } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { Component, inject, ViewChild } from '@angular/core';

import { ShortenUrl } from '../../../models/shorten-url.model';
import { ShortUrlResponse } from '../../../models/short-url-response.model';
import { UrlShortenerService } from '../../services/url-shortener.service';

@Component({
  selector: 'app-url-shortener',
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './url-shortener.component.html'
})
export class UrlShortenerComponent {
  private readonly _urlShortenerService = inject(UrlShortenerService);
  private readonly _clipboard = inject(Clipboard);

  shortUrlResponse?: ShortUrlResponse;
  url: ShortenUrl = { originalUrl: '' };

  @ViewChild('shortenUrlForm', { static: false }) shortenUrlForm!: NgForm;

  shortenUrl(): void {
    if (this.shortenUrlForm.invalid) return;

    this._urlShortenerService
      .shortenUrl(this.url)
      .subscribe(shortUrlResponse => {
        this.shortenUrlForm.resetForm();
        this.shortUrlResponse = shortUrlResponse;
      });
  }

  copyShortUrlToClipboard(): void {
    if (this.shortUrlResponse?.shortCode === '') return;

    this._clipboard.copy(`localhost:4200/${this.shortUrlResponse?.shortCode}`);
  }
}
