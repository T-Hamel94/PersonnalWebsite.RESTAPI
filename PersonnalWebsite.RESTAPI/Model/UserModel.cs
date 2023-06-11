using PersonnalWebsite.RESTAPI.Entities;

namespace PersonnalWebsite.RESTAPI.Model
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }

        public UserModel() { }

        public UserModel(Guid id, string lastName, string firstName, string username, string email, DateTime birthdate, bool isAdmin, DateTime createdAt, DateTime lastModifiedAt)
        { 
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Email = email;
            Birthdate = birthdate;
            IsAdmin = isAdmin;
            CreatedAt = createdAt;
            LastModifiedAt = lastModifiedAt;
        }

        public User ToEntity()
        {
            return new User(Id, LastName, FirstName, Username, Email, IsAdmin, Birthdate, CreatedAt, LastModifiedAt, new byte[0], new byte[0]);   
        }
    }
}