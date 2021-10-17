using System.Collections.Generic;
using System.Threading.Tasks;
using Contacts.Model;
using Contacts.Services.Repository;

namespace Contacts.DAL
{
    public class UserDAO
    {
        private IRepository _repository;

        public UserDAO(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> InsertAsync(UserModel user)
        {
            return await _repository.InsertAsync(user);
        }

        public async Task<UserModel> FindByLoginAsync(string login)
        {
            return await _repository.FindWithQueryAsync<UserModel>("SELECT * FROM User WHERE Login = ?", login);
        }
        public async Task<UserModel> FindByIDAsync(int id)
        {
            return await _repository.FindWithQueryAsync<UserModel>("SELECT * FROM User WHERE Id = ?", id);
        }

        /*public async Task<int> DeleteAsync(UserModel user)
        {
            return await _repository.DeleteAsync(user);
        }

        public async Task<int> UpdateAsync(UserModel user)
        {
            return await _repository.UpdateAsync(user);
        }

        public async Task<List<UserModel>> GetAllAsync()
        {
            return await _repository.GetTableAsync<UserModel>().ToListAsync();
        }*/
    }
}