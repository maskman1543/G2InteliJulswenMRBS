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
            var data = _roomService.RetrieveActiveRooms();
            return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CreateRoomModal()
        {
            return PartialView("Create");
        }

        [HttpGet("/Room/Edit/{Id}")]
        public IActionResult Edit(int Id)
        {
            var data = _roomService.RetrieveRoom(Id);
            return View(data);
        }

        [HttpGet("/Room/Delete/{Id}")]
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
                return Json(new { success = true, message = "Room created successfully!" });
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

        [HttpPost("/Room/Edit/{Id}")]
        public IActionResult Edit(RoomViewModel model)
        {
            _roomService.UpdateRoom(model, UserId);
            return RedirectToAction("RoomManagement");
        }

        [HttpPost("/Room/Delete/{Id}")]
        public IActionResult SoftDelete(int Id)
        {
            _roomService.DeleteRoom(Id);
            return RedirectToAction("RoomManagement");
        }
        #endregion

    }
}
