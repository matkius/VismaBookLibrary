using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaBookLibrary
{
    public class Interpreter
    {
        public BookController controller { get; set; }
        public Interpreter(BookController c)
        {
            controller = c;
        }

        public void interpret(string input)
        {
            try
            {
                string[] commandParts = input.Split('"')
                     .Select((x, index) => index % 2 == 0
                                           ? x.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                           : new string[] { x })
                     .SelectMany(element => element).ToArray();
                string command = commandParts[0].ToLower();
                string[] parts = commandParts.Skip(1).ToArray();
                switch (command)
                {
                    case "get":
                        controller.Get(parts);
                        break;
                    case "add":
                        controller.Add(parts);
                        break;
                    case "delete":
                    case "remove":
                        controller.Remove(parts);
                        break;
                    case "take":
                        controller.Take(parts);
                        break;
                    case "return":
                        controller.Return(parts);
                        break;
                    default:
                        Console.WriteLine("Error: Invalid command");
                        break;
                }
            }
            catch
            {
                Console.WriteLine("Error: Invalid command");
            }
        }
    }
}
