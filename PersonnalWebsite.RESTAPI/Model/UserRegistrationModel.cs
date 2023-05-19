using PersonnalWebsite.RESTAPI.Entities;

namespace PersonnalWebsite.RESTAPI.Model
{
    public class UserRegistrationModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }

        public UserRegistrationModel() { }

        public UserRegistrationModel(Guid id, string lastName, string firstName, string email, DateTime birthdate, DateTime createdAt, DateTime lastModifiedAt)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Birthdate = birthdate;
            CreatedAt = createdAt;
            LastModifiedAt = lastModifiedAt;
        }
    }
}
