using System.Threading.Tasks;

namespace Contacts.Services.Camera
{
    public interface ICameraService
    {
        Task<string> GetPhotoFromGallery();
        
        Task<string> TakePhoto();
    }
}