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
    public async Task<IActionResult> ShortenUrl([FromBody] OriginalUrlRequestDto originalUrlDto)
    {
      if (string.IsNullOrEmpty(originalUrlDto.OriginalUrl))
      {
        return BadRequest("URL cannot be empty");
      }

      if (!IsValidUrl(originalUrlDto.OriginalUrl))
      {
        return BadRequest("URL is invalid");
      }

      ShortUrlResponseDto shortUrlResponseDto;

      var existingUrl = await _context.Urls.FirstOrDefaultAsync(u => u.OriginalUrl == originalUrlDto.OriginalUrl);
      if (existingUrl != null)
      {
        shortUrlResponseDto = new ShortUrlResponseDto()
        {
          OriginalUrl = existingUrl.OriginalUrl,
          ShortCode = existingUrl.ShortCode,
          CreatedAt = DateTime.UtcNow
        };
        return Ok(shortUrlResponseDto);
      }

      long urlCount = await _context.Urls.CountAsync() + 1;
      string shortCode = GenerateShortCode(urlCount);
      DateTime createdAt = DateTime.UtcNow;

      ShortUrl shortUrl = new()
      {
        OriginalUrl = originalUrlDto.OriginalUrl.Trim(),
        ShortCode = shortCode,
        CreatedAt = createdAt
      };

      _context.Urls.Add(shortUrl);
      await _context.SaveChangesAsync();

      shortUrlResponseDto = new ShortUrlResponseDto()
      {
        OriginalUrl = originalUrlDto.OriginalUrl.Trim(),
        ShortCode = shortCode,
        CreatedAt = createdAt
      };
      return Ok(shortUrlResponseDto);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
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

      var originalUrlDto = new OriginalUrlRequestDto() { OriginalUrl = urlEntry.OriginalUrl };

      return Ok(originalUrlDto);
    }

    #region utility methods
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
    #endregion
  }
}
