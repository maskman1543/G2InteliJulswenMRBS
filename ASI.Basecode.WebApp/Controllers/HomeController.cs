using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.WebApp.Controllers
{
    /// <summary>
    /// Home Controller
    /// </summary>
    public class HomeController : ControllerBase<HomeController>
    {
        private readonly IRoomService _roomService;
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
                             IRoomService roomService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _roomService = roomService;
        }

        /// <summary>
        /// Returns Home View.
        /// </summary>
        /// <returns> Home View </returns>



        #region Get Methods
        [HttpGet]
        public IActionResult AdminDashboard()
        {
            return View();
        }
        [HttpGet]
        public IActionResult UserDashboard()
        {
            var data = _roomService.RetrieveAll();
            return View(data);
        }
        #endregion

        #region Post Methods
        #endregion
    }
}
