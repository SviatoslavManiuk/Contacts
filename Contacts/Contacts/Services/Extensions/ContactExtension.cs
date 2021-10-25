using Contacts.Model;
using Contacts.ViewModel;

namespace Contacts.Services.Extensions
{
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