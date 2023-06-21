using PersonnalWebsite.RESTAPI.CustomExceptions;
using PersonnalWebsite.RESTAPI.Data.Context;
using PersonnalWebsite.RESTAPI.Data.SQLServerEntity;
using PersonnalWebsite.RESTAPI.Entities;
using PersonnalWebsite.RESTAPI.Interfaces;

namespace PersonnalWebsite.RESTAPI.Data.Repo.SQLServerRepo
{
    // Uses UserSqlDTO and returns User
    public class UserSqlRepo : IUserRepo
    {
        private ApplicationDbContext _dbContext;

        public UserSqlRepo()
        {
            _dbContext = DbContextGeneration.GetApplicationDBContext();
        }
        public IEnumerable<User> GetUsers()
        {
            List<UserSQLServer> usersSQLDTO = _dbContext.Users.ToList();

            return usersSQLDTO.Select(u => u.ToEntity()).ToList();
        }

        public User GetUserByID(Guid userID)
        {
            if (userID == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userID));
            }

            UserSQLServer user = _dbContext.Users.Where(u => u.UserID == userID).SingleOrDefault();

            if (user is null)
            {
                throw new UserNotFoundException($"Could not find user with id: {userID}");
            }

            return user.ToEntity();
        }

        public User GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            UserSQLServer userSQLDTO = _dbContext.Users.Where(u => u.Email == email).FirstOrDefault();

            if (userSQLDTO is null)
            {
                throw new UserNotFoundException($"Could not find user with email: {email}");
            }

            return userSQLDTO.ToEntity();
        }

        public User GetUserByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            UserSQLServer userSQLDTO = _dbContext.Users.Where(u => u.Username == username).FirstOrDefault();

            if (userSQLDTO is null)
            {
                throw new UserNotFoundException($"Could not find user with username: {username}");
            }

            return userSQLDTO.ToEntity();
        }

        public bool UserExistsByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            return _dbContext.Users.Any(u => u.Email == email);
        }

        public bool UserExistsByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            return _dbContext.Users.Any(u => u.Username == username);
        }

        public User CreateUser(User user)
        {
            UserSQLServer userSQLDTO = new UserSQLServer(user);
            _dbContext.Users.Add(userSQLDTO);
            _dbContext.SaveChanges();

            return userSQLDTO.ToEntity();
        }

        public User UpdateUser(User userToUpdate)
        {
            UserSQLServer existingUser = _dbContext.Users.Find(userToUpdate.Id);  

            if(existingUser == null)
            {
                throw new UserNotFoundException($"Could not find user with id: {userToUpdate.Id}");
            }

            userToUpdate.LastModifiedAt = DateTime.Now;
            _dbContext.Users.Update(new UserSQLServer(userToUpdate));
            _dbContext.SaveChanges();

            return userToUpdate;
        }

        public void DeleteUser(Guid userID)
        {
            UserSQLServer existingUser = _dbContext.Users.Find(userID);

            if(existingUser == null)
            {
                throw new UserNotFoundException($"User not found with Guid: {userID}");
            }

            _dbContext.Users.Remove(existingUser);
            _dbContext.SaveChanges();
        }
    }
}
