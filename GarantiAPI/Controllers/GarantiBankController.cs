using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GarantiAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GarantiBankController : ControllerBase
    {
        [HttpGet("{musteriId}")]
        [Authorize(Policy = "ReadGaranti")]
        public double Bakiye(int musteriId)
        {
            //....
            return 1000;
        }
        [HttpGet("{musteriId}/{tutar}")]
        [Authorize(Policy = "AllGaranti")]
        public double YatirimYap(int musteriId, double tutar)
        {
            return tutar * 0.5;
        }
        [HttpGet("{musteriId}")]
        public List<string> TumHesaplar(int musteriId)
        {
            //....
            return new()
            {
                "123456789",
                "987654321",
                "564738291"
            };
        }
    }
}
