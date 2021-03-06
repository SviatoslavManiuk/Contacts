using System.Collections.Generic;
using System.Threading.Tasks;
using Contacts.Model;
using Contacts.Services.Repository;

namespace Contacts.Services.DAL
{
    public class ContactService
    {
        private IRepository _repository;

        public ContactService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> InsertAsync(ContactModel contact)
        {
            return await _repository.InsertAsync(contact);
        }

        public async Task<int> DeleteAsync(ContactModel contact)
        {
            return await _repository.DeleteAsync(contact);
        }

        public async Task<int> UpdateAsync(ContactModel contact)
        {
            return await _repository.UpdateAsync(contact);
        }

        public async Task<List<ContactModel>> GetContactsByUserAsync(int userId)
        {
            return await _repository.GetTable<ContactModel>().Where(contact => contact.UserId == userId).ToListAsync();
        }
    }
}