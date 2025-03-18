using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace UrlShortenerApi.Models.Domain
{
  [Index(nameof(ShortCode))]
  public class ShortUrl
  {
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string OriginalUrl { get; set; } = string.Empty;

    [Required]
    public string ShortCode { get; set; } = string.Empty;

    public int ClickCount { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  }
}
