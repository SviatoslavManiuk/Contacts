using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using Acr.UserDialogs;
using Contacts.DAL;
using Contacts.Model;
using Contacts.Services.Extensions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Contacts.ViewModel
{
    public class AddEditProfileViewModel: BindableBase, IInitialize
    {
        private ContactService _contactService;
        private INavigationService _navigationService;
        private int _id;
        private int _userId;
        private DateTime _date;
        

        public AddEditProfileViewModel(ContactService contactService, INavigationService navigationService)
        {
            _contactService = contactService;
            _navigationService = navigationService;
        }

        #region --- Public Properties ---

        public ICommand SaveIconTapCommand => new DelegateCommand(OnSaveIconTap).ObservesCanExecute(() => CanSave);
        public ICommand ProfileImageTapCommand => new Command(OnProfileImageTap);

        private string _nickName;
        public string NickName
        {
            get => _nickName;
            set => SetProperty(ref _nickName, value);
        }
        
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        
        private string _profileImageSource;
        public string ProfileImageSource
        {
            get => _profileImageSource;
            set => SetProperty(ref _profileImageSource, value);
        }
        
        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private bool _canSave;

        public bool CanSave
        {
            get => _canSave;
            set => SetProperty(ref _canSave, value);
        }
        
        #endregion
        
        #region --- Public Methods ---
        
        public void Initialize(INavigationParameters parameters)
        {
            var parameter = parameters[nameof(ContactModel)];
            if (parameter is ContactModel)
            {
                ContactModel contact = (ContactModel) parameter;
                
                if (contact.Id == -1)
                {
                    ProfileImageSource = Constants.PROFILE_ICON_SOURCE;
                    _date = DateTime.Now;
                }
                else
                {
                    _id = contact.Id;
                    NickName = contact.NickName;
                    Name = contact.Name;
                    Description = contact.Description;
                    ProfileImageSource = contact.ImageSource;
                    _date = contact.Date;

                }
                
                _userId = contact.UserId;
            }
            else
            {
                throw new Exception();
            }
        }
        
        #endregion

        #region --- Overrides ---

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case nameof(NickName):
                case nameof(Name):
                    CanSave = !(string.IsNullOrWhiteSpace(NickName) || string.IsNullOrWhiteSpace(Name));
                    break;
            }
        }

        #endregion
        #region --- Private Helpers ---

        private async void OnSaveIconTap()
        {
            ContactModel contact = new ContactModel()
            {
                UserId = _userId,
                Name = this.Name,
                NickName = this.NickName,
                ImageSource = this.ProfileImageSource,
                Description = this.Description,
                Date = _date
            };

            var parameters = new NavigationParameters();
            
            if (_id == 0)
            {
                parameters.Add("newContact", contact.ToContactViewModel());
                await _contactService.InsertAsync(contact);
            }
            else
            {
                contact.Id = _id;
                parameters.Add("editedContact", contact.ToContactViewModel());
                int res = await _contactService.UpdateAsync(contact);
            }
            
            
            await _navigationService.GoBackAsync(parameters);
        }

        private void OnProfileImageTap()
        {
            var cfg = new ActionSheetConfig()
                .SetCancel()
                .Add("Choose from gallery", GetPhotoFromGallery, Constants.GALLERY_ICON_SOURCE)
                .Add("Take a photo", TakePhoto, Constants.CAMERA_ICON_SOURCE);
            UserDialogs.Instance.ActionSheet(cfg);
        }

        private async void GetPhotoFromGallery()
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();
                var newFilePath = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);
                
                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFilePath))
                    await stream.CopyToAsync(newStream);
                ProfileImageSource = newFilePath;
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Error", "Ok");
            }
        }
        
        private async void TakePhoto()
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
                ProfileImageSource = newFilePath;
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Error", "Ok");
            }
        }
        
        

        #endregion
    }
}