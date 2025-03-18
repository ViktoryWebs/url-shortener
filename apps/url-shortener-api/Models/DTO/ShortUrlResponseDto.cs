using System.ComponentModel.DataAnnotations;

namespace UrlShortenerApi.Models.DTO
{
  public class ShortUrlResponseDto
  {
    public string OriginalUrl { get; set; } = string.Empty;

    public string ShortCode { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  }
}
