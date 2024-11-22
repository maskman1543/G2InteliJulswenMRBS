 using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Models;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public IActionResult ViewBooking(int pg = 1)
        {
            HttpContext.Session.SetString("IsViewBookingActive", "true");
            
            List<BookingViewModel> rooms = _bookingService.RetrieveAllBookings().ToList();


            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = rooms.Count;

            var pager = new Pager(recsCount, pg, pageSize);
            int recsSkip = (pg - 1) * pageSize;

            var data1 = rooms.Skip(recsSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;


            return View(data1);
        }
        #endregion

    }
}
