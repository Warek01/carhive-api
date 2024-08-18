using System.Globalization;
using System.Reflection;
using Api.Config;
using Api.Data;
using Api.Enums;
using Api.Helpers;
using Api.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Npgsql;
using Serilog;
using StackExchange.Redis;

namespace Api;

public static class Program {
  private static WebApplicationBuilder _builder = null!;

  public static void Main(string[] args) {
    CultureInfo.DefaultThreadCurrentCulture =
      CultureInfo.DefaultThreadCurrentUICulture =
        Thread.CurrentThread.CurrentCulture =
          Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

    _builder = WebApplication.CreateBuilder(args);

    _builder.WebHost.UseKestrel();
    _builder.Host.UseSerilog(
      (context, c) => {
        c.ReadFrom.Configuration(context.Configuration);
        c.MinimumLevel.Information();
        c.WriteTo.Console();
      }
    );
    _builder.Services.AddControllers();

    _builder.Services
      .Configure<KestrelServerOptions>(o => {
        o.Limits.MaxRequestBodySize = 50_000_000;
        o.Limits.MaxRequestLineSize = 8192;
      })
      .AddAutoMapper(typeof(MappingProfile))
      .AddSingleton<IConfiguration>(_builder.Configuration)
      .AddHttpClient()
      .AddProblemDetails()
      .AddExceptionHandler<HttpExceptionHandler>()
      .AddApiVersioning(options => {
        options.DefaultApiVersion = new ApiVersion(1);
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
      }).AddApiExplorer(options => {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
      });

    var fileProvider = new PhysicalFileProvider(_builder.Environment.WebRootPath);

    if (_builder.Environment.IsDevelopment()) {
      _builder.Services.AddDirectoryBrowser();
      SetupSwagger();
    }

    SetupDataSource();
    SetupAuthentication();
    SetupCache();

    AppServices.Register(_builder);

    var app = _builder.Build();

    app
      .UseSerilogRequestLogging()
      .UseSwagger()
      .UseSwaggerUI()
      .UseHttpsRedirection()
      .UseExceptionHandler()
      .UseFileServer()
      .UseStaticFiles(new StaticFileOptions {
        RequestPath = StaticFileService.RequestPath,
        HttpsCompression = HttpsCompressionMode.Compress,
        ServeUnknownFileTypes = false,
        RedirectToAppendTrailingSlash = false,
        FileProvider = fileProvider,
      });

    AuthService.SetupAuthorization(app);
    app.MapControllerRoute("Default", "{controller}/{action}/{id?}");

    if (app.Environment.IsDevelopment()) {
      app
        .UseCors(options => {
          options.AllowAnyOrigin();
          options.AllowAnyHeader();
          options.AllowAnyMethod();
        })
        .UseDirectoryBrowser(new DirectoryBrowserOptions {
          FileProvider = fileProvider,
          RequestPath = "/DirectoryBrowser",
          RedirectToAppendTrailingSlash = false,
        })
        .UseDefaultFiles()
        .UseStaticFiles(new StaticFileOptions {
          RequestPath = "/DirectoryBrowser",
          HttpsCompression = HttpsCompressionMode.Compress,
          ServeUnknownFileTypes = true,
          RedirectToAppendTrailingSlash = false,
          FileProvider = fileProvider,
        });

      app.MapGet("/", ctx => {
        ctx.Response.Redirect("swagger/index.html");
        return Task.CompletedTask;
      });
    }

    app.Run();
  }

  private static void SetupAuthentication() {
    var jwtConfig = new JwtConfig();
    _builder.Configuration.Bind("Jwt", jwtConfig);

    _builder.Services
      .AddSingleton(jwtConfig)
      .AddAuthentication(options => {
        options.DefaultScheme =
          options.DefaultChallengeScheme =
            options.DefaultAuthenticateScheme =
              options.DefaultSignInScheme =
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(options => {
        options.TokenValidationParameters = AuthService.CreateTokenValidationParameters(jwtConfig);
        options.MapInboundClaims = false;
      });

    _builder.Services.AddAuthorization();
  }

  private static void SetupSwagger() {
    _builder.Services
      .AddEndpointsApiExplorer()
      .AddSwaggerGen(options => {
        options.SwaggerDoc("v1", new OpenApiInfo {
          Version = "v1",
          Title = "CarHive API"
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
        options.OperationFilter<CustomSwaggerOperationFilter>();
      });
  }

  private static void SetupDataSource() {
    var dataSourceBuilder = new NpgsqlDataSourceBuilder(_builder.Configuration.GetConnectionString("Default"));

    dataSourceBuilder
      .MapEnum<CarFuelType>()
      .MapEnum<CarBodyStyle>()
      .MapEnum<CarColor>()
      .MapEnum<UserRole>()
      .MapEnum<CarStatus>()
      .MapEnum<CarDrivetrain>()
      .MapEnum<CarTransmission>()
      .MapEnum<ListingStatus>()
      .MapEnum<ListingAction>()
      .MapEnum<OauthIdentityProvider>()
      .MapEnum<ReportType>();

    dataSourceBuilder.EnableDynamicJson();
    dataSourceBuilder.EnableParameterLogging();

    NpgsqlDataSource dataSource = dataSourceBuilder.Build();
    _builder.Services
      .AddDbContext<CarHiveDbContext>(options => { options.UseNpgsql(dataSource); })
      .AddScoped<DbContext, CarHiveDbContext>();
  }

  private static void SetupCache() {
    ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(new ConfigurationOptions {
      EndPoints = { _builder.Configuration.GetConnectionString("Redis")! },
      Protocol = RedisProtocol.Resp3,
      AbortOnConnectFail = true,
      AllowAdmin = false,
    });

    _builder.Services.AddSingleton(muxer);
  }
}
