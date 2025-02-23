using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeverstalTestTask.Interfaces
{
    interface IDatabaseService
    {
        Task InitializeAsync();
        string GetDatabasePath();
    }
}
