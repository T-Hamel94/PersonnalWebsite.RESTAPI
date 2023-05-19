using PersonnalWebsite.RESTAPI.Model;

namespace PersonnalWebsite.RESTAPI.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }

        public User() { }

        public User(Guid id, string lastName, string firstName, string username, string email, bool isAdmin, byte[] passwordHash, byte[] passwordSalt, DateTime birthDate)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Email = email;
            IsAdmin = isAdmin;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            Birthdate = birthDate;
            CreatedAt = DateTime.UtcNow;
            LastModifiedAt = DateTime.UtcNow;
        }

        public User(Guid id, string lastName, string firstName, string username, string email, bool isAdmin, DateTime birthdate)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Email = email;
            IsAdmin = isAdmin;
            Birthdate = birthdate;
            CreatedAt = DateTime.UtcNow;
            LastModifiedAt = DateTime.UtcNow;

            PasswordSalt = new byte[0];
            PasswordHash = new byte[0];
        }


        public UserModel ToModel()
        {
            return new UserModel(Id, LastName, FirstName, Username, Email, Birthdate, IsAdmin, CreatedAt, LastModifiedAt);
        }
    }
}
