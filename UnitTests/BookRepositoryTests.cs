using System;
using Xunit;
using Moq;
using VismaBookLibrary;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;

namespace UnitTests
{
    public class BookRepositoryTests
    {
        [InlineData("", "", "", "", "", "")]
        [InlineData("t", "", "", "", "", "")]
        [InlineData("", "", "", "", "", "a")]
        [InlineData("", "test", "Science-Fiction", "English", "", "")]
        [Theory]
        public void bookRepository_getBooksTest(string name, string author, string category, string language, string isbn, string taken)
        {
            var bookRepo = new BookRepository(getTestBooks());
            var testBooks = getTestBooks().AsEnumerable();
            if (author != "")
                testBooks = testBooks.Where(x => x.author.ToLower().StartsWith(author));
            if (category != "")
                testBooks = testBooks.Where(x => x.category.ToLower().StartsWith(category));
            if (language != "")
                testBooks = testBooks.Where(x => x.language.ToLower().StartsWith(language));
            if (isbn != "")
                testBooks = testBooks.Where(x => x.isbn.ToLower().StartsWith(isbn));
            if (name != "")
                testBooks = testBooks.Where(x => x.name.ToLower().StartsWith(name));
            if (taken != "")
                testBooks = testBooks.Where(x => taken == "available" ? (x.borrower == null) : (x.borrower != null));
            var result = testBooks.ToList();
            List<Book> BooksResult = bookRepo.getBooks(name, author, category, language, isbn, taken);
            for (int i = 0; i < BooksResult.Count; i++)
            {
                Assert.True(BooksResult[i].Equals(result[i]));
            }
        }

        [Fact]
        public void bookRepository_addBooksTest()
        {
            var bookRepoMock = new Mock<BookRepository>(getTestBooks());
            bookRepoMock.Setup(x => x.saveToFile()).Returns(true);
            Book b = new Book("newBook", "newAuthor", "newCategory", "newLang", DateTime.MinValue, "123");
            bookRepoMock.Object.addBook(b);
            Assert.True(bookRepoMock.Object.getBooks("", "", "", "", "", "").Count == getTestBooks().Count + 1);
        }

        [InlineData(true, 1)]
        [InlineData(false, 100)]
        [Theory]
        public void bookRepository_removeBooksTest(bool expectedResult, int id)
        {
            var bookRepoMock = new Mock<BookRepository>(getTestBooks());
            bookRepoMock.Setup(x => x.saveToFile()).Returns(true);
            bookRepoMock.Object.deleteBook(id);
            List<Book> BooksResult = bookRepoMock.Object.getBooks("", "", "", "", "", "");
            Assert.Equal(expectedResult, BooksResult.Count == getTestBooks().Count - 1);
            Assert.Equal(expectedResult, BooksResult.Where(x => x.id == 1).FirstOrDefault() == null);
        }

        [Fact]
        public void bookRepository_takeBooksTest()
        {
            var bookRepoMock = new Mock<BookRepository>(getTestBooks());
            bookRepoMock.Setup(x => x.saveToFile()).Returns(true);
            List<Book> BooksResult = bookRepoMock.Object.getBooks("", "", "", "", "", "");
            bookRepoMock.Object.takeBook(BooksResult[0], "billy bob john", new DateTime(2022,1,1));
            Assert.True(BooksResult.Where(x => x.id == 1).First().borrower != null);
        }

        [Fact]
        public void bookRepository_returnBooksTest()
        {
            var bookRepoMock = new Mock<BookRepository>(getTestBooks());
            bookRepoMock.Setup(x => x.saveToFile()).Returns(true);
            List<Book> BooksResult = bookRepoMock.Object.getBooks("", "", "", "", "", "");
            bookRepoMock.Object.returnBook(BooksResult[0]);
            Assert.True(BooksResult.Where(x => x.id == 1).First().borrower == null);
        }


        public List<Book> getTestBooks()
        {
            return new List<Book>()
            {
                new Book(){
                    id = 1, name = "testbook1", author = "test author", category = "Fantasy", language = "English", isbn = "1234567890",
                    publicationDate = new DateTime(2000,1,1), borrower = null, takenDate = new DateTime(2020,1,1), returnDate = new DateTime(2020,2,1)
                },
                new Book(){
                    id = 2, name = "testbook2", author = "tesst author", category = "History", language = "German", isbn = "9876543210",
                    publicationDate = new DateTime(1999,1,1), borrower = "Bob Smith", takenDate = new DateTime(2021,1,1), returnDate = new DateTime(2021,2,1)
                },
                new Book(){
                    id = 3, name = "testbook3", author = "tessst author", category = "Horror", language = "Russian", isbn = "9638527410",
                    publicationDate = new DateTime(1998,1,1), borrower = null, takenDate = new DateTime(2019,1,1), returnDate = new DateTime(2022,5,1)
                },
                new Book(){
                    id = 4, name = "testbook1", author = "test author", category = "Science-Fiction", language = "English", isbn = "1234567890",
                    publicationDate = new DateTime(2000,1,1), borrower = "John Smith", takenDate = new DateTime(2020,1,1), returnDate = new DateTime(2020,2,1)
                },
                new Book(){
                    id = 5, name = "testbook2", author = "tesst author", category = "History", language = "German", isbn = "9876543210",
                    publicationDate = new DateTime(1999,1,1), borrower = null, takenDate = new DateTime(2021,1,1), returnDate = new DateTime(2021,2,1)
                },
                new Book(){
                    id = 6, name = "testbook3", author = "tessst author", category = "Horror", language = "Russian", isbn = "9638527410",
                    publicationDate = new DateTime(1998,1,1), borrower = "Tom Smith", takenDate = new DateTime(2019,1,1), returnDate = new DateTime(2022,5,1)
                }
            };
        }
    }
}
