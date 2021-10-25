using System;
using System.IO;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Essentials;

namespace Contacts.Services.Camera
{
    public class CameraService : ICameraService
    {
        public async Task<string> GetPhotoFromGallery()
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();
                var newFilePath = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);
                
                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFilePath))
                    await stream.CopyToAsync(newStream);
                return newFilePath;
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Error", "Ok");
            }
            return "";
        }
        
        public async Task<string> TakePhoto()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions 
                { 
                    Title = $"xamarin.{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.png"
                });
                var newFilePath = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);
                
                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFilePath))
                    await stream.CopyToAsync(newStream);
                return newFilePath;
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Error", "Ok");
            }

            return "";
        }
    }
}