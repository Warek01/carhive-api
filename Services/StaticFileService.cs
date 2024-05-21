using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;

namespace FafCarsApi.Services;

public class StaticFileService
{
  private static readonly string _root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

  public StaticFileService()
  {
    if (!Directory.Exists(_root))
      Directory.CreateDirectory(_root);
  }

  public static void SetupStaticFileServing(WebApplication app)
  {
    app.UseFileServer();
    app.UseStaticFiles(new StaticFileOptions
    {
      RequestPath = "/api/v1/file",
      HttpsCompression = HttpsCompressionMode.Compress,
      ServeUnknownFileTypes = false,
      FileProvider = new PhysicalFileProvider(_root)
    });
  }

  public void Create(string filename, byte[] bytes)
  {
    File.WriteAllBytes(
      Path.Combine(_root, filename),
      bytes
    );
  }
}