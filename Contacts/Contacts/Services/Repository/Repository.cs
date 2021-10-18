using System;
using System.Collections.Generic;
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
            _database = new Lazy<SQLiteAsyncConnection>(() =>
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "contacts.db3");
                
                var database = new SQLiteAsyncConnection(path);
                database.CreateTableAsync<UserModel>();
                database.CreateTableAsync<ContactModel>();

                return database;
            });
        }

        public Task<int> InsertAsync<T>(T entity) where T : IEntityBase, new()
        {
            return _database.Value.InsertAsync(entity);
        }

        public Task<int> UpdateAsync<T>(T entity) where T : IEntityBase, new()
        {
            return _database.Value.UpdateAsync(entity);
        }

        public Task<int> DeleteAsync<T>(T entity) where T : IEntityBase, new()
        {
            return _database.Value.DeleteAsync(entity);
        }

        public Task<List<T>> GetAllAsync<T>() where T : IEntityBase, new()
        {
            return _database.Value.Table<T>().ToListAsync();
        }
        
        public Task<T> FindWithQueryAsync<T>(string query, params object[] args) where T : IEntityBase, new()
        {
            return _database.Value.FindWithQueryAsync<T>(query, args);
        }
    }
}