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

         #region Get Methods
        [HttpGet]
        public IActionResult RoomManagement()
        {
            HttpContext.Session.SetString("IsRoomManagementActive", "true");
            HttpContext.Session.Remove("IsUserManagementActive");
            HttpContext.Session.Remove("IsViewBookingActive");
            var data = _roomService.RetrieveActiveRooms();
            return View(data);
        }
        public IActionResult Create()
        {
            HttpContext.Session.Remove("IsRoomManagementActive");
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
            try
            {
                _roomService.AddRoom(model, UserId);
                TempData["SuccessMessage1"] = "Room created successfully!";
                return View();
            }
            catch (InvalidDataException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Resources.Messages.Errors.ServerError;
            }
            return View();
        }

        [HttpPost]
        public IActionResult Edit(RoomViewModel model)
        {
            _roomService.UpdateRoom(model, UserId);
            return RedirectToAction("RoomManagement");
        }

        [HttpPost]
        public IActionResult SoftDelete(int Id)
        {
            _roomService.DeleteRoom(Id);
            return RedirectToAction("RoomManagement");
        }
        #endregion

    }
}
