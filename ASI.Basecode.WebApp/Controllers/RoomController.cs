using ASI.Basecode.Data.Models;
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
        public IActionResult RoomManagement(int pg = 1)
        {
            List<RoomViewModel> rooms = _roomService.RetrieveActiveRooms().ToList();
            var data = _roomService.RetrieveActiveRooms();

            const int pageSize = 3;
            if(pg < 1)
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
        [HttpGet]
        public IActionResult SearchRoom(string term)
        {
            // If no search term is provided, return all rooms
            if (string.IsNullOrEmpty(term))
            {
                var rooms = _roomService.RetrieveActiveRooms(); // Replace with your logic to fetch all rooms
                return Json(rooms);
            }

            // Filter rooms based on the search term
            var filteredRooms = _roomService.GetRoomsBySearchTerm(term); // Your service method to search rooms

            return Json(filteredRooms);
        }

        #endregion

        #region Post Methods
        [HttpPost]
        public IActionResult Create(RoomViewModel model)
        {
            try
            {
                _roomService.AddRoom(model, UserId);
                TempData["SuccessMessage"] = "Room created successfully.";
                return Json(new { success = true});
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
                TempData["SuccessMessage"] = "Room updated successfully.";
                // Return success response
                return Json(new { success = true});

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
