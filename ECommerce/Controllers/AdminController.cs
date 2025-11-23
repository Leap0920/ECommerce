using System.Web.Mvc;

namespace ECommerce.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Products()
        {
            return View();
        }

        public ActionResult Orders()
        {
            return View();
        }
        
        public ActionResult Customers()
        {
            return View();
        }

        public ActionResult Reports()
        {
            return View();
        }
    }
}
