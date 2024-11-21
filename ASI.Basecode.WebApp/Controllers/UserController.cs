using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Authentication;
using ASI.Basecode.WebApp.Models;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using System.Xml.Schema;
using static ASI.Basecode.Resources.Constants.Enums;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace ASI.Basecode.WebApp.Controllers
{
    public class UserController : ControllerBase<UserController>
    {
        private readonly IUserService _userService;
        /// <param name="localizer">The localizer.</param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="mapper"></param>
        public UserController(
                            IUserService userService,
                            IHttpContextAccessor httpContextAccessor,
                            ILoggerFactory loggerFactory,
                            IConfiguration configuration,
                            IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)


        {
            this._userService = userService;
        }

        /*public IActionResult Index()
        {
            var data = _userService.RetrieveAll();
            return View(data);
        }*/
       

        #region Get Methods


        [HttpGet]
        public IActionResult UserManagement(int pg = 1)
        {
            List<UserViewModel> users = _userService.RetrieveActiveNonAdminUsers().ToList();
            var data = _userService.RetrieveActiveNonAdminUsers();

            const int pageSize = 3;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = users.Count;

            var pager = new Pager(recsCount, pg, pageSize);
            int recsSkip = (pg - 1) * pageSize;

            var data1 = users.Skip(recsSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;


            return View(data1);
            
        }

        [HttpGet]
        public IActionResult CreateUserModal()
        {
            return PartialView("Create");
        }

        [HttpGet]
        public IActionResult EditUserModal(int id)
        {
            // Retrieve the user based on the ID
            var user = _userService.RetrieveUser(id); // Ensure this method correctly retrieves the user
            if (user == null)
            {
                return NotFound(); 
            }

            // Create the UserViewModel to pass the data to the modal view
            var viewModel = new UserViewModel
            {
                Id = user.Id,
                UserId = user.UserId,
                Name = user.Name,
                Roles = user.Roles,
            };

            // Return the partial view with the user data for editing
            return PartialView("Edit", viewModel); 
        }


        [HttpGet]
        public IActionResult DeleteUserModal(int id)
        {
            var user = _userService.RetrieveUser(id); // Retrieve the user by ID
            if (user == null)
            {
                return NotFound(); // Handle user not found
            }
            return PartialView("Delete", user); // Pass the user to the Delete partial view
        }

        [HttpGet]
        public IActionResult SearchUser(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                var users = _userService.RetrieveActiveNonAdminUsers();
                return Json(users);
            }

            var filteredUsers = _userService.GetUsersBySearchTerm(term);
            return Json(filteredUsers);
        }

        #endregion

        #region Post Methods
        [HttpPost]
        public IActionResult Create(UserViewModel model)
        {
            try
            {
                _userService.AddUser(model);
                TempData["SuccessMessage"] = "User created successfully.";
                return Json(new { success = true});
            }
            catch (InvalidDataException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = Resources.Messages.Errors.ServerError });
            }
        }

        [HttpPost]
        public IActionResult Edit(UserViewModel model)
        {
            try
            {

                // Call your service to update the user
                _userService.UpdateUser(model);
                TempData["SuccessMessage"] = "User updated successfully.";
                // Return success response
                return Json(new { success = true});
               
            }
            catch (InvalidDataException ex)
            {
              
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
                _userService.DeleteUser(id); 
                TempData["SuccessMessage"] = "User deleted successfully.";

                return Json(new { success = true, redirectUrl = Url.Action("UserManagement", "User") });
            }
            catch (Exception)
            {
                // Handle any errors
                return Json(new { success = false, message = "An error occurred while deleting the user." });
            }
        }
        #endregion



    }
}