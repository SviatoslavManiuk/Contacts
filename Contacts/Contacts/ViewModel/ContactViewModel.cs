using System;
using System.Windows.Input;
using Contacts.Model;
using Prism.Mvvm;

namespace Contacts.ViewModel
{
    public class ContactViewModel: BindableBase
    {
        public ContactViewModel(int id, string name, string nickName, string description, string profileImageSource,
            DateTime date)
        {
            Id = id;
            Name = name;
            NickName = nickName;
            Description = description;
            ProfileImageSource = profileImageSource;
            Date = date;
        }
        #region --- Public Properties ---

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get => _deleteCommand;
            set => SetProperty(ref _deleteCommand, value);
        }
        
        private ICommand _editCommand;
        public ICommand EditCommand
        {
            get => _editCommand;
            set => SetProperty(ref _editCommand, value);
        }
        
        public int Id { get; }

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
        
        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }
        
        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        #endregion
    }
    
    public static class ContactExtension
    {
        public static ContactViewModel ToContactViewModel(this ContactModel contact)
        {
            return new ContactViewModel(contact.Id, contact.Name, contact.NickName,
                    contact.Description, contact.ImageSource, contact.Date);
        }

        public static ContactModel ToContactModel(this ContactViewModel contactViewModel) => new()
        {
            Id = contactViewModel.Id,
            Name = contactViewModel.Name,
            NickName = contactViewModel.NickName,
            ImageSource = contactViewModel.ProfileImageSource,
            Date = contactViewModel.Date,
            Description = contactViewModel.Description
        };
    }
}