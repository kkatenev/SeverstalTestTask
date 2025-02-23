using SeverstalTestTask.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeverstalTestTask.Other
{
    public class MachineRecord : IMachineRecord
    {
        public int MachineNumber { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal TareWeight { get; set; }
        public decimal NetWeight { get; set; }
        public DateTime TareDate { get; set; }
        public DateTime GrossDate { get; set; }

        public MachineRecord(int machineNumber, decimal grossWeight, decimal tareWeight, decimal netWeight, DateTime tareDate, DateTime grossDate)
        {
            MachineNumber = machineNumber;
            GrossWeight = grossWeight;
            TareWeight = tareWeight;
            NetWeight = netWeight;
            TareDate = tareDate;
            GrossDate = grossDate;
        }

        public static decimal CalculateNetWeight(decimal grossWeight, decimal tareWeight)
        {
            return grossWeight - Math.Min(grossWeight, tareWeight);
        }
    }
}
