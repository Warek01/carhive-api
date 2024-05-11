using System.Text;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using FafCarsApi.Models;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;


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
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
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

  OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
  {
    Name = "Bearer",
    BearerFormat = "JWT",
    Scheme = "bearer",
    Description = "Specify the authorization token.",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.Http,
  };
  options.AddSecurityDefinition("jwt_auth", securityDefinition);

  OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
  {
    Reference = new OpenApiReference()
    {
      Id = "jwt_auth",
      Type = ReferenceType.SecurityScheme
    }
  };
  OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
  {
    {securityScheme, new string[] { }},
  };
  options.AddSecurityRequirement(securityRequirements);
});

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

builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
  options.TokenValidationParameters = new TokenValidationParameters
  {
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = false,
    ValidateIssuerSigningKey = true
  };
});
builder.Services.AddAuthorization();

builder.Services.AddDbContext<FafCarsDbContext>(options =>
{
  options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

AppServices.Register(builder);

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseFileServer();
app.UseStaticFiles(new StaticFileOptions
{
  RequestPath = "/api/file",
  HttpsCompression = HttpsCompressionMode.Compress,
  ServeUnknownFileTypes = false,
  FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
});
app.UseCors(options =>
{
  options.AllowAnyOrigin();
  options.AllowAnyHeader();
  options.AllowAnyMethod();
});
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute("Default", "{controller}/{action}/{id?}");
app.Run();
