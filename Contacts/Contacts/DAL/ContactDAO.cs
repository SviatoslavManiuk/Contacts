using System.Collections.Generic;
using System.Threading.Tasks;
using Contacts.Model;
using Contacts.Services.Repository;

namespace Contacts.DAL
{
    public class ContactDAO
    {
        private IRepository _repository;

        public ContactDAO(IRepository repository)
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

        public async Task<List<ContactModel>> GetContactsByUserAsync(UserModel user)
        {
            return await _repository.GetTableAsync<ContactModel>().Where(contact =>
                    contact.UserId == user.Id).ToListAsync();
        }
        
        /*public async Task<List<ContactModel>> GetAllAsync()
        {
            return await _repository.GetTableAsync<ContactModel>().ToListAsync();
        }*/
    }
}