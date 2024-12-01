namespace RealEstate.Utilities
{
    public static class Helper
    {
        public static async Task<string> SaveImageAsync(IFormFile file, string folderName)
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
