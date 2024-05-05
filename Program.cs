using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using FafCarsApi.Models;
using FafCarsApi.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(
  new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger()
);
builder.Host.UseSerilog(
  (context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc("v1", new OpenApiInfo
  {
    Version = "v1",
    Title = "FAF cars API",
    Description = "API for TUM web lab 7",
    Contact = new OpenApiContact
    {
      Name = "Alexandru Dobrojan",
      Email = "alexandrudobrojan@gmail.com",
      Url = new Uri("https://github.com/Warek01/faf-cars-api.git")
    }
  });
});
Services.Register(builder);

builder.Services.AddApiVersioning(options =>
{
  options.DefaultApiVersion = new ApiVersion(1);
  options.ReportApiVersions = true;
  options.AssumeDefaultVersionWhenUnspecified = true;
  options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
  options.GroupNameFormat = "'v'V";
  options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddDbContext<FafCarsDbContext>(options =>
{
  options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseSwagger();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
app.UseHttpsRedirection();
app.UseFileServer();
app.UseRouting();
app.UseCors(options =>
{
  options.AllowAnyOrigin();
  options.AllowAnyHeader();
  options.AllowAnyMethod();
});
app.MapControllerRoute("Default", "{controller}/{action}/{id?}");

app.Run();
