using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Api.Services;

public class ImageService(StaticFileService staticFileService) {
  public const string DefaultImageExtension = "webp";

  public async Task CreateImage(IFormFile file, PathString path) {
    await using Stream stream = file.OpenReadStream();
    using Image image = await Image.LoadAsync(stream);

    image.Mutate(ctx => { ctx.Crop(Math.Min(1920, image.Width), Math.Min(1080, image.Height)); });

    var encoder = new WebpEncoder {
      Quality = 80,
      Method = WebpEncodingMethod.Level4,
      FileFormat = WebpFileFormatType.Lossy,
    };

    await image.SaveAsWebpAsync(
      Path.Combine(staticFileService.RootPath, path),
      encoder
    );
  }
}
