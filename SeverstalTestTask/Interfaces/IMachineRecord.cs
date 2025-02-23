using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeverstalTestTask.Interfaces
{
    interface IMachineRecord
    {
        int MachineNumber { get; set; }
        decimal GrossWeight { get; set; }
        decimal TareWeight { get; set; }
        decimal NetWeight { get; set; }
        DateTime TareDate { get; set; }
        DateTime GrossDate { get; set; }
    }
}
