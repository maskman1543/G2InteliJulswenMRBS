using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IBookingRepository
    {
        void AddBooking(Booking model);
        IEnumerable<Booking> RetrieveAll();
        void UpdateBooking(Booking model);
        void DeleteBooking(Booking Booking);
    }
}
