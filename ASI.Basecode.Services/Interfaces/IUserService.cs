using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System.Collections.Generic;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IUserService
    {
        LoginResult AuthenticateUser(string userid, string password, ref User user);
        void AddUser(UserViewModel model);
        UserViewModel RetrieveUser(int Id);
        IEnumerable<UserViewModel> RetrieveActiveNonAdminUsers();
        void UpdateUser(UserViewModel model);
        void DeleteUser(int Id);
        bool AdminExists();

        // Add the missing method - Camus 
        List<UserViewModel> GetUsersBySearchTerm(string term);
    }
}
