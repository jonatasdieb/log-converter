using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateTesting.JonatasDiebAraujoLima.Interfaces
{
    public interface ISaveConvertedLogs
    {
        void Save(string convertedLogs, string targetPath);
    }
}
