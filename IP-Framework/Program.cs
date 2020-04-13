using System;
using System.Dynamic;
using System.Text;

namespace IP_Framework
{
    class Program
    {
        
        static void Main(string[] args)
        {
            EventHandler newHandler = new EventHandler();


            byte[] array = new byte[100];

            EventHandlerContext context = new EventHandlerContext(array, 100);

            while (true)
            {
                string command = Console.ReadLine();
                if (command == "Exit")
                    break;
                string[] arguments = command.Split(" ");
                if (arguments.Length != 3)
                    continue;
                if (Enum.IsDefined(typeof(EventHandlerFunctions), arguments[0]))
                {
                    context.command = (EventHandlerFunctions) Enum.Parse(typeof(EventHandlerFunctions), arguments[0].ToString());
                }
                if (Enum.IsDefined(typeof(SubModuleFunctions), arguments[1]))
                {
                    context.subModuleCommand = (SubModuleFunctions)Enum.Parse(typeof(SubModuleFunctions), arguments[1].ToString());
                }
                // arguments[2] should be a JSON data - the actual Context;

                Console.WriteLine(newHandler.InvokeCommand(context));
                string raspuns = Encoding.ASCII.GetString(context.contextHandler.answer);
                Console.WriteLine(raspuns);

            }

            newHandler = null;

        }
    }
}
