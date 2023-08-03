namespace Application.Services
{
    public interface IConsoleService
    {
        string GetUserInput();
        void WriteToConsole(string textString);
    }
}