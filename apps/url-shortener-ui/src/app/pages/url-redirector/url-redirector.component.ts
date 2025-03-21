import { Component, Inject, inject, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { UrlShortenerService } from "../../services/url-shortener.service";
import { RedirectUrlResponse } from "../../../models/redirect-url-response.model";
import { DOCUMENT } from "@angular/common";

@Component({
  selector: 'app-url-redirector',
  templateUrl: './url-redirector.component.html',
})
export class UrlRedirectorComponent implements OnInit {

  private readonly _route = inject(ActivatedRoute);
  private readonly _urlShortenerService = inject(UrlShortenerService);
  private shortCode!: string | null;
  private redirectUrl!: RedirectUrlResponse;

  constructor(@Inject(DOCUMENT) private _document: Document) { }

  ngOnInit(): void {
    this._route.paramMap.subscribe({
      next: params => {
        this.shortCode = params.get('shortCode');
        console.log(this.shortCode);
      }
    });
    if (this.shortCode !== null) {
      this._urlShortenerService.redirectToOriginalUrl(this.shortCode).subscribe({
        next: response => {
          this.redirectUrl = response;
          console.log(this.redirectUrl);
        },
        error: err => console.error(err),
        complete: () => this._document.location.href = this.redirectUrl.originalUrl
      })
    }
  }
}
