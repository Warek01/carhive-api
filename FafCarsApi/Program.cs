using System.Globalization;
using System.Reflection;
using Asp.Versioning;
using FafCarsApi.Config;
using FafCarsApi.Data;
using FafCarsApi.Enums;
using FafCarsApi.Helpers;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;
using Serilog;

namespace FafCarsApi;

public static class Program {
  private static WebApplicationBuilder _builder = null!;

  public static void Main(string[] args) {
    var culture = CultureInfo.InvariantCulture;
    CultureInfo.DefaultThreadCurrentCulture = culture;
    CultureInfo.DefaultThreadCurrentUICulture = culture;
    Thread.CurrentThread.CurrentCulture = culture;
    Thread.CurrentThread.CurrentUICulture = culture;
    
    _builder = WebApplication.CreateBuilder(args);

    _builder.Host.UseSerilog(
      (context, c) => {
        c.ReadFrom.Configuration(context.Configuration);
        c.MinimumLevel.Information();
        c.WriteTo.Console();
      }
    );

    _builder.Services.AddAutoMapper(typeof(MappingProfile));
    _builder.Services.AddControllers();
    _builder.Services.AddSingleton<IConfiguration>(_builder.Configuration);
    _builder.Services.Configure<KestrelServerOptions>(o => {
      o.Limits.MaxRequestBodySize = 50_000_000;
      o.Limits.MaxRequestLineSize = 8192;
    });

    _builder.Services.AddProblemDetails();
    _builder.Services.AddExceptionHandler<HttpExceptionHandler>();

    _builder.Services.AddApiVersioning(options => {
      options.DefaultApiVersion = new ApiVersion(1);
      options.ReportApiVersions = true;
      options.AssumeDefaultVersionWhenUnspecified = true;
      options.ApiVersionReader = new UrlSegmentApiVersionReader();
    }).AddApiExplorer(options => {
      options.GroupNameFormat = "'v'V";
      options.SubstituteApiVersionInUrl = true;
    });

    SetupDataSource();
    SetupAuthentication();
    SetupSwagger();
    AppServices.Register(_builder);

    var app = _builder.Build();

    app.UseSerilogRequestLogging();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
    app.UseExceptionHandler();
    StaticFileService.SetupStaticFileServing(app);
    app.UseCors(options => {
      options.AllowAnyOrigin();
      options.AllowAnyHeader();
      options.AllowAnyMethod();
    });
    AuthService.SetupAuthorization(app);
    app.MapControllerRoute("Default", "{controller}/{action}/{id?}");

    app.Run();
  }

  private static void SetupAuthentication() {
    _builder.Services.AddAuthentication(options => {
      options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options => {
      options.TokenValidationParameters = AuthService.GetTokenValidationParameters(_builder.Configuration);
      options.MapInboundClaims = false;
    });
    _builder.Services.AddAuthorization();
  }

  private static void SetupSwagger() {
    _builder.Services.AddEndpointsApiExplorer();
    _builder.Services.AddSwaggerGen(options => {
      options.SwaggerDoc("v1", new OpenApiInfo {
        Version = "v1",
        Title = "FAF cars API",
        Description = "API for TUM web lab 7",
        Contact = new OpenApiContact {
          Name = "Alexandru Dobrojan",
          Email = "alexandrudobrojan@gmail.com",
          Url = new Uri("https://github.com/Warek01/faf-cars-api.git")
        }
      });

      var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
      options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

      var securityDefinition = new OpenApiSecurityScheme {
        Name = "Bearer",
        BearerFormat = "JWT",
        Scheme = "bearer",
        Description = "Specify the authorization token.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http
      };
      options.AddSecurityDefinition("jwt_auth", securityDefinition);

      var securityScheme = new OpenApiSecurityScheme {
        Reference = new OpenApiReference {
          Id = "jwt_auth",
          Type = ReferenceType.SecurityScheme
        }
      };
      var securityRequirements = new OpenApiSecurityRequirement {
        [securityScheme] = []
      };
      options.AddSecurityRequirement(securityRequirements);
    });
  }

  private static void SetupDataSource() {
    var dataSourceBuilder = new NpgsqlDataSourceBuilder(_builder.Configuration.GetConnectionString("Default"));
    dataSourceBuilder.MapEnum<EngineType>();
    dataSourceBuilder.MapEnum<BodyStyle>();
    dataSourceBuilder.MapEnum<CarColor>();
    dataSourceBuilder.MapEnum<UserRole>();
    dataSourceBuilder.EnableParameterLogging();

    NpgsqlDataSource dataSource = dataSourceBuilder.Build();
    _builder.Services.AddDbContext<FafCarsDbContext>(options => { options.UseNpgsql(dataSource); });
    _builder.Services.AddScoped<DbContext, FafCarsDbContext>();
  }
}
