using System.Text.RegularExpressions;
using FafCarsApi.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace FafCarsApi.Helpers;

public static partial class ImageHelper {
  [GeneratedRegex(@"^(data:image\/[a-zA-Z]+;base64,|base64,)", RegexOptions.IgnoreCase)]
  private static partial Regex Base64ImagePrefixRegex();

  // Create, convert and save an image to static files folder
  public static async Task Create(string fileName, string rawBody) {
    var regex = Base64ImagePrefixRegex();

    string replaced = regex.Replace(
      rawBody,
      string.Empty
    );
    byte[] buffer = Convert.FromBase64String(replaced);
    using var stream = new MemoryStream(buffer);

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