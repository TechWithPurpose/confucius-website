using ConfuciusWebsite.ViewModels.EventViewModels;

namespace ConfuciusWebsite.Services
{
    public interface IImageService
    {
        List<ImageOption> GetImages(string itemType, int itemId);
        void SaveImages(string itemType, int itemId, List<IFormFile> files);
        void DeleteImages(string itemType, int itemId);
        void DeleteImageById(int imageId);
    }
}
