using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.IO;
using System.Threading.Tasks;

namespace RealEstate.Utilities
{
    public static class Helper
    {
        public static async Task<string> SaveImageAsync(IFormFile file, string folderName, int width, int height)
        {
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);
            var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine($"wwwroot/Uploads/{folderName}", uniqueFileName);

            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (var inputStream = file.OpenReadStream())
            using (var outputStream = new FileStream(filePath, FileMode.Create))
            {
                // Load the image, resize it, and save it
                using (var image = Image.Load(inputStream))
                {
                    image.Mutate(x => x.Resize(width, height)); // Resize the image
                    await image.SaveAsync(outputStream, new JpegEncoder()); // Save as JPEG
                }
            }

            // Return the relative URL to store in the database
            return $"/Uploads/{folderName}/{uniqueFileName}";
        }

        public static async Task<string> SaveingImageAsync(IFormFile file, string folderName)
        {
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);
            var filePath = Path.Combine($"wwwroot/Uploads/{folderName}", $"{fileName}_{Guid.NewGuid()}{extension}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return the URL to store in the database
            return $"/Uploads/{folderName}/{Path.GetFileName(filePath)}";
        }
    }
}
