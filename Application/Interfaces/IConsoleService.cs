namespace Application.Interfaces
{
    public interface IConsoleService
    {
        string GetUserInput();
        void WriteToConsole(string textString);
    }
}