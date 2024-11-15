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
            newBooking.StartDate = model.StartDate.Date;
            newBooking.StartTime = new TimeSpan(model.StartTime.Hours, model.StartTime.Minutes, 0);  
            newBooking.EndTime = new TimeSpan(model.EndTime.Hours, model.EndTime.Minutes, 0);  
            newBooking.Status = "Booked";
            newBooking.IsDeleted = false;
            newBooking.CreatedBy = userId;
            newBooking.UpdatedBy = userId;
            newBooking.Username = userId;
            newBooking.CreatedTime = DateTime.Now;
            newBooking.UpdatedTime = DateTime.Now;

            _bookingRepository.AddBooking(newBooking);
        }
        public IEnumerable<BookingViewModel> RetrieveAllBookings()
        {
            var bookings = _bookingRepository.RetrieveAll()
                                             .Where(booking => booking.IsDeleted = true)
                                             .Select(booking => new BookingViewModel
                                             {
                                                 BookingId = booking.BookingId,
                                                 Purpose = booking.Purpose,
                                                 Status = booking.Status,
                                                 StartDate = booking.StartDate,
                                                 StartTime = new TimeSpan(booking.StartTime.Hours, booking.StartTime.Minutes, 0),
                                                 EndTime = new TimeSpan(booking.EndTime.Hours, booking.EndTime.Minutes, 0),
                                                 RoomId = booking.RoomId,
                                                 RoomName = booking.Room.RoomName,
                                                 Username = booking.Username
                                             })
                                             .ToList();

            return bookings;
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
                                           StartTime = new TimeSpan(booking.StartTime.Hours, booking.StartTime.Minutes, 0),
                                           EndTime = new TimeSpan(booking.EndTime.Hours, booking.EndTime.Minutes, 0),
                                           RoomId = booking.RoomId,
                                           RoomName = booking.Room.RoomName
                                       });

            return bookings;
        }
        public BookingViewModel RetrieveBooking(int Id)
        {
            var booking = _bookingRepository.RetrieveAll()
                                    .Where(x => x.BookingId == Id)
                                    .Select(booking => new BookingViewModel
                                    {
                                        BookingId = booking.BookingId,
                                        Purpose = booking.Purpose,
                                        Status = booking.Status,
                                        StartDate = booking.StartDate,
                                        StartTime = new TimeSpan(booking.StartTime.Hours, booking.StartTime.Minutes, 0),
                                        EndTime = new TimeSpan(booking.EndTime.Hours, booking.EndTime.Minutes, 0),
                                        RoomId = booking.RoomId,
                                        RoomName = booking.Room.RoomName
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
                booking.Status = "Booked";
                booking.UpdatedTime = DateTime.Now;
                booking.UpdatedBy = userId;
                booking.Username = userId;

               
                _bookingRepository.UpdateBooking(booking);
            }
        }


        public void DeleteBooking(int Id)
        {
            var booking = _bookingRepository.RetrieveAll().FirstOrDefault(x => x.BookingId == Id);
             if (booking != null)
    {
        // Update the status and mark as deleted
        booking.Status = "Cancelled";
        booking.IsDeleted = true;
        booking.UpdatedTime = DateTime.Now;

        // Save the changes to the repository
        _bookingRepository.UpdateBooking(booking);
    }
    else
    {
        throw new InvalidOperationException("Booking not found.");
    }
        }
    }
}
