using Laci.Models;
using Laci.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Laci.Controllers
{
    [ApiController]
    [Route("records")]
    public class RecordsController : ControllerBase
    {
        private readonly CityService _cityService;
        private readonly RecordService _recordService;

        private readonly ILogger<RecordsController> _logger;

        private readonly Dictionary<string, string> _columnMappings = new Dictionary<string, string>{
            { "\"geo_merge\"", "City" },
            { "\"population\"", "Population" },
            { "\"persons_tested_final\"", "Tests" },
            { "\"cases_final\"", "Cases" },
            { "\"deaths_final\"", "Deaths" }
        };

        public RecordsController(CityService cityService, RecordService recordService, ILogger<RecordsController> logger)
        {
            _cityService = cityService;
            _recordService = recordService;
            _logger = logger;
        }

        [HttpGet("latest")]
        public ResponseStructure Latest()
        {
            return ResponseStructure.Result(_recordService.GetLatestRecords());
        }

        [HttpPost("{year}-{month}-{day}")]
        public ResponseStructure Import(int year, int month, int day, List<IFormFile> files)
        {
            var date = new DateTime(year, month, day);
            foreach (var file in files)
                processFile(date, file);

            return ResponseStructure.Result();
        }

        private void processFile(DateTime date, IFormFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream());

            string line = reader.ReadLine();
            string[] words = line.ToLower().Split(',');
            Dictionary<string, int> columnIndexes = new Dictionary<string, int>();
            for (var i = 0; i < words.Length; ++i)
                if (_columnMappings.ContainsKey(words[i]))
                    columnIndexes.Add(_columnMappings[words[i]], i);

            if (!columnIndexes.ContainsKey("City")) {
                _logger.LogWarning("No city column found in the uploaded file");
                return;
            }

            while ((line = reader.ReadLine()) != null) {
                if (string.IsNullOrEmpty(line)) continue;

                words = line.Split(',');
                var name = words[columnIndexes["City"]];
                name = name.Substring(1, name.Length - 2); // strip the quotes
                var city = _cityService.GetCity(name);
                if (city == null) {
                    city = new City { Name = name };
                    _cityService.AddCity(city);
                    _cityService.SaveChanges();
                }

                if (columnIndexes.ContainsKey("Population")) {
                    int population;
                    if (int.TryParse(words[columnIndexes["Population"]], out population))
                        city.Population = population;
                }

                var record = _recordService.GetRecord(city.Id, date);
                if (record == null) {
                    record = new Record {
                        CityId = city.Id,
                        Date = date
                    };
                    _recordService.AddRecord(record);
                }

                if (columnIndexes.ContainsKey("Tests"))
                    record.Tests = int.Parse(words[columnIndexes["Tests"]]);
                if (columnIndexes.ContainsKey("Cases"))
                    record.Cases = int.Parse(words[columnIndexes["Cases"]]);
                if (columnIndexes.ContainsKey("Deaths"))
                    record.Deaths = int.Parse(words[columnIndexes["Deaths"]]);
            }
            _cityService.SaveChanges();
            _recordService.SaveChanges();
        }
    }
}
