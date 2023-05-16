﻿using PersonnalWebsite.RESTAPI.Data.Repo;
using PersonnalWebsite.RESTAPI.Entities;
using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Model;

namespace PersonnalWebsite.RESTAPI.Service
{
    // Uses User and returns UserModel
    public class UserService : IUserService
    {
        private IUserRepo _userRepo;
        private IAuthService _authService;

        public UserService(IUserRepo userRepo, IAuthService authService)
        {
            _userRepo = userRepo;
            _authService = authService;
        }

        public UserModel RegisterUser(UserRegistrationModel model)
        {
            _authService.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Email = model.Email,
                Age = model.Age,
                CreatedAt = DateTime.UtcNow,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };

            _userRepo.CreateUser(user);

            return user.ToModel();
        }

        public UserModel CreateUser(UserModel user)
        {
            User createdUser = _userRepo.CreateUser(user.ToEntity());

            return createdUser.ToModel();
        }

        public UserModel GetUserByID(Guid p_userGuid)
        {
            UserModel user = _userRepo.GetUserByID(p_userGuid).ToModel();

            return user;
        }

        public UserModel GetUserByEmail(string email)
        {
            User user = _userRepo.GetUserByEmail(email);

            return user?.ToModel();
        }

        public IEnumerable<UserModel> GetUsers()
        {
            IEnumerable<UserModel> users = _userRepo.GetUsers().Select(u => u.ToModel());

            return users;
        }

        public UserModel UpdateUser(UserModel user)
        {
            User userToUpdate = _userRepo.UpdateUser(user.ToEntity());

            return userToUpdate.ToModel();
        }

        public void DeactivateUser(Guid p_user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(Guid p_userGuid)
        {
            throw new NotImplementedException();
        }

    }
}