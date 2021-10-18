using System;
using System.IO;
using System.Threading.Tasks;
using Contacts.Model;
using SQLite;

namespace Contacts.Services.Repository
{
    public class Repository : IRepository
    {
        private Lazy<SQLiteAsyncConnection> _database;

        public Repository()
        {
            _database = new Lazy<SQLiteAsyncConnection>(()=>
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "contacts.db3");
                var database = new SQLiteAsyncConnection(path);

                database.CreateTableAsync<UserModel>().Wait();
                database.CreateTableAsync<ContactModel>().Wait();
                return database;
            });
        }

        public async Task<int> InsertAsync<T>(T entity) where T : IEntityBase, new()
        {
            return await _database.Value.InsertAsync(entity);
        }

        public async Task<int> UpdateAsync<T>(T entity) where T : IEntityBase, new()
        {
            return await _database.Value.UpdateAsync(entity);
        }

        public async Task<int> DeleteAsync<T>(T entity) where T : IEntityBase, new()
        {
            return await _database.Value.DeleteAsync(entity);
        }

        public AsyncTableQuery<T> GetTable<T>() where T : IEntityBase, new()
        {
            return _database.Value.Table<T>();
        }
        
        public async Task<T> FindWithQueryAsync<T>(string query, params object[] args) where T : IEntityBase, new()
        {
            return await _database.Value.FindWithQueryAsync<T>(query, args);
        }
    }
}