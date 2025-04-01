using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Authentication;
using UrlShortenerApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddDbContext<UrlShortenerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SupabaseConnection")));

builder.Services.AddSingleton(_ => new Supabase.Client(
  "https://gedczgdsomrjamxekcnr.supabase.co",
  "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImdlZGN6Z2Rzb21yamFteGVrY25yIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDE1OTA0NTIsImV4cCI6MjA1NzE2NjQ1Mn0.lm_n07mxnxHrd9aWjk37wJvwPplDaKvpcLesHYeqt2w"
));

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAngular",
      policy =>
      {
        policy.WithOrigins("http://localhost:4200") // TODO: Update in prod
                .AllowAnyMethod()
                .AllowAnyHeader();
      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseMiddleware<SupabaseAuthMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();
