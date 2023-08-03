namespace Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ConsoleService : IConsoleService
    {
        public ConsoleService()
        {

        }

        public void PrintLineDecoration()
        {
            // Console.WriteLine()
        }

        public void WriteToConsole(string textString)
        {
            Console.WriteLine(textString);
        }

        public string GetUserInput()
        {
            return Console.ReadLine();
        }
    }
}
