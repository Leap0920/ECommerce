using System.Web.Mvc;

namespace ECommerce.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer Dashboard (redirects to Dashboard view)
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        // GET: Customer Dashboard
        public ActionResult Dashboard()
        {
            return View();
        }

        // GET: Customer Orders
        public ActionResult Orders()
        {
            return View();
        }
    }
}
