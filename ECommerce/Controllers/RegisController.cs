using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Controllers
{
    public class RegisController : Controller
    {
        // GET: Regis
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult SignIn()
        {
            return View();
        }
    }
}