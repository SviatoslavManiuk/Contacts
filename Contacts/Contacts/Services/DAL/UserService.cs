using System.Threading.Tasks;
using Contacts.Model;
using Contacts.Services.Repository;

namespace Contacts.Services.DAL
{
    public class UserService
    {
        private IRepository _repository;

        public UserService(IRepository repository)
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

    }
}