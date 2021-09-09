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
    [Route("cities")]
    public class CitiesController : ControllerBase
    {
        private readonly CityService _cityService;
        private readonly RecordService _recordService;

        public CitiesController(CityService cityService, RecordService recordService)
        {
            _cityService = cityService;
            _recordService = recordService;
        }

        [HttpGet]
        public ResponseStructure Cites()
        {
            return ResponseStructure.Result(_cityService.GetCities());
        }

        [HttpGet("{cityId}/records")]
        public ResponseStructure CityRecords(int cityId)
        {
            return ResponseStructure.Result(_recordService.GetRecords(cityId));
        }
    }
}
