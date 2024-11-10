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
    public class BookingAdminController : ControllerBase<BookingAdminController>
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
        public BookingAdminController(
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
            var data = _bookingService.RetrieveAllBookings();
            return View(data);
        }
        #endregion

        #region Post Methods
        [HttpPost]
        public IActionResult ChangeStatus(int bookingId, string status)
        {
            if (status != "Approved" && status != "Disapproved" && status != "Pending")
            {
                return Json(new { success = false, message = "Invalid status value." });
            }

            try
            {
                _bookingService.UpdateBookingStatus(bookingId, status);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion
    }
}
