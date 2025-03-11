using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UrlShortenerApi.Data;
using UrlShortenerApi.Models.Domain;
using UrlShortenerApi.Models.DTO;

namespace UrlShortenerApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UrlController : ControllerBase
  {
    private readonly UrlShortenerDbContext _context;

    public UrlController(UrlShortenerDbContext context)
    {
        _context = context;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("shorten")]
    public async Task<IActionResult> ShortenUrl([FromBody] string originalUrl)
    {
      if (string.IsNullOrEmpty(originalUrl))
      {
        return BadRequest("URL cannot be empty");
      }

      if (!IsValidUrl(originalUrl))
      {
        return BadRequest("URL is invalid");
      }

      var existingUrl = await _context.Urls.FirstOrDefaultAsync(u => u.OriginalUrl == originalUrl);
      if (existingUrl != null)
      {
          return Ok(existingUrl.ShortCode);
      }

      long urlCount = await _context.Urls.CountAsync() + 1;
      string shortCode = GenerateShortCode(urlCount);
      ShortUrl shortUrl = new()
      {
        OriginalUrl = originalUrl,
        ShortCode = shortCode
      };

      _context.Urls.Add(shortUrl);
      await _context.SaveChangesAsync();

      var shortUrlDto = new ShortUrlDto() { ShortCode = shortCode };

      return Ok(shortUrlDto);
    }

    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{shortCode}")]
    public async Task<IActionResult> RedirectToOriginalUrl(string shortCode)
    {
      var urlEntry = await _context.Urls.FirstOrDefaultAsync(u => u.ShortCode == shortCode);
      if (urlEntry == null)
      {
        return NotFound("Short URL not found.");
      }

      urlEntry.ClickCount++;
      await _context.SaveChangesAsync();

      var originalUrlDto = new OriginalUrlDto() { OriginalUrl = urlEntry.OriginalUrl };

      return Ok(originalUrlDto);
    }

    // Generate Short Code using MD5 Hash of the counter
    private static string GenerateShortCode(long count)
    {
      byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(count.ToString());
      byte[] hashBytes = MD5.HashData(inputBytes);

      return Convert.ToHexString(hashBytes)[..7]; // Take first 7 chars
    }

    // URL Validation
    private static bool IsValidUrl(string url)
    {
      string pattern = @"^(https?:\/\/)?([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}(:\d{1,5})?(\/\S*)?$";
      return Regex.IsMatch(url, pattern, RegexOptions.IgnoreCase);
    }
  }
}
