using System.Web.Mvc;

namespace ECommerce.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        // GET: Checkout
        public ActionResult Checkout()
        {
            return View();
        }

        // GET: OrderConfirmation
        public ActionResult OrderConfirmation()
        {
            return View();
        }
    }
}
