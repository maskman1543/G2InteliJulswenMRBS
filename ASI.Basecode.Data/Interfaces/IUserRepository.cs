using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<User> GetUsers();
        bool UserExists(string userId);
        void AddUser(User user);
        IEnumerable<User> RetrieveAll();
        void UpdateUser(User user);
        void DeleteUser(User user);
        bool AdminExists();
        bool IsUserDeleted(string userId);

        //for searching added by camus  
        IEnumerable<User> SearchUser(string term);
    }
}
