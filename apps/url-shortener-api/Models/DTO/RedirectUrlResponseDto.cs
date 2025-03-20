namespace UrlShortenerApi.Models.DTO
{
  public class RedirectUrlResponseDto
  {
    public string OriginalUrl { get; set; }

    public string ShortCode { get; set; }

    public DateTime CreatedAt { get; set; }
  }
}
