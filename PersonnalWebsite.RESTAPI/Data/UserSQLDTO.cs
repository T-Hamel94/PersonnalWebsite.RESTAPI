using Srv_PersonnalWebsite.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonnalWebsite.RESTAPI.Data
{
    [Table("Users")]
    public class UserSQLDTO
    {
        [Column("UserID")]
        [Key]
        public Guid UserID { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("FirstName")]
        public string FirstName { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("Age")]
        public int Age { get; set; }
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        [Column("LastModifiedAt")]
        public DateTime LastModifiedAt { get; set; }

        public UserSQLDTO() { }

        public UserSQLDTO(Guid id, string name, string firstName, string email, int age, DateTime createdAt, DateTime lastModifiedAt)
        {
            UserID = id;
            Name = name;
            FirstName = firstName;
            Email = email;
            Age = age;
            CreatedAt = createdAt;
            LastModifiedAt = lastModifiedAt;
        }

        public User ToEntity()
        {
            return new User(UserID, Name, FirstName, Email, Age, CreatedAt, LastModifiedAt);
        }
    }
}
