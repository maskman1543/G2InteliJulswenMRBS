 using ASI.Basecode.Services.Interfaces;
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

        [HttpGet("/BookingUser/Create/{Id}")]
        public IActionResult Create(int Id)
        {
            var model = new BookingViewModel
            {
                RoomId = Id // Set the RoomId to the selected room
            };

            return View(model);
        }

        [HttpGet("/BookingUser/Edit/{Id}")]
        public IActionResult Edit(int Id)
        {
            var data = _bookingService.RetrieveBooking(Id);
            return View(data);
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
        [HttpPost("/BookingUser/Create/{Id}")]
        public IActionResult Create(BookingViewModel model)
        {
            if (ModelState.IsValid)
            {   
                _bookingService.AddBooking(model, UserId);

                TempData["SuccessMessage3"] = "Booking created successfully!";
                return RedirectToAction("UserDashboard", "Home");
            }
            else
            {
                // If the model is invalid, set error message
                TempData["ErrorMessage"] = "There was an issue with your submission. Please check the form and try again.";
                return View(model); 
            }
           
        }

        [HttpPost("/BookingUser/Edit/{Id}")]
        public IActionResult Edit(BookingViewModel model)
        {
            if (ModelState.IsValid)
            { 
                model.Status = "Booked";
                _bookingService.UpdateBooking(model, UserId);

                // TempData message to show success
                TempData["SuccessMessage"] = "Booking updated successfully!";
                return RedirectToAction("ViewBooking");
            }
            else
            {
                // If the model is invalid, set error message
                TempData["ErrorMessage"] = "There was an issue with your submission. Please check the form and try again.";
                return View(model); 
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
