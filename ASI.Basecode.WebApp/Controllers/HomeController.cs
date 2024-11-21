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
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    /// <summary>
    /// Home Controller
    /// </summary>
    public class HomeController : ControllerBase<HomeController>
    {
        private readonly IRoomService _roomService;
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="localizer"></param>
        /// <param name="mapper"></param>
        public HomeController(IHttpContextAccessor httpContextAccessor,
                             ILoggerFactory loggerFactory,
                             IConfiguration configuration,
                             IMapper mapper,
                             IRoomService roomService,
                             IUserService userService,
                             IBookingService bookingService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _roomService = roomService;
            _userService = userService;
            _bookingService = bookingService;
        }

        /// <summary>
        /// Returns Home View.
        /// </summary>
        /// <returns> Home View </returns>



        #region Get Methods
        [HttpGet]
        public IActionResult AdminDashboard()
        {
            // Get the count of active rooms
            var roomCount = _roomService.RetrieveActiveRooms().Count();
            var userCount = _userService.RetrieveActiveNonAdminUsers().Count();
            var BookingCount = _bookingService.RetrieveAllBookings().Count();
            ViewBag.UserCount = userCount;
            ViewBag.RoomCount = roomCount; // Passing the count to the view
            ViewBag.BookingCount = BookingCount;
            return View();
        }

        [HttpGet]
        public IActionResult UserDashboard(int pg = 1)
        {
            
            
            List<RoomViewModel> rooms = _roomService.RetrieveActiveRooms().ToList();
          
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

        #region Post Methods
        #endregion
    }
}
