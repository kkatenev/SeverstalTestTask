using Microsoft.EntityFrameworkCore;
using SeverstalTestTask.Db;
using SeverstalTestTask.Interfaces;
using SeverstalTestTask.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeverstalTestTask.Models
{
    public class SeverstalTestTaskModel
    {
        private readonly MachineDbContext _context;

        public SeverstalTestTaskModel(MachineDbContext context)
        {
            _context = context;
        }

        public async Task AddRecordAsync(MachineRecord record)
        {
            var entity = new MachineRecordEntity
            {
                MachineNumber = record.MachineNumber,
                GrossWeight = record.GrossWeight,
                TareWeight = record.TareWeight,
                NetWeight = record.NetWeight,
                TareDate = record.TareDate,
                GrossDate = record.GrossDate
            };

            await _context.MachineRecords.AddAsync(entity);
        }

        public async Task<List<MachineRecord>> GetRecordsAsync()
        {
            var records = await _context.MachineRecords
                .Select(r => new MachineRecord(
                    r.MachineNumber,
                    r.GrossWeight,
                    r.TareWeight,
                    r.NetWeight,
                    r.TareDate,
                    r.GrossDate))
                .ToListAsync();

            return records;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task GenerateRecordsAsync(int numberOfRecords)
        {
            Random random = new Random();
            DateTime startDate = DateTime.Now;

            List<int> machineNumbers = new List<int>();
            for (int j = 0; j < 20; j++)
            {
                machineNumbers.Add(random.Next(1, 100));
            }

            for (int i = 0; i < numberOfRecords; i++)
            {
                try
                {
                    int grossWeight = random.Next(100, 5001);
                    int tareWeight = random.Next(1, grossWeight);
                    int selectedMachineNumber = machineNumbers[random.Next(machineNumbers.Count)];

                    var newRecord = new MachineRecord(
                        machineNumber: selectedMachineNumber,
                        grossWeight: grossWeight,
                        tareWeight: tareWeight,
                        netWeight: MachineRecord.CalculateNetWeight(grossWeight, tareWeight),
                        tareDate: startDate.AddMinutes(i),
                        grossDate: startDate.AddMinutes(i));

                    await AddRecordAsync(newRecord);
                    await SaveChangesAsync();


                    LogManager.Instance.AddEvent($"Record {i + 1} was added successfully: MachineNumber={selectedMachineNumber}, GrossWeight={grossWeight}, TareWeight={tareWeight}");
                }
                catch (Exception ex)
                {
                    LogManager.Instance.AddError($"Error adding record: {ex.Message}");
                }
            }
        }
    }
}
