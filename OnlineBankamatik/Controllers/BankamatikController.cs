using AuthServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnlineBankamatik.Controllers
{
    public class BankamatikController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> OdemeYap()
        {
            var authenticationProperties = (await HttpContext.AuthenticateAsync()).Properties.Items;
            string accessToken = authenticationProperties.FirstOrDefault(x => x.Key == ".Token.access_token").Value;

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            HttpResponseMessage responseMessage = await httpClient.GetAsync("https://localhost:2000/api/garantibank/bakiye/3");
            string bakiye = await responseMessage.Content.ReadAsStringAsync();

            ViewBag.Bakiye = bakiye;
            return View();
        }
    }
}
