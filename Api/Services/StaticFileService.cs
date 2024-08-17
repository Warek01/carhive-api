using System.Text;

namespace Api.Services;

public class StaticFileService {
  public readonly string RootPath;

  // The relative path from the base url to the files folder
  public const string RequestPath = "/Api/File";

  public const string UploadsPath = "Uploads";

  public StaticFileService(IHostEnvironment hostEnvironment) {
    RootPath = hostEnvironment.ContentRootPath;
    
    if (!Directory.Exists(RootPath)) {
      Directory.CreateDirectory(RootPath);
    }
  }

  public async Task CreateFile(string body, params string[] paths) {
    await CreateFile(Encoding.UTF8.GetBytes(body), paths);
  }

  public Task CreateFile(byte[] bytes, params string[] paths) {
    return File.WriteAllBytesAsync(
      Path.Combine(RootPath, Path.Combine(paths)),
      bytes
    );
  }
}
