namespace CandidateTesting.JonatasDiebAraujoLima.Interfaces
{
    public interface ILogAdapter
    {
        string GetProvider();
        string Adapt(string sourceUrl, string targetFormat);
    }
}
