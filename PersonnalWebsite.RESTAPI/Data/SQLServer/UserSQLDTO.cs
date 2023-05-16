using PersonnalWebsite.RESTAPI.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonnalWebsite.RESTAPI.Data.SQLServer
{
    [Table("Users")]
    public class UserSQLDTO
    {
        [Column("UserID")]
        [Key]
        public Guid UserID { get; set; }
        [Column("FirstName")]
        public string FirstName { get; set; }
        [Column("Name")]
        public string LastName { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("Age")]
        public int Age { get; set; }
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        [Column("LastModifiedAt")]
        public DateTime LastModifiedAt { get; set; }

        public UserSQLDTO() { }

        public UserSQLDTO(User user)
        {
            UserID = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Age = user.Age;
            CreatedAt = user.CreatedAt;
            LastModifiedAt = user.LastModifiedAt;
        }

        public UserSQLDTO(Guid id, string lastName, string firstName, string email, int age, DateTime createdAt, DateTime lastModifiedAt)
        {
            UserID = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Age = age;
            CreatedAt = createdAt;
            LastModifiedAt = lastModifiedAt;
        }

        public User ToEntity()
        {
            return new User(UserID, LastName, FirstName, Email, Age, CreatedAt, LastModifiedAt);
        }
    }
}
