using PersonnalWebsite.RESTAPI.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonnalWebsite.RESTAPI.Data.SQLServer
{
    [Table("Users")]
    public class UserSQLServer
    {
        [Column("UserID")]
        [Key]
        public Guid UserID { get; set; }
        [Column("FirstName")]
        public string FirstName { get; set; }
        [Column("LastName")]
        public string LastName { get; set; }
        [Column("Username")]
        public string Username { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("PasswordHash")]
        public byte[] PasswordHash { get; set; }
        [Column("PasswordSalt")]
        public byte[] PasswordSalt { get; set; }
        [Column("IsAdmin")]
        public bool IsAdmin { get; set; }
        [Column("Birthdate")]
        public DateTime Birthdate { get; set; }
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        [Column("LastModifiedAt")]
        public DateTime LastModifiedAt { get; set; }

        public UserSQLServer() { }

        public UserSQLServer(User user)
        {
            UserID = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
            Email = user.Email;
            IsAdmin = user.IsAdmin;
            PasswordHash = user.PasswordHash;
            PasswordSalt = user.PasswordSalt;   
            Birthdate = user.Birthdate;
            CreatedAt = user.CreatedAt;
            LastModifiedAt = user.LastModifiedAt;
        }

        public UserSQLServer(Guid id, string lastName, string firstName, string username, string email, bool isAdmin, byte[] passwordHash, byte[] passwordSalt, DateTime birthdate, DateTime createdAt, DateTime lastModifiedAt)
        {
            UserID = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Email = email;
            IsAdmin = isAdmin;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            Birthdate = birthdate;
            CreatedAt = createdAt;
            LastModifiedAt = lastModifiedAt;
        }

        public User ToEntity()
        {
            return new User(UserID, LastName, FirstName, Username, Email, IsAdmin, PasswordHash, PasswordSalt, Birthdate, CreatedAt, LastModifiedAt);
        }
    }
}
