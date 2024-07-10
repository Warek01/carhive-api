using FafCarsApi.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace FafCarsApi.Helpers;

public static class ImageHelper {
  public static async Task Create(string fileName, IFormFile file) {
    await using Stream stream = file.OpenReadStream();
    using Image image = await Image.LoadAsync(stream);

    image.Mutate(ctx => { ctx.Crop(Math.Min(1920, image.Width), Math.Min(1080, image.Height)); });

    var encoder = new WebpEncoder {
      Quality = 80,
      Method = WebpEncodingMethod.Level4,
      FileFormat = WebpFileFormatType.Lossy
    };

    await image.SaveAsWebpAsync(
      Path.Combine(StaticFileService.Root, fileName),
      encoder
    );
  }
}
