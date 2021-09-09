using Laci.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laci.Services
{
    public class RecordService
    {
        private readonly AppDbContext _db;

        public RecordService(AppDbContext db)
        {
            _db = db;
        }

        public Record GetRecord(int cityId, DateTime date)
        {
            return _db.Records.Where(r => r.CityId == cityId && r.Date.Date == date.Date)
                .SingleOrDefault();
        }

        public List<Record> GetRecords(int cityId)
        {
            return _db.Records.Where(r => r.CityId == cityId).OrderBy(r => r.Date).ToList();
        }

        public List<Record> GetLatestRecords()
        {
            var sql = @"SELECT * FROM ""Records"" WHERE ""Date"" = (SELECT max(""Date"") FROM ""Records"")";
            return _db.Records.FromSqlRaw(sql).Include(r => r.City).ToList();
        }

        public void AddRecord(Record record) => _db.Records.Add(record);

        public void SaveChanges() => _db.SaveChanges();
    }
}
