using PersonnalWebsite.RESTAPI.Entities;
using PersonnalWebsite.RESTAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PersonnalWebsite.RESTAPI.Test.TestHelper
{
    public static class UserHelper
    {
        public static User GenerateUser()
        {
            return new User()
            {       
                Id = Guid.NewGuid(),
                Username = "Nietzschy",
                FirstName = "Friedrich",
                LastName = "Nietzsche",
                Email= "Niestzche44@gmail.com",
                IsAdmin = false,
                PasswordHash = new byte[10],
                PasswordSalt = new byte[10],
                Birthdate=DateTime.Now.AddYears(-29),
                CreatedAt = DateTime.Now.AddHours(-5),
                LastModifiedAt = DateTime.Now.AddHours(-5),
            };
        }

        public static User GenerateAdminUser()
        {
            return new User()
            {
                Id = Guid.NewGuid(),
                Username = "Marximus",
                FirstName = "Karl",
                LastName = "Marx",
                Email = "KarloMarxo@mail.org",
                IsAdmin = true,
                PasswordHash = new byte[10],
                PasswordSalt = new byte[10],
                Birthdate = DateTime.Now.AddYears(-120),
                CreatedAt = DateTime.Now.AddHours(-5),
                LastModifiedAt = DateTime.Now.AddHours(-5),
            };
        }

        public static UserRegistrationModel GenerateUserRegistrationModel()
        {
            return new UserRegistrationModel()
            {
                Id = Guid.NewGuid(),
                FirstName = "John ",
                LastName = "F. Kennedy",
                Username = "JFK",
                Password = "peaceandlove",
                Email = "JFK@us.gov",
                Birthdate = DateTime.Now.AddYears(-90),
                CreatedAt = DateTime.Now,
                LastModifiedAt = DateTime.Now,
            };
        }

        public static List<User> GenerateListOf5Users()
        {
            return new List<User>()
            {
                new User
                {   Id = Guid.NewGuid(),
                    Username = "Nietzschy_4",
                    FirstName = "Friedrich",
                    LastName = "Nietzsche",
                    Email= "Niestzche44@gmail.com",
                    IsAdmin = false,
                    PasswordHash = new byte[10],
                    PasswordSalt = new byte[10],
                    Birthdate=DateTime.Now.AddYears(-29),
                    CreatedAt = DateTime.Now.AddHours(-5),
                    LastModifiedAt = DateTime.Now.AddHours(-5),
                },

                new User
                {   Id = Guid.NewGuid(),
                    Username = "JFK_2",
                    FirstName = "John .F",
                    LastName = "Kennedy",
                    Email= "JFK@gmail.com",
                    IsAdmin = false,
                    PasswordHash = new byte[10],
                    PasswordSalt = new byte[10],
                    Birthdate=DateTime.Now.AddYears(-69),
                    CreatedAt = DateTime.Now.AddHours(-5),
                    LastModifiedAt = DateTime.Now.AddHours(-5),
                },

                new User
                {   Id = Guid.NewGuid(),
                    Username = "b_gates_1",
                    FirstName = "Bill",
                    LastName = "Gates",
                    Email= "billgates@hotmail.com",
                    IsAdmin = true,
                    PasswordHash = new byte[10],
                    PasswordSalt = new byte[10],
                    Birthdate=DateTime.Now.AddYears(-39),
                    CreatedAt = DateTime.Now.AddHours(-5),
                    LastModifiedAt = DateTime.Now.AddHours(-5),
                },

                new User
                {   Id = Guid.NewGuid(),
                    Username = "krusty_3",
                    FirstName = "Krusty",
                    LastName = "Klown",
                    Email= "KTK@gmail.com",
                    IsAdmin = true,
                    PasswordHash = new byte[10],
                    PasswordSalt = new byte[10],
                    Birthdate = DateTime.Now.AddYears(-19),
                    CreatedAt = DateTime.Now.AddHours(-5),
                    LastModifiedAt = DateTime.Now.AddHours(-5),
                },

                new User
                {   Id = Guid.NewGuid(),
                    Username = "TonyMiami_5",
                    FirstName = "Tony",
                    LastName = "Montana",
                    Email= "worldisurs@gmail.com",
                    IsAdmin = true,
                    PasswordHash = new byte[10],
                    PasswordSalt = new byte[10],
                    Birthdate = DateTime.Now.AddYears(-32),
                    CreatedAt = DateTime.Now.AddHours(-5),
                    LastModifiedAt = DateTime.Now.AddHours(-5),
                },
            };
        }

        public static List<UserPublicModel> GenerateListOf5UsersPublicModel()
        {
            return GenerateListOf5Users().Select(u => u.ToPublicModel()).ToList();
        }

        public static ClaimsPrincipal GenerateClaimsPrincipal(Guid userId)
        {
            var identity = new ClaimsIdentity(new[] 
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                
            });

            return new ClaimsPrincipal(identity);
        }
    }
}
