using SeverstalTestTask.Db;
using SeverstalTestTask.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeverstalTestTask.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly MachineDbContext _context;

        public DatabaseService(MachineDbContext context)
        {
            _context = context;
        }

        public string GetDatabasePath()
        {
            return Path.Combine(
                Directory.GetCurrentDirectory(),
                "Db",
                "machinedata.db"
            );
        }

        public async Task InitializeAsync()
        {
            var dbPath = GetDatabasePath();
            var dbExists = File.Exists(dbPath);
            if (dbExists == false)
            {
                await _context.Database.EnsureCreatedAsync();
                await SeedInitialDataAsync();
            }
        }

        private async Task SeedInitialDataAsync()
        {
            var testRecord = new MachineRecordEntity
            {
                MachineNumber = 1,
                GrossWeight = 1000,
                TareWeight = 200,
                NetWeight = 800,
                TareDate = DateTime.Now,
                GrossDate = DateTime.Now
            };

            await _context.MachineRecords.AddAsync(testRecord);
            await _context.SaveChangesAsync();
        }
    }
}