using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Contacts.DAL;
using Contacts.Model;
using Contacts.Services.Repository;
using Contacts.Services.Settings;
using Contacts.View;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Contacts.ViewModel
{
    public class MainListViewModel: BindableBase, IInitialize
    {
        private ContactService _contactService;
        private ISettingsManager _settingsManager;
        private INavigationService _navigationService;

        public MainListViewModel(ContactService contactService, ISettingsManager settingsManager, INavigationService navigationService)
        {
            _contactService = contactService;
            _settingsManager = settingsManager;
            _navigationService = navigationService;
            Contacts = new ObservableCollection<ContactViewModel>();
        }
        
        
        #region --- Public Properties ---

        public ICommand LogOutButtonTapCommand => new Command(OnLogOutButtonTap);

        public ObservableCollection<ContactViewModel> _contacts;
        public ObservableCollection<ContactViewModel> Contacts
        {
            get => _contacts;
            set => SetProperty(ref _contacts, value);
        }
        
        private bool _isContactsEmpty;
        public bool IsContactsEmpty
        {
            get => _isContactsEmpty;
            set => SetProperty(ref _isContactsEmpty, value);
        }

        #endregion

        #region --- Public Methods ---

        public async void Initialize(INavigationParameters parameters)
        {
            int userId = (int) parameters["userId"];
            
            var _contactList = await _contactService.GetContactsByUserAsync(userId);
            Contacts = new ObservableCollection<ContactViewModel>(_contactList.Select(x => x.ToContactViewModel()));
            
            var deleteCommand = new Command(OnDeleteCommand);
            var editCommand = new Command(OnEditCommand);
            foreach (var contact in Contacts)
            {
                contact.DeleteCommand = deleteCommand;
                contact.EditCommand = editCommand;
            }
            
        }

        #endregion
        
        #region --- Overrides ---
        
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case nameof(Contacts):
                    IsContactsEmpty =true;
                    break;
            }
        }
        #endregion

        #region --- Private Helpers ---

        private void OnDeleteCommand()
        {
            
        }
        
        private void OnEditCommand()
        {
            
        }
        
        private async void OnLogOutButtonTap()
        {
            _settingsManager.UserId = -1;
            await _navigationService.NavigateAsync("/NavigationPage/" + nameof(SignIn));
        }

        #endregion
    }
}