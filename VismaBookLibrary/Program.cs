using System;

namespace VismaBookLibrary
{
    class Program
    {
        public static string fileName = "books.json";
        static void Main(string[] args)
        {
            Interpreter interpreter = new Interpreter(new Commands(new BookController(fileName)));
            Console.WriteLine("Welcome to Visma book library");
            Console.WriteLine("Available commands:");
            Console.WriteLine("get - gets all books. filter params: -n name, -a author, -c category, -l language, -i isbn, -t isTaken (true/false)");
            Console.WriteLine("add - adds a book. required params: -n name, -a author, -c category, -l language, -p publication date, -i isbn");
            Console.WriteLine("remove - deletes a book. Declare one id after command");
            Console.WriteLine("take - takes a book. required params: id, -n borrower's name, -d return date. Example: take 1 -n \"john smith\" -d 2022-02-01");
            Console.WriteLine("return - deletes a book. Declare one id after command");
            while (true)
            {
                Console.Write("> ");
                string command = Console.ReadLine();
                interpreter.interpret(command);
            }
            
        }
    }
}
