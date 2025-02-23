using Microsoft.EntityFrameworkCore;
using SeverstalTestTask.Other;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeverstalTestTask.Db
{
    public class MachineDbContext : DbContext
    {
        public DbSet<MachineRecordEntity> MachineRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Db",
                "machinedata.db"
            );

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
