using CandidateTesting.JonatasDiebAraujoLima.Models;
using System.Collections.Generic;

namespace CandidateTesting.JonatasDiebAraujoLima.Interfaces
{
    public interface ILogsToTargetFormatter
    {
        string Format(List<Log> logs);
        string GetFormat();
    }
}
