using Srv_PersonnalWebsite.Entity;

namespace PersonnalWebsite.RESTAPI.Model
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }

        public UserModel() { }

        public UserModel(Guid id, string name, string firstName, string email, int age, DateTime createdAt, DateTime lastModifiedAt)
        {
            Id = id;
            Name = name;
            FirstName = firstName;
            Email = email;
            Age = age;
            CreatedAt = createdAt;
            LastModifiedAt = lastModifiedAt;
        }

        public User ToEntity()
        {
            return new User(Id, Name, FirstName, Email, Age, CreatedAt, LastModifiedAt);
        }
    }
}
