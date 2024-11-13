using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
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
using System.Threading.Tasks;
using static ASI.Basecode.Resources.Constants.Enums;

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
        public IActionResult UserManagement()
        {
            var data = _userService.RetrieveActiveNonAdminUsers();
            return View(data);
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


        [HttpGet("/User/Delete/{Id}")]
        public IActionResult Delete(int Id)
        {
            var data = _userService.RetrieveUser(Id);
            return View(data);
        }

        #endregion

        #region Post Methods
        [HttpPost]
        public IActionResult Create(UserViewModel model)
        {
            try
            {
                _userService.AddUser(model);
                TempData["SuccessMessage"] = "User created successfully!";
                return Json(new { success = true, message = "User created successfully!" });
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

                // Return success response
                return Json(new { success = true, message = "User updated successfully!" });
               
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

        [HttpPost("/User/Delete/{Id}")]
        public IActionResult SoftDelete(int Id)
        {
            _userService.DeleteUser(Id);
            return RedirectToAction("UserManagement");
        }
        #endregion



    }
}
