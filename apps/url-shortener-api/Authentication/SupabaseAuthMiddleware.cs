using System.IdentityModel.Tokens.Jwt;

namespace UrlShortenerApi.Authentication
{
  public class SupabaseAuthMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly Supabase.Client _supabaseClient;
    private readonly ILogger<SupabaseAuthMiddleware> _logger;

    public SupabaseAuthMiddleware(RequestDelegate next, Supabase.Client supabaseClient, ILogger<SupabaseAuthMiddleware> logger)
    {
      _next = next;
      _supabaseClient = supabaseClient;
      _logger = logger; 
    }

    public async Task Invoke(HttpContext context)
    {
      var authHeader = context.Request.Headers.Authorization.FirstOrDefault();

      if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer"))
      {
        var token = authHeader.Substring("Bearer ".Length).Trim();

        try
        {
          var handler = new JwtSecurityTokenHandler();
          var jwtToken = handler.ReadJwtToken(token);

          var userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

          if (!string.IsNullOrEmpty(userId))
          {
            context.Items["SupabaseUserId"] = userId; // Store in HttpContext
          }
        }
        catch (Exception ex)
        {
          context.Response.StatusCode = StatusCodes.Status401Unauthorized;
          _logger.LogWarning("Invalid JWT Token: {Message}", ex.Message);
          await context.Response.WriteAsync("Invalid JWT Token: " + ex.Message);
          return;
        }
      }
      await _next(context);
    }
  }
}
