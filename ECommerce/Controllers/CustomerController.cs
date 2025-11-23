using System.Web.Mvc;

namespace ECommerce.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Orders
        public ActionResult Orders()
        {
            return View();
        }

        // GET: Customer list (for admin)
        public ActionResult Index()
        {
            return View();
        }
    }
}
