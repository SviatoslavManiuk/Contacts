using System;
using Contacts.Model;
using Prism.Mvvm;

namespace Contacts.ViewModel
{
    public class ContactViewModel: BindableBase
    {
        #region --- Public Properties ---
        
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
        
        private string _imageSource;
        public string ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
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
        /*public static ContactModel ToContactModel(this ContactViewModel contact) => new()
        {
            Name = contact.Name,
            NickName = contact.NickName,
        };*/

        public static ContactViewModel ToContactViewModel(this ContactModel contact) => new()
        {
            Name = contact.Name,
            NickName = contact.NickName,
            ImageSource = contact.ImageSource,
            Date = contact.Date,
            Description = contact.Description
        };
    }
}