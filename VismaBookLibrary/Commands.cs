using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VismaBookLibrary
{
    public class Commands
    {
        public BookController bookController { get; set; }
        public Commands(BookController bc)
        {
            bookController = bc;
        }

        public bool Get(string[] parts)
        {
            string filterName = "";
            string filterAuthor = "";
            string filterCategory = "";
            string filterLanguage = "";
            string filterIsbn = "";
            string filterTaken = "";
            if(parts.Length % 2 != 0)
            {
                Console.WriteLine("Error: There is a mistake in writing parameters");
                return false;
            }
            parts = parts.Select(x => x.ToLower()).ToArray();
            for (int i = 0; i < parts.Length; i=i+2)
            {
                switch (parts[i])
                {
                    case "-n":
                    case "-name":
                        filterName = parts[i+1];
                        break;
                    case "-a":
                    case "-auth":
                    case "-author":
                        filterAuthor = parts[i+1];
                        break;
                    case "-c":
                    case "-cat":
                    case "-category":
                        filterCategory = parts[i+1];
                        break;
                    case "-l":
                    case "-lang":
                    case "-language":
                        filterLanguage = parts[i+1];
                        break;
                    case "-i":
                    case "-isbn":
                        filterIsbn = parts[i+1];
                        break;
                    case "-t":
                    case "-taken":
                        switch (parts[i+1])
                        {
                            case "t":
                            case "true":
                            case "taken":
                                filterTaken = "taken";
                                break;
                            case "f":
                            case "false":
                            case "a":
                            case "available":
                                filterTaken = "available";
                                break;
                            default:
                                Console.WriteLine("Error: unrecognized option for -taken: " + parts[i + 1]);
                                return false;
                        }
                        break;
                    default:
                        Console.WriteLine("Error: invalid parameter: " + parts[i]);
                        return false;
                }
            }
            List<Book> books = bookController.getBooks(filterName, filterAuthor, filterCategory, filterLanguage, filterIsbn, filterTaken);
            if (books.Count == 0)
                Console.WriteLine("List is empty");
            else
            {
                Console.WriteLine(new String('-', 200));
                Console.WriteLine("| {0, 5} | {1,25} | {2,25} | {3,17} | {4,15} | {5,16} | {6,15} | {7,25} | {8,15} | {9,15} |",
                    "Id", "Name", "Author", "Category", "Language", "Publication Date", "ISBN", "Borrower", "Takeout Date", "Return Date");
                Console.WriteLine(new String('-', 200));
                foreach (var book in books)
                {
                    Console.WriteLine(book.ToString());
                }
            }
            return true;
        }

        public bool Add(string[] parts)
        {
            if(parts.Length % 2 != 0)
            {
                Console.WriteLine("Error: incorrect parameter count");
                return false;
            }

            string nameInput = "";
            string authorinput = "";
            string categoryInput = "";
            string languageInput = "";
            DateTime publicationDate = DateTime.MinValue;
            string isbnInput = "";

            for (int i = 0; i < parts.Length; i = i + 2)
            {
                switch (parts[i])
                {
                    case "-n":
                    case "-name":
                        nameInput = parts[i + 1];
                        if (nameInput == "")
                        {
                            Console.WriteLine("Error: Name cannot be empty");
                            return false;
                        }
                        break;
                    case "-a":
                    case "-auth":
                    case "-author":
                        authorinput = parts[i + 1];
                        if (authorinput == "")
                        {
                            Console.WriteLine("Error: Author cannot be empty");
                            return false;
                        }
                        break;
                    case "-c":
                    case "-cat":
                    case "-category":
                        categoryInput = parts[i + 1];
                        switch (categoryInput.ToLower())
                        {
                            case "action":
                            case "adventure":
                                categoryInput = "Action/Adventure";
                                break;
                            case "fantasy":
                                categoryInput = "Fantasy";
                                break;
                            case "history":
                            case "historical":
                                categoryInput = "History";
                                break;
                            case "detective":
                            case "mystery":
                            case "crime":
                                categoryInput = "Mystery/Crime";
                                break;
                            case "horror":
                                categoryInput = "Horror";
                                break;
                            case "romance":
                            case "love":
                                categoryInput = "Romance";
                                break;
                            case "science fiction":
                            case "sci-fi":
                                categoryInput = "Science Fiction";
                                break;
                            case "biography":
                            case "autobiography":
                            case "biographical":
                            case "autobiographical":
                                categoryInput = "Biography";
                                break;
                            case "novel":
                                categoryInput = "Novel";
                                break;
                            default:
                                Console.WriteLine("Error: Category not found");
                                return false;
                        }
                        break;
                    case "-l":
                    case "-lang":
                    case "-language":
                        languageInput = parts[i + 1];
                        switch (languageInput.ToLower())
                        {
                            case "lt":
                            case "lit":
                            case "lithuanian":
                                languageInput = "Lithuanian";
                                break;
                            case "en":
                            case "eng":
                            case "english":
                                languageInput = "English";
                                break;
                            case "rus":
                            case "russian":
                                languageInput = "Russian";
                                break;
                            case "de":
                            case "deu":
                            case "german":
                                languageInput = "German";
                                break;
                            case "fr":
                            case "french":
                                languageInput = "French";
                                break;
                            default:
                                Console.WriteLine("Error: Language not found");
                                return false;
                        }
                        break;
                    case "-p":
                    case "-pub":
                    case "-date":
                    case "-pubdate":
                    case "-publicationdate":
                        string dateInput = parts[i + 1];
                        if (dateInput == "")
                        {
                            Console.WriteLine("Error: Date cannot be empty");
                            return false;
                        }
                        try
                        {
                            publicationDate = DateTime.Parse(dateInput);
                        }
                        catch
                        {
                            Console.WriteLine("Error: Incorrect date format (yyyy-MM-dd)");
                            return false;
                        }
                        break;
                    case "-i":
                    case "-isbn":
                        isbnInput = parts[i + 1];
                        if (isbnInput == "")
                        {
                            Console.WriteLine("Error: ISBN cannot be empty");
                            return false;
                        }
                        else if (!Regex.IsMatch(isbnInput, @"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$"))
                        {
                            Console.WriteLine("Error: incorrect ISBN format");
                            return false;
                        }

                        break;

                    default:
                        Console.WriteLine("Error: invalid parameter: " + parts[i]);
                        return false;
                }
            }
            bool paramsMissing = false;
            if(nameInput == "")
            {
                Console.WriteLine("Error: Name parameter missing");
                paramsMissing = true;
            }
            if (authorinput == "")
            {
                Console.WriteLine("Error: Author parameter missing");
                paramsMissing = true;
            }
            if (categoryInput == "")
            {
                Console.WriteLine("Error: Category parameter missing");
                paramsMissing = true;
            }
            if (languageInput == "")
            {
                Console.WriteLine("Error: Language parameter missing");
                paramsMissing = true;
            }
            if (publicationDate == DateTime.MinValue)
            {
                Console.WriteLine("Error: Publication date parameter missing");
                paramsMissing = true;
            }
            if (isbnInput == "")
            {
                Console.WriteLine("Error: ISBN parameter missing");
                paramsMissing = true;
            }
            if (paramsMissing)
                return false;
            return bookController.addBook(new Book(nameInput, authorinput, categoryInput, languageInput, publicationDate, isbnInput));
        }

        public bool Remove(string[] parts)
        {
            if (parts.Length != 1)
            {
                Console.WriteLine("Error: Incorrect parameter count. This command accepts only one id");
                return false;
            }

            int id;
            try
            {
                id = int.Parse(parts[0]);
            }
            catch
            {
                Console.WriteLine("Error: Id should be a number");
                return false;
            }
            return bookController.deleteBook(id);
        }

        public bool Take(string[] parts)
        {
            int id;
            try
            {
                id = int.Parse(parts[0]);
            }
            catch
            {
                Console.WriteLine("Error: Id should be a number");
                return false;
            }
            Book selectedBook = bookController.getBook(id);
            if (selectedBook == null)
            {
                Console.WriteLine("Error: Book with id: " + id + " could not be found");
                return false;
            }
            else if (selectedBook.borrower != null)
            {
                Console.WriteLine("Error: Book with id: " + id + " has already been taken");
                return false;
            }

            if (parts.Length % 2 == 0)
            {
                Console.WriteLine("Error: incorrect parameter count");
                return false;
            }

            string borrowerNameInput = "";
            DateTime returnDate = DateTime.MinValue;


            for (int i = 1; i < parts.Length; i = i + 2)
            {
                switch (parts[i])
                {
                    case "-n":
                    case "-name":
                    case "-b":
                    case "-borrower":
                    case "-borrowername":
                        borrowerNameInput = parts[i + 1];
                        if (borrowerNameInput.ToLower() == "")
                        {
                            Console.WriteLine("Error: Name cannot be empty");
                            return false;
                        }    
                        else if (bookController.getBorrowedBooksCount(borrowerNameInput.ToLower()) >= 3)
                        {
                            Console.WriteLine("Error: Declared person has already borrowed 3 or more books");
                            return false;
                        }
                        break;
                    case "-d":
                    case "-date":
                    case "-rd":
                    case "-returndate":
                        string returnDateInput = parts[i + 1];
                        if (returnDateInput.ToLower() == "")
                        {
                            Console.WriteLine("Error: Return date cannot be empty");
                            return false;
                        }
                        try
                        {
                            returnDate = DateTime.Parse(returnDateInput);
                        }
                        catch
                        {
                            Console.WriteLine("Error: Incorrect date format (yyyy-MM-dd)");
                            return false;
                        }
                        if (returnDate < DateTime.Today)
                        {
                            Console.WriteLine("Error: Return date cannot be earlier than today's date");
                            return false;
                        }
                        else if (returnDate > DateTime.Today.AddMonths(2))
                        {
                            Console.WriteLine("Error: Book cannot be borrowed for longer than 2 months");
                            return false;
                        }
                        break;
                }
            }
            bool paramsMissing = false;
            if (borrowerNameInput == "")
            {
                Console.WriteLine("Error: Borrower name parameter missing");
                paramsMissing = true;
            }
            if (returnDate == DateTime.MinValue)
            {
                Console.WriteLine("Error: Return date parameter missing");
                paramsMissing = true;
            }
            if (paramsMissing)
                return false;
            return bookController.takeBook(selectedBook, borrowerNameInput, returnDate);
        }

        public bool Return(string[] parts)
        {

            if (parts.Length != 1)
            {
                Console.WriteLine("Error: Incorrect parameter count. This command accepts only one id");
                return false;
            }

            int id;
            try
            {
                id = int.Parse(parts[0]);
            }
            catch
            {
                Console.WriteLine("Error: Id should be a number");
                return false;
            }

            Book selectedBook = bookController.getBook(id);
            if (selectedBook == null)
            {
                Console.WriteLine("Error: Book with id: " + id + " could not be found");
                return false;
            }
            else if (selectedBook.borrower == null)
            {
                Console.WriteLine("Error: Book with id: " + id + " has not been taken by anybody");
                return false;
            }

            return bookController.returnBook(selectedBook);
        }
    }
}
