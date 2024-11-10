 using ASI.Basecode.Data;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    

    public class BookingService: IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public BookingService(IBookingRepository bookingRepository, IMapper mapper, IConfiguration configuration)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _config = configuration;
        }
        public void AddBooking(BookingViewModel model, string userId)
        {
            var newBooking = new Booking();
            _mapper.Map(model, newBooking);
            newBooking.StartDate = model.StartDate.Date; // Only store the date part in StartDate
            newBooking.EndDate = model.EndDate.Date;     // Only store the date part in EndDate
            newBooking.Status = "Pending";
            newBooking.IsDeleted = false;
            newBooking.CreatedBy = userId;
            newBooking.UpdatedBy = userId;
            newBooking.Username = userId;
            newBooking.CreatedTime = DateTime.Now;
            newBooking.UpdatedTime = DateTime.Now;

            _bookingRepository.AddBooking(newBooking);
        }

        public IEnumerable<BookingViewModel> RetrieveActiveBookings(string userId)
        {
            // Retrieve all users and apply the necessary filtering
            var bookings = _bookingRepository.RetrieveAll()
                                       .Where(booking => booking.IsDeleted != true && booking.Username == userId)
                                       .Select(booking => new BookingViewModel
                                       {
                                           BookingId = booking.BookingId,
                                           Purpose = booking.Purpose,
                                           Status = booking.Status,
                                           StartDate = booking.StartDate,
                                           EndDate = booking.EndDate,
                                           RoomId = booking.RoomId,
                                           RoomName = booking.Room.RoomName
                                       });

            return bookings;
        }
        public BookingViewModel RetrieveBooking(int Id)
        {
            var booking = _bookingRepository.RetrieveAll()
                                    .Where(x => x.BookingId == Id)
                                    .Select(s => new BookingViewModel
                                    {
                                        BookingId = s.BookingId,
                                        Purpose = s.Purpose,
                                        Status = s.Status,
                                        StartDate = s.StartDate,
                                        EndDate = s.EndDate,
                                        RoomId = s.RoomId,
                                        RoomName = s.Room.RoomName
                                    })
                                    .FirstOrDefault();

            return booking;
        }
        public void UpdateBooking(BookingViewModel model, string userId)
        {
            
            var booking = _bookingRepository.RetrieveAll().FirstOrDefault(x => x.BookingId == model.BookingId);

            if (booking != null)
            {
                // Check if RoomId exists in the Room table if RoomId is being changed
                var room = _bookingRepository.RetrieveAll()
                            .Where(b => b.RoomId == model.RoomId)
                            .Select(b => b.Room)
                            .FirstOrDefault();

                if (room == null)
                {
                    throw new InvalidOperationException($"Room with ID {model.RoomId} does not exist.");
                }

                
                _mapper.Map(model, booking);
                booking.Status = "Pending";
                booking.UpdatedTime = DateTime.Now;
                booking.UpdatedBy = userId;

               
                _bookingRepository.UpdateBooking(booking);
            }
        }
        public void DeleteBooking(int Id)
        {
            var booking = _bookingRepository.RetrieveAll().FirstOrDefault(x => x.BookingId == Id);
            if (booking != null)
            {
                _bookingRepository.DeleteBooking(booking);
            }
            else
            {
                throw new InvalidOperationException("Booking not found.");
            }
        }
    }
}
