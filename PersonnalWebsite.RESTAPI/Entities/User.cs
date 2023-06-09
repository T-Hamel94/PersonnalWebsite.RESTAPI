﻿using PersonnalWebsite.RESTAPI.Model;

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


        public User(Guid id, string lastName, string firstName, string username, string email, bool isAdmin, DateTime birthdate, DateTime createdAt, DateTime lastModifiedAt, byte[] passwordHash, byte[] passwordSalt)
        {
            Id = id;
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

        public UserModel ToModel()
        {
            return new UserModel(Id, LastName, FirstName, Username, Email, Birthdate, IsAdmin, CreatedAt, LastModifiedAt);
        }

        public UserPublicModel ToPublicModel()
        {
            return new UserPublicModel(Id, LastName, FirstName, Username, CreatedAt);
        }

        public UserRegistrationModel ToRegistrationModel()
        {
            return new UserRegistrationModel(Id, LastName, FirstName, Username, string.Empty, Email, Birthdate, CreatedAt, LastModifiedAt);
        }

    }
}
