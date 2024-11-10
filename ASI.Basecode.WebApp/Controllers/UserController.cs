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
            HttpContext.Session.SetString("IsUserManagementActive", "true");
            HttpContext.Session.Remove("IsRoomManagementActive");
            HttpContext.Session.Remove("IsViewBookingActive");
            var data = _userService.RetrieveActiveNonAdminUsers();
            return View(data);
        }
        public IActionResult Create()
        {
            HttpContext.Session.Remove("IsUserManagementActive");
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var data = _userService.RetrieveUser(Id);
            data.Password = string.Empty;
            return View(data);
        }

        [HttpGet]
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
        public IActionResult Edit(UserViewModel model)
        {
            _userService.UpdateUser(model);
            return RedirectToAction("UserManagement");
        }

        [HttpPost]
        public IActionResult SoftDelete(int Id)
        {
            _userService.DeleteUser(Id);
            return RedirectToAction("UserManagement");
        }
        #endregion



    }
}
