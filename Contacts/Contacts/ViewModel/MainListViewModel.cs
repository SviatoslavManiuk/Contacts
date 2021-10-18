using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Contacts.DAL;
using Contacts.Model;
using Contacts.Services.Repository;
using Prism.Mvvm;
using Prism.Navigation;

namespace Contacts.ViewModel
{
    public class MainListViewModel: BindableBase, IInitializeAsync
    {
        private ContactDAO _contactDao;

        public MainListViewModel(ContactDAO contactDao)
        {
            _contactDao = contactDao;
            Contacts = new ObservableCollection<ContactViewModel>();
        }
        
        
        #region --- Public Properties ---

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

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            int userId = (int) parameters["userId"];

            var _contactList = await _contactDao.GetContactsByUserAsync();
            Contacts = new ObservableCollection<ContactViewModel>(_contactList.Select(x=>x.ToContactViewModel()));
            
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
    }
}