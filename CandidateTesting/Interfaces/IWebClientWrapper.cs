using System.IO;

namespace CandidateTesting.JonatasDiebAraujoLima.Interfaces
{
    public interface IWebClientWrapper
    {
        Stream OpenRead(string url);
    }
}
