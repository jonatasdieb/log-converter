using CandidateTesting.JonatasDiebAraujoLima.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateTesting.JonatasDiebAraujoLima.Services
{
    public class SaveConvertedLogs : ISaveConvertedLogs
    {
        public void Save(string convertedLogs, string targetPath)
        {
            using (StreamWriter writer = new StreamWriter(targetPath))
            {
                writer.Write(convertedLogs);
            }
        }
    }
}
