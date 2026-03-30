using Data;
using Microsoft.EntityFrameworkCore;
using Services.Implementations;

namespace Tests;

public class ReviewServiceTests : IDisposable
{
    private readonly ReviewService _service;
    private readonly BookCatalogDBContext _context;

    public ReviewServiceTests()
    {
        var options = new DbContextOptionsBuilder<BookCatalogDBContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;


        _context = new BookCatalogDBContext(options);
        _service = new ReviewService(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Test]
    public void AddReview_ShouldWork()
    {
        var book = new Book { Title = "Test", Authors = "None", Genres = "None", Year = 2002 };
        _context.Books.Add(book);
        _context.SaveChanges();

        _service.AddReview(book.Id, null, 5, "Great");

        Assert.AreEqual(1, _context.Reviews.Count());
    }

    [Test]
    public void AverageRating_ShouldBeCorrect()
    {
        var book = new Book { Title = "Test", Authors = "None", Genres = "None", Year = 2002 };
        _context.Books.Add(book);
        _context.SaveChanges();

        _context.Reviews.Add(new Review { BookId = book.Id, Rating = 4, UserId = null, Content="Good" });
        _context.Reviews.Add(new Review { BookId = book.Id, Rating = 2, UserId = null, Content = "Good" });
        _context.SaveChanges();

        var avg = _service.GetAvarageRating(book.Id);

        Assert.AreEqual(3, avg);
    }

    [Test]
    public void InvalidRating_ShouldThrow()
    {
        Assert.Throws<Exception>(() =>
            _service.AddReview(1, null, 10, "Bad"));
    }

    [Test]
    public void GetAverageRating_NoReviews_ShouldReturnZero()
    {
        var book = new Book { Title = "Test", Authors = "None", Genres = "None", Year = 2002 };
        _context.Books.Add(book);
        _context.SaveChanges();

        var result = _service.GetAvarageRating(book.Id);

        Assert.AreEqual(0, result);
    }

    [Test]
    public void GetAverageRating_MultipleReviews_ShouldReturnCorrectAverage()
    {
        var book = new Book { Title = "Test", Authors = "None", Genres = "None", Year = 2002 };
        _context.Books.Add(book);
        _context.SaveChanges();

        _context.Reviews.Add(new Review { BookId = book.Id, Rating = 5, UserId = null, Content = "Good" });
        _context.Reviews.Add(new Review { BookId = book.Id, Rating = 3, UserId = null, Content = "Good" });
        _context.Reviews.Add(new Review { BookId = book.Id, Rating = 1, UserId = null, Content = "Good" });
        _context.SaveChanges();

        var result = _service.GetAvarageRating(book.Id);

        Assert.AreEqual(3, result);
    }
}