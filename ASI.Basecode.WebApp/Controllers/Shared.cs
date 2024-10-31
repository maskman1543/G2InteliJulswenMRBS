using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASI.Basecode.WebApp.Controllers
{
    public class Shared : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Dashboard()
        {
            return this.View();
        }
    }
}
