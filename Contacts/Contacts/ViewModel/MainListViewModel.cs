using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Contacts.DAL;
using Contacts.Model;
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

        public ICommand LogOutIconTapCommand => new Command(OnLogOutIconTap);
        
        public ICommand AddButtonTapCommand => new Command(OnAddButtonTap);

        private ObservableCollection<ContactViewModel> _contacts;
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
        
        public int UserId { get; private set; }

        #endregion

        #region --- Public Methods ---

        public async void Initialize(INavigationParameters parameters)
        {
            UserId = (int) parameters["userId"];
            
            var contactList = await _contactService.GetContactsByUserAsync(UserId);
            Contacts = new ObservableCollection<ContactViewModel>(contactList.Select(x => x.ToContactViewModel()));
            
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
                    IsContactsEmpty = (Contacts.Count == 0);
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
        
        private async void OnLogOutIconTap()
        {
            _settingsManager.UserId = -1;
            await _navigationService.NavigateAsync("/NavigationPage/" + nameof(SignIn));
        }

        private async void OnAddButtonTap()
        {
            var parameters = new NavigationParameters();
            parameters.Add(nameof(ContactModel), new ContactModel(){Id = -1, UserId = this.UserId});
            await _navigationService.NavigateAsync(nameof(AddEditProfile), parameters);
        }
        

        #endregion
    }
}