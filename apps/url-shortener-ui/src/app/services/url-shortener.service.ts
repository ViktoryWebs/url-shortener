import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ShortUrlResponse } from "../../models/short-url-response.model";
import { ShortenUrl } from "../../models/shorten-url.model";
import { RedirectUrlResponse } from "../../models/redirect-url-response.model";

@Injectable({
  providedIn: 'root'
})
export class UrlShortenerService {
  private readonly _http = inject(HttpClient);
  private readonly _backendHost = 'https://localhost:5000/api/url';

  public shortenUrl(url: ShortenUrl): Observable<ShortUrlResponse> {
    return this._http.post<ShortUrlResponse>(`${this._backendHost}/shorten`, url);
  }

  public redirectToOriginalUrl(shortCode: string): Observable<RedirectUrlResponse> {
    return this._http.get<RedirectUrlResponse>(`${this._backendHost}/${shortCode}`);
  }
}
