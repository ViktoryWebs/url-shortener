using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Models;

namespace UrlShortenerApi.Data
{
  public class UrlShortenerDbContext : DbContext
  {
    public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : base(options) { }

    public DbSet<ShortUrl> Urls { get; set; }
  }
}
