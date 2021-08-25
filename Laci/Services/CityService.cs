using Laci.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laci.Services
{
    public class CityService
    {
        private readonly AppDbContext _db;

        public CityService(AppDbContext db)
        {
            _db = db;
        }

        public List<City> GetCities()
        {
            return _db.Cities.OrderBy(c => c.Name).ToList();
        }

        public City GetCity(int id)
        {
            return _db.Cities.Find(id);
        }

        public City GetCity(string name)
        {
            return _db.Cities.Where(c => c.Name.ToUpper() == name.ToUpper()).SingleOrDefault();
        }

        public void AddCity(City city) => _db.Cities.Add(city);

        public void SaveChanges() => _db.SaveChanges();
    }
}
