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
        public Commands cmd { get; set; }
        public Interpreter(Commands c)
        {
            cmd = c;
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
                        cmd.Get(parts);
                        break;
                    case "add":
                        cmd.Add(parts);
                        break;
                    case "delete":
                    case "remove":
                        cmd.Remove(parts);
                        break;
                    case "take":
                        cmd.Take(parts);
                        break;
                    case "return":
                        cmd.Return(parts);
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
