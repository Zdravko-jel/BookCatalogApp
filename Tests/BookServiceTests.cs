using Data;
using Microsoft.EntityFrameworkCore;
using Services.Implementations;

namespace Tests;

public class BookServiceTests : IDisposable
{
    private readonly BookService _service;
    private readonly BookCatalogDBContext _context;

    public BookServiceTests()
    {
        var options = new DbContextOptionsBuilder<BookCatalogDBContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;


        _context = new BookCatalogDBContext(options);
        _service = new BookService(_context);
    }

    [Test]
    public void Add_ShouldAddBook()
    {
        var book = new Book { Title = "Test", Authors = "None", Genres = "None", Year = 2002 };

        _service.Add(book);

        Assert.AreEqual(1, _context.Books.Count());

        _context.Books.Remove(book);
    }

    [Test]
    public void Delete_ShouldRemoveBook()
    {
        var book = new Book { Title = "Test", Authors = "None", Genres = "None", Year = 2002 };
        _context.Books.Add(book);
        _context.SaveChanges();

        int cntBefore = _context.Books.Count();

        _service.Delete(book.Id);

        Assert.AreEqual(cntBefore - 1, _context.Books.Count());
    }

    [Test]
    public void GetById_ShouldReturnCorrectBook()
    {
        var book = new Book { Title = "Test", Authors = "None", Genres = "None", Year = 2002 };

        _context.Books.Add(book);
        _context.SaveChanges();

        var result = _service.GetById(book.Id);

        Assert.NotNull(result);
        Assert.AreEqual("Test", result.Title);
    }

    [Test]
    public void GetAll_ShouldReturnAllBooks()
    {
        _context.Books.Add(new Book { Title = "Book 1", Authors = "None", Genres = "None", Year = 2002 });
        _context.Books.Add(new Book { Title = "Book 2", Authors = "None", Genres = "None", Year = 2002 });
        _context.SaveChanges();

        var result = _service.GetAll();

        Assert.AreEqual(2, result.Count);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
