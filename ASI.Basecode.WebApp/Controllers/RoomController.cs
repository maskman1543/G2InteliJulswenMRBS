 using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ASI.Basecode.WebApp.Controllers
{
    public class RoomController : ControllerBase<RoomController>
    {
        private readonly IRoomService _roomService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="roomService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="mapper"></param>
        public RoomController(
                            IHttpContextAccessor httpContextAccessor,
                            ILoggerFactory loggerFactory,
                            IConfiguration configuration,
                            IMapper mapper, 
                            IRoomService roomService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _roomService = roomService;
        }

        /// <summary>
        /// reutrns Book View
        /// </summary>
        /// <returns>Book view</returns>        
        public IActionResult Index()
        {
            var data = _roomService.RetrieveAll();
            return View(data);
        }

        #region Get Methods
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
                var data = _roomService.RetrieveRoom(Id);
                return View(data);
        }


        [HttpGet]
        public IActionResult Delete(int Id)
        {
                var data = _roomService.RetrieveRoom(Id);
                return View(data);
        }


        #endregion


        #region Post Methods

        [HttpPost]
        public IActionResult Create(RoomViewModel model)
        {
                _roomService.AddRoom(model, UserId);
                return RedirectToAction("Create");
        }

        [HttpPost]
        public IActionResult Edit(RoomViewModel model)
        {
                _roomService.UpdateRoom(model, UserId);
                return RedirectToAction("Index");
            }

        [HttpPost]
        public IActionResult PostDelete(int Id)
        {
                _roomService.DeleteRoom(Id);
                return RedirectToAction("Index");
        }

        #endregion
    }
}
