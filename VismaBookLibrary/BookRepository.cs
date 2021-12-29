using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaBookLibrary
{
    public class BookRepository
    {
        private string dbFile;
        private List<Book> AllBooks = new List<Book>();
        private int currentId = 1;

        public BookRepository(List<Book> books) {
            AllBooks = books;
            if (AllBooks.Count > 0)
            {
                currentId = AllBooks.OrderByDescending(x => x.id).First().id + 1;
            }
        }
        public BookRepository(string fileName)
        {
            dbFile = "../../../" + fileName;

            AllBooks = JsonConvert.DeserializeObject<List<Book>>(File.ReadAllText(dbFile));

            if (AllBooks.Count > 0)
            {
                currentId = AllBooks.OrderByDescending(x => x.id).First().id + 1;
            }
        }

        public List<Book> getBooks(string name, string author, string category, string language, string isbn, string taken)
        {
            if (AllBooks.Count == 0)
            {
                AllBooks = JsonConvert.DeserializeObject<List<Book>>(File.ReadAllText(dbFile));
            }

            if(author != "" || category != "" || language != "" || isbn != "" || name != "" || taken != "")
            {
                var filteredBooks = AllBooks.AsEnumerable();
                if (author != "")
                    filteredBooks = filteredBooks.Where(x => x.author.ToLower().StartsWith(author));
                if (category != "")
                    filteredBooks = filteredBooks.Where(x => x.category.ToLower().StartsWith(category));
                if (language != "")
                    filteredBooks = filteredBooks.Where(x => x.language.ToLower().StartsWith(language));
                if (isbn != "")
                    filteredBooks = filteredBooks.Where(x => x.isbn.ToLower().StartsWith(isbn));
                if (name != "")
                    filteredBooks = filteredBooks.Where(x => x.name.ToLower().StartsWith(name));
                if (taken != "")
                    filteredBooks = filteredBooks.Where(x => taken == "available" ? (x.borrower == null) : (x.borrower != null));
                return filteredBooks.ToList();
            }
            return AllBooks;
        }

        public Book getBook(int id)
        {
            return AllBooks.Where(x => x.id == id).FirstOrDefault();
        }

        public virtual bool addBook(Book book)
        {
            book.id = currentId;
            currentId++;

            AllBooks.Add(book);
            if (!saveToFile())
            {
                Console.WriteLine("There has been an error while trying to save changes in json file");
                return false;
            }
            Console.WriteLine("Book added sucessfully!");
            return true;
        }

        public virtual bool deleteBook(int id)
        {
            Book book = AllBooks.Where(x => x.id == id).FirstOrDefault();
            if (book == null)
            {
                Console.WriteLine("Book with id: " + id + " cannot be found");
                return false;
            }
            AllBooks.Remove(book);
            if (!saveToFile())
            {
                Console.WriteLine("There has been an error while trying to save changes in json file");
                return false;
            }
            Console.WriteLine("Book with id: " + id + " deleted sucessfully!");
            return true;

        }

        public virtual bool takeBook(Book book, string borrowerName, DateTime returnDate)
        {
            book.borrower = borrowerName;
            book.takenDate = DateTime.Today;
            book.returnDate = returnDate;
            if (!saveToFile())
            {
                Console.WriteLine("There has been an error while trying to save changes in json file");
                return false;
            }
            Console.WriteLine("Book with id: " + book.id + " has been taken sucessfully!");
            return true;
        }

        public virtual bool returnBook(Book book)
        {
            DateTime returnDate = (DateTime)book.returnDate;
            string name = book.borrower;
            book.borrower = null;
            book.takenDate = null;
            book.returnDate = null;
            if (!saveToFile())
            {
                Console.WriteLine("There has been an error while trying to save changes in json file");
                return false;
            }
            if (returnDate < DateTime.Today)
                Console.WriteLine("Book with id: " + book.id + " was returned late. Santa is adding " + name + " to his naughty list.");
            else
                Console.WriteLine("Book with id: " + book.id + " has been returned sucessfully!");
            return true;
        }

        public int getBorrowedBooksCount(string borrowerName)
        {
            return AllBooks.Where(x => x.borrower?.ToLower() == borrowerName).Count();
        }

        public virtual bool saveToFile()
        {
            try
            {
                string json = JsonConvert.SerializeObject(AllBooks, Formatting.Indented);
                File.WriteAllText(dbFile, json);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
