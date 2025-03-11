using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UrlShortenerApi.Data;
using UrlShortenerApi.Models;

namespace UrlShortenerApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UrlController : ControllerBase
  {
    private readonly UrlShortenerDbContext _context;
    private static long count = 1L;

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

      string pattern = @"^https?://(?:[a-zA-Z0-9.-]+(?:\\.[a-zA-Z]{2,}))(?:\\/[^\\s]*)?$";
      Regex regex = new Regex(pattern);
      if (!regex.IsMatch(originalUrl))
      {
        return BadRequest("URL is invalid");
      }

      string shortCode = GenerateShortCode();
      ShortUrl shortUrl = new()
      {
        OriginalUrl = originalUrl,
        ShortCode = shortCode
      };

      _context.Urls.Add(shortUrl);
      await _context.SaveChangesAsync();

      return Ok(shortCode);
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

      return Ok(new { url = urlEntry.OriginalUrl });
    }

    private static string GenerateShortCode()
    {          
      byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(count.ToString());
      byte[] hashBytes = MD5.HashData(inputBytes);
      
      return Convert.ToHexString(hashBytes)[..7];
    }
  }
}
