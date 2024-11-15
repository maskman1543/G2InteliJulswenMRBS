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
            this.GetDbSet<Booking>().Update(model);
            UnitOfWork.SaveChanges();
        }
    }
}