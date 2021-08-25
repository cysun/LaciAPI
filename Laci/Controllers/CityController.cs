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
    public class CityController : ControllerBase
    {
        private readonly CityService _cityService;
        private readonly RecordService _recordService;

        public CityController(CityService cityService, RecordService recordService)
        {
            _cityService = cityService;
            _recordService = recordService;
        }

        [HttpGet]
        public List<City> Cites()
        {
            return _cityService.GetCities();
        }

        [HttpGet("{cityId}/records")]
        public List<Record> CityRecords(int cityId)
        {
            return _recordService.GetRecords(cityId);
        }
    }
}
