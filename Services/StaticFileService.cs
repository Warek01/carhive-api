using System.Text;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;

namespace FafCarsApi.Services;

public class StaticFileService {
  private static readonly string _root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

  public StaticFileService() {
    if (!Directory.Exists(_root))
      Directory.CreateDirectory(_root);
  }

  public static void SetupStaticFileServing(WebApplication app) {
    app.UseFileServer();
    app.UseStaticFiles(new StaticFileOptions {
      RequestPath = "/api/v1/file",
      HttpsCompression = HttpsCompressionMode.Compress,
      ServeUnknownFileTypes = false,
      FileProvider = new PhysicalFileProvider(_root)
    });
  }

  public static async Task Create(string fileName, string body) {
    await Create(fileName, Encoding.UTF8.GetBytes(body));
  }

  public static async Task Create(string fileName, byte[] bytes) {
    await File.WriteAllBytesAsync(
      Path.Combine(_root, fileName),
      bytes
    );
  }
}