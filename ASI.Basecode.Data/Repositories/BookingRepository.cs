using ASI.Basecode.Data.Interfaces;
using Basecode.Data.Repositories;
using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore;
namespace ASI.Basecode.Data.Repositories
{
    public class BookingRepository : BaseRepository, IBookingRepository
    {
        public BookingRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        // Method to get UserId by User's Id (Primary Key)
        public string GetUserIdByUserId(int userId)
        {
            // Query the User table for the specified UserId and return the UserId field
            var user = this.GetDbSet<User>()
                           .FirstOrDefault(u => u.Id == userId); // Query based on the User's primary key (Id)

            return user?.UserId;  // Return the UserId or null if not found
        }
        public void AddBooking(Booking model)
        {
            this.GetDbSet<Booking>().Add(model);
            UnitOfWork.SaveChanges();
        }
        public IEnumerable<Booking> RetrieveAll()
        {
            return this.GetDbSet<Booking>()
                      .Include(b => b.Room) // Include the Room navigation property
                      .ToList();
        }
        public void UpdateBooking(Booking model)
        {
            this.GetDbSet<Booking>().Update(model);
            UnitOfWork.SaveChanges();
        }
        public void DeleteBooking(Booking model)
        {
            model.IsDeleted = true;
            this.GetDbSet<Booking>().Update(model);
            UnitOfWork.SaveChanges();
        }
    }
}