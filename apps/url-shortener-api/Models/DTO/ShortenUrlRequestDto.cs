using System.ComponentModel.DataAnnotations;

namespace UrlShortenerApi.Models.DTO
{
  public class ShortenUrlRequestDto
  {
    [Required] 
    public string OriginalUrl { get; set; }
  }
}
