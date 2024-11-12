using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZXing.Client.Result;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UserService(IUserRepository repository, IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _repository = repository;
            _config = configuration;

        }
        public LoginResult AuthenticateUser(string userId, string password, ref User user)
        {
            user = new User();
            var passwordKey = PasswordManager.EncryptPassword(password);
            user = _repository.GetUsers().Where(x => x.UserId == userId &&
                                                     x.Password == passwordKey).FirstOrDefault();

            return user != null ? LoginResult.Success : LoginResult.Failed;
        }

        public void AddUser(UserViewModel model)
        {
            var user = new User();
            if (!_repository.UserExists(model.UserId))
            {
                _mapper.Map(model, user);
                user.Password = PasswordManager.EncryptPassword(model.Password);
                user.CreatedTime = DateTime.Now;
                user.UpdatedTime = DateTime.Now;
                user.CreatedBy = user.Name;
                user.UpdatedBy = user.Name;
                user.IsDeleted = false;

                // Assign roles based on the existence of an admin
                if (!_repository.AdminExists())
                {
                    user.Roles = "Admin"; // First user is admin
                }
                else
                {
                    user.Roles = "User"; // Subsequent users are regular users
                }

                _repository.AddUser(user);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.UserExists);
            }
        }

        public UserViewModel RetrieveUser(int Id)
        {
            var user = _repository.RetrieveAll().Where(x => x.Id.Equals(Id)).Select(s => new UserViewModel
            {
                Id = s.Id,
                UserId = s.UserId,
                Name = s.Name,
                Roles = s.Roles,
                Password = s.Password,
            }).FirstOrDefault();

            return user;
        }

        public IEnumerable<UserViewModel> RetrieveActiveNonAdminUsers()
        {
            // Retrieve all users and apply the necessary filtering
            var users = _repository.RetrieveAll()
                                       .Where(user => user.IsDeleted != true && !user.Roles.Contains("Admin"))
                                       .Select(user => new UserViewModel
                                       {
                                           Id = user.Id,
                                           UserId = user.UserId,
                                           Name = user.Name,
                                           Roles = user.Roles,
                                           Password= user.Password,
                                       });

            return users;
        }

        public void UpdateUser(UserViewModel model)
        {
            var user = _repository.RetrieveAll().Where(x => x.Id.Equals(model.Id)).FirstOrDefault();
            if (user != null)
            {
                _mapper.Map(model, user);
                user.Password = PasswordManager.EncryptPassword(model.Password);
                user.UpdatedTime = DateTime.Now;
                user.UpdatedBy = user.Name;

                _repository.UpdateUser(user);
            }
        }
        public void DeleteUser(int Id)
        {
            var user = _repository.RetrieveAll().FirstOrDefault(x => x.Id == Id);
            if (user != null)
            {
                _repository.DeleteUser(user);
            }
        }

        public bool AdminExists()
        {
            return _repository.AdminExists();
        }

    }
}
