using PersonnalWebsite.RESTAPI.Model;

namespace PersonnalWebsite.RESTAPI.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }

        public User() { }

        public User(Guid id, string name, string firstName, string email, int age, DateTime createdAt, DateTime lastModifiedAt)
        {
            Id = id;
            Name = name;
            FirstName = firstName;
            Email = email;
            Age = age;
            CreatedAt = createdAt;
            LastModifiedAt = lastModifiedAt;
        }

        public UserModel ToModel()
        {
            return new UserModel(Id, Name, FirstName, Email, Age, CreatedAt, LastModifiedAt);
        }
    }
}
