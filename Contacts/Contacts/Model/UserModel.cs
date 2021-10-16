using SQLite;

namespace Contacts.Model
{
    public class UserModel: IEntityBase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public string Login { get; set; }
        
        public string Password { get; set; }
    }
}