﻿using ASI.Basecode.Data.Interfaces;
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
                user.CreatedBy = "Admin";
                user.UpdatedBy = "Admin";
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

        public bool ActiveUser(string userId)
        {
            var activeUser = _repository.GetActiveUserByUserId(userId);
            return activeUser != null;
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
            // Retrieve the user from the database
            var user = _repository.RetrieveAll().Where(x => x.Id.Equals(model.Id)).FirstOrDefault();
            if (user != null)
            {
                if(!_repository.UserExists(model.UserId))
                {
                    // Preserve the existing Roles value
                    var existingRole = user.Roles;

                    // Map other properties from the model, excluding the role
                    _mapper.Map(model, user);
                    user.Password = PasswordManager.EncryptPassword(model.Password);
                    user.UpdatedTime = DateTime.Now;
                    user.UpdatedBy = user.Name;

                    // Restore the original Roles value
                    user.Roles = existingRole;

                    // Update the user in the database
                    _repository.UpdateUser(user);
                }
                else
                {
                    throw new InvalidDataException(Resources.Messages.Errors.UserExists);
                }
                
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.ServerError);
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


        public List<UserViewModel> GetUsersBySearchTerm(string term)
        {
            // Retrieve all active non-admin users
            var users = RetrieveActiveNonAdminUsers()
                .Where(u =>
                    u.UserId.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    u.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    u.Roles.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    UserId = u.UserId,
                    Name = u.Name,
                    Roles = u.Roles
                }).ToList();

            Console.WriteLine($"Filtered Users: {users.Count}"); // Log count of filtered users
            return users;   
        }
    }
}
