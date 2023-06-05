using PersonnalWebsite.RESTAPI.Entities;

namespace PersonnalWebsite.RESTAPI.Model
{
    public class UserPublicModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserPublicModel() { }

        public UserPublicModel(Guid id, string lastName, string firstName, string username, DateTime createdAt)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            CreatedAt = createdAt;
        }
    }
}
