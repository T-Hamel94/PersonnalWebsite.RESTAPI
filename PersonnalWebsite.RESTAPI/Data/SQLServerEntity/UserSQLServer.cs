using PersonnalWebsite.RESTAPI.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonnalWebsite.RESTAPI.Data.SQLServerEntity
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
            Birthdate = user.Birthdate;
            CreatedAt = user.CreatedAt;
            LastModifiedAt = user.LastModifiedAt;
            PasswordHash = user.PasswordHash;
            PasswordSalt = user.PasswordSalt;
        }

        public UserSQLServer(Guid id, string lastName, string firstName, string username, string email, bool isAdmin, DateTime birthdate, DateTime createdAt, DateTime lastModifiedAt, byte[] passwordHash, byte[] passwordSalt)
        {
            UserID = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Email = email;
            IsAdmin = isAdmin;
            Birthdate = birthdate;
            CreatedAt = createdAt;
            LastModifiedAt = lastModifiedAt;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        public User ToEntity()
        {
            return new User(UserID, LastName, FirstName, Username, Email, IsAdmin, Birthdate, CreatedAt, LastModifiedAt, PasswordHash, PasswordSalt);
        }
    }
}
