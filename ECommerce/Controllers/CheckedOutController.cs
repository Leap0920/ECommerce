using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Controllers
{
    public class CheckedOutController : Controller
    {
        // GET: CheckedOut
        public ActionResult Checked()
        {
            return View();
        }
    }
}