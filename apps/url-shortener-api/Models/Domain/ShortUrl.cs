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
    public string OriginalUrl { get; set; }

    [Required]
    public string ShortCode { get; set; }

    public int ClickCount { get; set; } = 0;

    public DateTime CreatedAt { get; set; }
  }
}
