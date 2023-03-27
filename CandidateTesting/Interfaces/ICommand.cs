namespace CandidateTesting.JonatasDiebAraujoLima.Interfaces
{
    public interface ICommand
    {
        string GetContext();
        void Execute(string[] args);
    }
}
