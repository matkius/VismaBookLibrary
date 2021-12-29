using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaBookLibrary;
using Xunit;

namespace UnitTests
{
    public class BookControllerTests
    {
        [InlineData(true, new string[0])]
        [InlineData(false, new string[] { "-n" })]
        [InlineData(true, new string[] { "-n", "test" })]
        [InlineData(true, new string[] { "-n", "test", "-cat", "sci" })]
        [InlineData(false, new string[] { "-t", "aaaaa" })]
        [Theory]
        public void BookController_GetTest(bool expectedResult, string[] parts)
        {
            var bookRepoMock = new Mock<BookRepository>(getTestBooks());
            BookController controller = new BookController(bookRepoMock.Object);
            Assert.Equal(controller.Get(parts), expectedResult);
        }

        [InlineData(false, new string[0])]
        [InlineData(false, new string[] { "-n", "", "-a", "", "-c", "", "-l", "", "-p", "", "-i", "" })]
        [InlineData(false, new string[] { "-n", "testName", "-a", "", "-c", "", "-l", "", "-p", "", "-i", "" })]
        [InlineData(false, new string[] { "-n", "testName", "-a", "testAuthor", "-c", "category", "-l", "", "-p", "", "-i", "" })]
        [InlineData(false, new string[] { "-n", "testName", "-a", "testAuthor", "-c", "novel", "-l", "lv", "-p", "", "-i", "" })]
        [InlineData(false, new string[] { "-n", "testName", "-a", "testAuthor", "-c", "novel", "-l", "lt", "-p", "", "-i", "" })]
        [InlineData(false, new string[] { "-n", "testName", "-a", "testAuthor", "-c", "novel", "-l", "lt", "-p", "sdfgsdfg", "-i", "" })]
        [InlineData(false, new string[] { "-n", "testName", "-a", "testAuthor", "-c", "novel", "-l", "lt", "-p", "2000-01-01", "-i", "aaa" })]
        [InlineData(true, new string[] { "-n", "testName", "-a", "testAuthor", "-c", "novel", "-l", "lt", "-p", "2000-01-01", "-i", "1111111111" })]
        [Theory]
        public void BookController_AddTest(bool expectedResult, string[] parts)
        {
            var bookRepoMock = new Mock<BookRepository>(getTestBooks());
            bookRepoMock.Setup(x => x.addBook(It.IsAny<Book>())).Returns(true);
            BookController controller = new BookController(bookRepoMock.Object);
            Assert.Equal(controller.Add(parts), expectedResult);
        }

        [InlineData(false, new string[0])]
        [InlineData(true, new string[] { "1" })]
        [InlineData(false, new string[] { "aaa" })]
        [Theory]
        public void BookController_DeleteTest(bool expectedResult, string[] parts)
        {
            var bookRepoMock = new Mock<BookRepository>(getTestBooks());
            bookRepoMock.Setup(x => x.deleteBook(It.IsAny<int>())).Returns(true);
            BookController controller = new BookController(bookRepoMock.Object);
            Assert.Equal(controller.Remove(parts), expectedResult);
        }

        [InlineData(false, new string[0])]
        [InlineData(false, new string[] { "1", "-n", "testUser", "-d", "2022-02-01" })]
        [InlineData(false, new string[] { "3", "-n", "Bob Smith", "-d", "2022-02-01" })]
        [InlineData(false, new string[] { "3", "-n", "Bob Smith", "-d", "2021-02-01" })]
        [InlineData(false, new string[] { "3", "-n", "Bob Smith", "-d", "2023-02-01" })]
        [InlineData(true, new string[] { "3", "-n", "testUser", "-d", "2022-02-01" })]
        [Theory]
        public void BookController_TakeTest(bool expectedResult, string[] parts)
        {
            var bookRepoMock = new Mock<BookRepository>(getTestBooks());
            bookRepoMock.Setup(x => x.takeBook(It.IsAny<Book>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(true);
            BookController controller = new BookController(bookRepoMock.Object);
            Assert.Equal(controller.Take(parts), expectedResult);
        }

        [InlineData(false, new string[0])]
        [InlineData(false, new string[] { "aaa" })]
        [InlineData(false, new string[] { "3" })]
        [InlineData(true, new string[] { "1" })]
        [Theory]
        public void BookController_ReturnTest(bool expectedResult, string[] parts)
        {
            var bookRepoMock = new Mock<BookRepository>(getTestBooks());
            bookRepoMock.Setup(x => x.returnBook(It.IsAny<Book>())).Returns(true);
            BookController controller = new BookController(bookRepoMock.Object);
            Assert.Equal(controller.Return(parts), expectedResult);
        }


        public List<Book> getTestBooks()
        {
            return new List<Book>()
            {
                new Book(){
                    id = 1, name = "testbook1", author = "test author", category = "Fantasy", language = "English", isbn = "1234567890",
                    publicationDate = new DateTime(2000,1,1), borrower = "Bob Smith", takenDate = new DateTime(2020,1,1), returnDate = new DateTime(2020,2,1)
                },
                new Book(){
                    id = 2, name = "testbook2", author = "tesst author", category = "History", language = "German", isbn = "9876543210",
                    publicationDate = new DateTime(1999,1,1), borrower = "Bob Smith", takenDate = new DateTime(2021,1,1), returnDate = new DateTime(2021,2,1)
                },
                new Book(){
                    id = 3, name = "testbook3", author = "tessst author", category = "Horror", language = "Russian", isbn = "9638527410",
                    publicationDate = new DateTime(1998,1,1), borrower = null, takenDate = null, returnDate = null
                },
                new Book(){
                    id = 4, name = "testbook1", author = "test author", category = "Science-Fiction", language = "English", isbn = "1234567890",
                    publicationDate = new DateTime(2000,1,1), borrower = "John Smith", takenDate = new DateTime(2020,1,1), returnDate = new DateTime(2020,2,1)
                },
                new Book(){
                    id = 5, name = "testbook2", author = "tesst author", category = "History", language = "German", isbn = "9876543210",
                    publicationDate = new DateTime(1999,1,1), borrower = null, takenDate = null, returnDate = null
                },
                new Book(){
                    id = 6, name = "testbook3", author = "tessst author", category = "Horror", language = "Russian", isbn = "9638527410",
                    publicationDate = new DateTime(1998,1,1), borrower = "Bob Smith", takenDate = new DateTime(2019,1,1), returnDate = new DateTime(2022,5,1)
                }
            };
        }
    }
}
