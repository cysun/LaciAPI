using Laci.Models;
using Laci.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laci.Controllers
{
    [ApiController]
    [Route("records")]
    public class RecordsController : ControllerBase
    {
        private readonly RecordService _recordService;

        public RecordsController(RecordService recordService)
        {
            _recordService = recordService;
        }

        [HttpGet("latest")]
        public ResponseStructure Latest()
        {
            return ResponseStructure.Result(_recordService.GetLatestRecords());
        }
    }
}
