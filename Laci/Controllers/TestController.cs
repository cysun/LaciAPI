using Laci.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laci.Controllers
{
    [Route("test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("1")]
        [Authorize]
        public ResponseStructure Test1()
        {
            return ResponseStructure.Result(from c in User.Claims select new { c.Type, c.Value });
        }

        [HttpGet("2")]
        [Authorize(Policy = "HasApiScope")]
        public ResponseStructure Test2()
        {
            return ResponseStructure.Result(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
