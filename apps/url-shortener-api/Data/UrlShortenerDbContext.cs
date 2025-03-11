using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Models.Domain;

namespace UrlShortenerApi.Data
{
  public class UrlShortenerDbContext : DbContext
  {
    public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : base(options) { }

    public DbSet<ShortUrl> Urls { get; set; }
  }
}
