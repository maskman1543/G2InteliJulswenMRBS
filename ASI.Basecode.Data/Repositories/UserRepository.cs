using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Client.Result;

namespace ASI.Basecode.Data.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public IQueryable<User> GetUsers()
        {
            return this.GetDbSet<User>();
        }

        public bool UserExists(string userId)
        {
            return this.GetDbSet<User>().Any(x => x.UserId == userId);
        }

        public bool IsUserDeleted(string userId)
        {
            var user = this.GetDbSet<User>().FirstOrDefault(x => x.UserId == userId);
            return user != null && user.IsDeleted;
        }

        public void AddUser(User user)
        {
            this.GetDbSet<User>().Add(user);
            UnitOfWork.SaveChanges();
        }

        public IEnumerable<User> RetrieveAll()
        {
            return this.GetDbSet<User>();
        }

        public void UpdateUser(User user)
        {
            this.GetDbSet<User>().Update(user);
            UnitOfWork.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            user.IsDeleted = true;
            this.GetDbSet<User>().Update(user);
            UnitOfWork.SaveChanges();
        }

        public bool AdminExists()
        {
            return this.GetDbSet<User>().Any(u => u.Roles.Contains("Admin"));
        }

        // New implementation for searching rooms - Camus 
        public IEnumerable<User> SearchUser(string term)
        {   
                return this.GetDbSet<User>()
                            .Where(r => !r.IsDeleted && (r.Name.Contains(term) || r.UserId.Contains(term)))
                            .ToList();
        }
    }
}
