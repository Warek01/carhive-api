using Asp.Versioning;
using FafCarsApi.Models;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

namespace FafCarsApi;

public static class Program
{
  private static WebApplicationBuilder _builder = null!;

  public static void Main(string[] args)
  {
    _builder = WebApplication.CreateBuilder(args);

    _builder.Host.UseSerilog(
      (context, c) =>
      {
        c.ReadFrom.Configuration(context.Configuration);
        c.MinimumLevel.Information();
        c.WriteTo.Console();
      }
    );

    _builder.Services.AddControllers();
    _builder.Services.AddSingleton<IConfiguration>(_builder.Configuration);

    _builder.Services.AddApiVersioning(options =>
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

    _builder.Services.AddDbContext<FafCarsDbContext>(options =>
    {
      options.UseNpgsql(_builder.Configuration.GetConnectionString("Default"));
    });

    SetupAuthentication();
    SetupSwagger();
    AppServices.Register(_builder);

    var app = _builder.Build();

    app.UseSerilogRequestLogging();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
    StaticFileService.SetupStaticFileServing(app);
    app.UseCors(options =>
    {
      options.AllowAnyOrigin();
      options.AllowAnyHeader();
      options.AllowAnyMethod();
    });
    AuthService.SetupAuthorization(app);
    app.MapControllerRoute("Default", "{controller}/{action}/{id?}");

    app.Run();
  }

  private static void SetupAuthentication()
  {
    _builder.Services.AddAuthentication(options =>
    {
      options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
      options.TokenValidationParameters = AuthService.GetTokenValidationParameters(_builder.Configuration);
      options.MapInboundClaims = false;
    });
    _builder.Services.AddAuthorization();
  }

  private static void SetupSwagger()
  {
    _builder.Services.AddEndpointsApiExplorer();
    _builder.Services.AddSwaggerGen(options =>
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

      OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme
      {
        Name = "Bearer",
        BearerFormat = "JWT",
        Scheme = "bearer",
        Description = "Specify the authorization token.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
      };
      options.AddSecurityDefinition("jwt_auth", securityDefinition);

      OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme
      {
        Reference = new OpenApiReference()
        {
          Id = "jwt_auth",
          Type = ReferenceType.SecurityScheme
        }
      };
      OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement
      {
        { securityScheme, new string[] { } },
      };
      options.AddSecurityRequirement(securityRequirements);
    });
  }
}