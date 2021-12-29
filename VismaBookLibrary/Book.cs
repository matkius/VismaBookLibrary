using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaBookLibrary
{
    public class Book
    {
        public int id { get; set; }
        public string name { get; set; }
        public string author { get; set; }
        public string category { get; set; }
        public string language { get; set; }
        public DateTime publicationDate { get; set; }
        public string isbn { get; set; }

        public string? borrower { get; set; }
        public DateTime? takenDate { get; set; }
        public DateTime? returnDate { get; set; }

        public Book() { }
        public Book(string n, string a, string cat, string lang, DateTime date, string i)
        {
            name = n;
            author = a;
            category = cat;
            language = lang;
            publicationDate = date;
            isbn = i;
        }

        public override String ToString()
        {
            return String.Format("| {0, 5} | {1,25} | {2,25} | {3,17} | {4,15} | {5,16} | {6,15} | {7,25} | {8,15} | {9,15} |",
                id, name, author, category, language, publicationDate.ToShortDateString(), isbn, borrower,
                takenDate == null ? "" : takenDate.Value.ToShortDateString(),
                returnDate == null ? "" : returnDate.Value.ToShortDateString());
        }

        public bool Equals(Book b)
        {
            return id == b.id && name == b.name && author == b.author && category == b.category && language == b.language &&
                publicationDate == b.publicationDate && isbn == b.isbn && borrower == b.borrower && takenDate == b.takenDate && returnDate == b.returnDate;
        }
    }
}
