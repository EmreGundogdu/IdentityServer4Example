﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HalkBankAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class HalkBankController : ControllerBase
    {
        [HttpGet("{musteriID}")]
        [Authorize(Policy = "ReadWriteHalkBank")]
        public double Bakiye(int musteriId)
        {
            //....
            return 500.15;
        }
        [HttpGet("{musteriID}")]
        public List<string> TumHesaplar(int musteriId)
        {
            //....
            return new()
            {
                "135792468",
                "019283745",
                "085261060"
            };
        }
    }
}
