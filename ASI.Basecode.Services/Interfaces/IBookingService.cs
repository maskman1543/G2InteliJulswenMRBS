using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IBookingService
    {
        void AddBooking(BookingViewModel model, string userId);
        IEnumerable<BookingViewModel> RetrieveAllBookings();
        IEnumerable<BookingViewModel> RetrieveActiveBookings(string userId);
        BookingViewModel RetrieveBooking(int BookingId);
        void UpdateBooking(BookingViewModel model, string userId);
        void DeleteBooking(int Id);
    }
}
