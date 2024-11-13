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

        [HttpGet]
        public IActionResult EditRoomModal(int id)
        {
            // Retrieve the Room based on the ID
            var room = _roomService.RetrieveRoom(id);
            if (room == null)
            {
                return NotFound();
            }

            // Create the RoomViewModel to pass the data to the modal view
            var viewModel = new RoomViewModel
            {
                Id = room.Id,
                RoomName = room.RoomName,
                Capacity = room.Capacity,
                Location = room.Location,
                Equipment = room.Equipment,
                Price = room.Price,
            };

            // Return the partial view with the room data for editing
            return PartialView("Edit", viewModel);
        }

        [HttpGet]
        public IActionResult DeleteRoomModal(int id)
        {
            var room = _roomService.RetrieveRoom(id); // Retrieve the room by ID
            if (room == null)
            {
                return NotFound(); // Handle room not found
            }
            return PartialView("Delete", room); // Pass the room to the Delete partial view
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

        [HttpPost]
        public IActionResult Edit(RoomViewModel model)
        {
            try
            {
                // Call your service to update the room
                _roomService.UpdateRoom(model, UserId);

                // Return success response
                return Json(new { success = true, message = "Room updated successfully!" });

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

        [HttpPost]
        public IActionResult SoftDelete(int id)
        {
            try
            {
                _roomService.DeleteRoom(id); // Soft delete logic for the room
                TempData["SuccessMessage"] = "Room deleted successfully.";

                // Return a JSON response with a redirect URL
                return Json(new { success = true, redirectUrl = Url.Action("RoomManagement", "Room") });
            }
            catch (Exception)
            {
                // Handle any errors
                return Json(new { success = false, message = "An error occurred while deleting the Room." });
            }
        }
        #endregion

    }
}
