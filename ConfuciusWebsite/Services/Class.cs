using ConfuciusWebsite.Data;
using ConfuciusWebsite.Models;
using ConfuciusWebsite.ViewModels.EventViewModels;

namespace ConfuciusWebsite.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;

        public ImageService(IWebHostEnvironment env, ApplicationDbContext context)
        {
            _env = env;
            _context = context;
        }

        public List<ImageOption> GetImages(string itemType, int itemId)
        {
            return _context.Images
                .Where(i => i.ItemType == itemType && i.ItemId == itemId)
                .OrderBy(i => i.SortOrder)
                .Select(i => new ImageOption
                {
                    Id = i.Id,
                    FilePath = i.FilePath,
                    AltText_BG = i.AltText_BG,
                    AltText_EN = i.AltText_EN
                })
                .ToList();
        }

        public void SaveImages(string itemType, int itemId, List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return;

            string uploadPath = Path.Combine(_env.WebRootPath, "uploads", itemType.ToLower());

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            int sortOrder = _context.Images
                .Where(i => i.ItemType == itemType && i.ItemId == itemId)
                .Select(i => (int?)i.SortOrder)
                .Max() ?? 0;

            foreach (var file in files)
            {

                if (file == null) continue;

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (string.IsNullOrEmpty(extension)) continue;

                if (file.Length == 0)
                    continue;

                string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                string physicalPath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                string url = $"/uploads/{itemType.ToLower()}/{fileName}";

                _context.Images.Add(new Image
                {
                    ItemType = itemType,
                    ItemId = itemId,
                    FilePath = url,
                    SortOrder = ++sortOrder,
                    CreatedAt = DateTime.Now
                });
            }

            _context.SaveChanges();
        }

        public void DeleteImages(string itemType, int itemId)
        {
            var images = _context.Images
                .Where(i => i.ItemType == itemType && i.ItemId == itemId)
                .ToList();

            foreach (var img in images)
            {
                string relative = img.FilePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
                string physicalPath = Path.Combine(_env.WebRootPath, relative);

                if (File.Exists(physicalPath))
                    File.Delete(physicalPath);
            }

            _context.Images.RemoveRange(images);
            _context.SaveChanges();
        }

        public void DeleteImageById(int imageId)
        {
            var image = _context.Images.FirstOrDefault(i => i.Id == imageId);
            if (image == null)
                return;

            // Normalize path
            var relativePath = image.FilePath
                .TrimStart('/')
                .Replace('/', Path.DirectorySeparatorChar);

            var physicalPath = Path.Combine(_env.WebRootPath, relativePath);

            if (File.Exists(physicalPath))
                File.Delete(physicalPath);

            _context.Images.Remove(image);
            _context.SaveChanges();
        }
    }
}
