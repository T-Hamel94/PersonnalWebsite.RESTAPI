

using PersonnalWebsite.RESTAPI.Entities;

namespace PersonnalWebsite.RESTAPI.Model
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }

        public UserModel() { }

        public UserModel(Guid id, string lastName, string firstName, string email, int age, DateTime createdAt, DateTime lastModifiedAt)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Age = age;
            CreatedAt = createdAt;
            LastModifiedAt = lastModifiedAt;
        }

        public User ToEntity()
        {
            return new User(Id, LastName, FirstName, Email, Age, CreatedAt, LastModifiedAt);
        }
    }
}
