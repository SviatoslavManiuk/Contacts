using System;
using SQLite;

namespace Contacts.Model
{
    [Table("Contact")]
    public class ContactModel : IEntityBase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public string NickName { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }
        
        public string ImageSource { get; set; }
        
        public DateTime Date { get; set; }
    }
}