﻿ using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;

namespace ASI.Basecode.WebApp.Controllers
{
    public class BookingUserController : ControllerBase<BookingUserController>
    {
        private readonly IBookingService _bookingService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bookingService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="mapper"></param>
        public BookingUserController(
                            IHttpContextAccessor httpContextAccessor,
                            ILoggerFactory loggerFactory,
                            IConfiguration configuration,
                            IMapper mapper, 
                            IBookingService bookingService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _bookingService = bookingService;
        }


        #region Get Methods
        [HttpGet]
        public IActionResult ViewBooking()
        {
            HttpContext.Session.SetString("IsViewBookingActive", "true");
            var data = _bookingService.RetrieveActiveBookings(UserId);
            return View(data);
        }

        [HttpGet]
        public IActionResult CreateBookingModal(int roomId)
        {
            var model = new BookingViewModel
            {
                RoomId = roomId
            };
            return PartialView("Create", model);
        }

        [HttpGet]
        public IActionResult EditBookingModal(int BookingId)
        {
            // Retrieve the Room based on the ID
            var booking = _bookingService.RetrieveBooking(BookingId);
            if (booking == null)
            {
                return NotFound();
            }

            // Create the RoomViewModel to pass the data to the modal view
            var viewModel = new BookingViewModel
            {
                BookingId = booking.BookingId,
                Purpose = booking.Purpose,
                StartDate = booking.StartDate,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Status = booking.Status,
                RoomId = booking.RoomId,
                RoomName = booking.RoomName,
            };

            //Return the partial view with the room data for editing
            return PartialView("Edit", viewModel);
        }

        [HttpGet("/BookingUser/Delete/{Id}")]
        public IActionResult Delete(int Id)
        {
            var data = _bookingService.RetrieveBooking(Id);  // Retrieves the booking details
            if (data == null)
            {
                TempData["ErrorMessage"] = "Booking not found!";  // Show error if booking not found
                return RedirectToAction("ViewBooking");
            }
            return View(data);
        }
        #endregion

        #region Post Methods
        [HttpPost]
        public IActionResult Create(BookingViewModel model)
        {
            try
            {   
                _bookingService.AddBooking(model, UserId);
                TempData["SuccessMessage"] = "Booking created successfully!";
                return Json(new { success = true, message = "Booking created successfully!" });
            }
            catch (InvalidDataException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = Resources.Messages.Errors.ServerError });
            }

        }

        [HttpPost]
        public IActionResult Edit(BookingViewModel model)
        {
            try
            {
                model.Status = "Booked";
                // Call your service to update the booking
                _bookingService.UpdateBooking(model, UserId);

                // Return success response
                return Json(new { success = true, message = "Booking updated successfully!" });

            }
            catch (InvalidDataException ex)
            {
                // Handle known exceptions (e.g., validation failures)
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                // Handle any unforeseen exceptions
                return Json(new { success = false, message = Resources.Messages.Errors.ServerError });
            }
        }

        [HttpPost("/BookingUser/Delete/{BookingId}")]
        public IActionResult SoftDelete(int BookingId)
        {
            try
            {
                _bookingService.DeleteBooking(BookingId);  // Call service to delete the booking
                TempData["SuccessMessage"] = "Booking deleted successfully!";  // Show success message
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;  // Handle error and display message
            }

            return RedirectToAction("ViewBooking");
        }
        #endregion

    }
}
