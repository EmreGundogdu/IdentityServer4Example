using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineBankamatik.Controllers
{
    public class BankamatikController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult OdemeYap()
        {
            return View();
        }
    }
}
