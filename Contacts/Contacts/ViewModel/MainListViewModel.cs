using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Contacts.Model;
using Contacts.Services.DAL;
using Contacts.Services.Extensions;
using Contacts.Services.Settings;
using Contacts.View;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Contacts.ViewModel
{
    public class MainListViewModel: BindableBase, IInitialize, INavigationAware
    {
        private ContactService _contactService;
        private ISettingsManager _settingsManager;
        private INavigationService _navigationService;

        private List<Comparison<ContactViewModel>> _comparisons;

        public MainListViewModel(ContactService contactService, ISettingsManager settingsManager, INavigationService navigationService)
        {
            _contactService = contactService;
            _settingsManager = settingsManager;
            _navigationService = navigationService;
            Contacts = new ObservableCollection<ContactViewModel>();
            _comparisons = new List<Comparison<ContactViewModel>>()
            {
                NickNameComparer, NameComparer, DateComparer
            };
        }
        
        
        #region --- Public Properties ---

        public ICommand LogOutIconTapCommand => new Command(OnLogOutIconTap);
        
        public ICommand SettingsIconTapCommand => new Command(OnSettingsIconTap);
        
        public ICommand AddButtonTapCommand => new Command(OnAddButtonTap);
        
        public ICommand EditProfileItemTapCommand => new Command(OnEditProfileItemTap);
        
        public ICommand DeleteProfileItemTapCommand => new Command(OnDeleteProfileItemTap);

        private ObservableCollection<ContactViewModel> _contacts;
        public ObservableCollection<ContactViewModel> Contacts
        {
            get => _contacts;
            set => SetProperty(ref _contacts, value);
        }
        
        private ContactViewModel _selectedItem;
        public ContactViewModel SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }
        
        private bool _isContactsEmpty;

        public bool IsContactsEmpty
        {
            get => _isContactsEmpty;
            set => SetProperty(ref _isContactsEmpty, value);
        }
        
        private int _selectedSort;
        public int SelectedSort
        {
            get => _selectedSort;
            set => SetProperty(ref _selectedSort, value);
        }

        public int UserId { get; private set; }

        #endregion

        #region --- Public Methods ---

        public async void Initialize(INavigationParameters parameters)
        {
            UserId = (int) parameters["userId"];
            SelectedSort = _settingsManager.SelectedSort;
            
            var contactList = await _contactService.GetContactsByUserAsync(UserId);
            Contacts = new ObservableCollection<ContactViewModel>(contactList.Select(x => x.ToContactViewModel()));
            
            var deleteCommand = new Command(OnDeleteProfileItemTap);
            var editCommand = new Command(OnEditProfileItemTap);
            foreach (var contact in Contacts)
            {
                contact.DeleteCommand = deleteCommand;
                contact.EditCommand = editCommand;
            }
            Contacts.Sort(_comparisons[SelectedSort]);
        }
        
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters["newContact"] is ContactViewModel newContact)
            {
                newContact.DeleteCommand = new Command(OnDeleteProfileItemTap);
                newContact.EditCommand = new Command(OnEditProfileItemTap);
                int index = Contacts.BinarySearh(newContact, _comparisons[SelectedSort]);
                Contacts.Insert(index, newContact);
                RaisePropertyChanged(nameof(Contacts));
            }

            if (parameters["editedContact"] is ContactViewModel editedContact)
            {
                editedContact.DeleteCommand = new Command(OnDeleteProfileItemTap);
                editedContact.EditCommand = new Command(OnEditProfileItemTap);
                var oldContact = Contacts.First(contact => contact.Id == editedContact.Id);
                Contacts.Remove(oldContact);
                int index = Contacts.BinarySearh(editedContact, _comparisons[SelectedSort]);
                Contacts.Insert(index, editedContact);
            }

            var selectedSortParameter = parameters["SelectedSort"];
            if (selectedSortParameter != null)
            {
                SelectedSort = (int) parameters["SelectedSort"];
                
                Contacts.Sort(_comparisons[SelectedSort]);
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
        
        private async void OnDeleteProfileItemTap()
        {
            if (SelectedItem != null)
            {
                bool accept = await UserDialogs.Instance.ConfirmAsync("Are you sure you want to delete the profile?",
                    null, "Delete");
                if (accept)
                {
                    await _contactService.DeleteAsync(SelectedItem.ToContactModel());
                    Contacts.Remove(SelectedItem);
                    RaisePropertyChanged(nameof(Contacts));
                }

                SelectedItem = null;
            }
        }
        
        private async void OnEditProfileItemTap()
        {
            if (SelectedItem == null) return;
            
            var parameters = new NavigationParameters();
            ContactModel contact = SelectedItem.ToContactModel();
            contact.UserId = UserId;
            parameters.Add(nameof(ContactModel), contact);
            SelectedItem = null;
            await _navigationService.NavigateAsync(nameof(AddEditProfile), parameters);
        }
        
        private async void OnLogOutIconTap()
        {
            _settingsManager.UserId = -1;
            await _navigationService.NavigateAsync("/NavigationPage/" + nameof(SignIn));
        }
        
        private async void OnSettingsIconTap()
        {
            await _navigationService.NavigateAsync(nameof(Settings));
        }

        private async void OnAddButtonTap()
        {
            var parameters = new NavigationParameters();
            parameters.Add(nameof(ContactModel), new ContactModel(){Id = -1, UserId = this.UserId});
            await _navigationService.NavigateAsync(nameof(AddEditProfile), parameters);
        }

        private int NameComparer(ContactViewModel contact1, ContactViewModel contact2)
        {
            return contact1.Name.CompareTo(contact2.Name);
        }
        
        private int NickNameComparer(ContactViewModel contact1, ContactViewModel contact2)
        {
            return contact1.NickName.CompareTo(contact2.NickName);
        }
        
        private int DateComparer(ContactViewModel contact1, ContactViewModel contact2)
        {
            return contact1.Date.CompareTo(contact2.Date);
        }

        #endregion
    }
}