using System.Text;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;

namespace FafCarsApi.Services;

public class StaticFileService {
  public static readonly string Root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
  public static readonly PathString RequestPath = new PathString("/api/v1/file");

  public StaticFileService() {
    if (!Directory.Exists(Root))
      Directory.CreateDirectory(Root);
  }

  public static void SetupStaticFileServing(WebApplication app) {
    app.UseFileServer();
    app.UseStaticFiles(new StaticFileOptions {
      RequestPath = RequestPath,
      HttpsCompression = HttpsCompressionMode.Compress,
      ServeUnknownFileTypes = false,
      FileProvider = new PhysicalFileProvider(Root)
    });
  }

  public static async Task Create(string fileName, string body) {
    await Create(fileName, Encoding.UTF8.GetBytes(body));
  }

  public static async Task Create(string fileName, byte[] bytes) {
    await File.WriteAllBytesAsync(
      Path.Combine(Root, fileName.ToLower()),
      bytes
    );
  }
}
