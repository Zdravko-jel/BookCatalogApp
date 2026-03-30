using Data;
using Microsoft.EntityFrameworkCore;
using Services.Implementations;

namespace Tests;

public class UserServiceTests : IDisposable
{
    private readonly UserService _service;
    private readonly BookCatalogDBContext _context;

    public UserServiceTests()
    {
        var options = new DbContextOptionsBuilder<BookCatalogDBContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;


        _context = new BookCatalogDBContext(options);
        _service = new UserService(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Test]
    public void AddUser_ShouldWork()
    {
        _service.AddUser("test", "test@mail.com");

        var user = _service.GetByEmail("test@mail.com");

        Assert.AreEqual(1, _context.Users.Count());

        _context.Users.Remove(user);
    }

    [Test]
    public void GetAllUsers_ShouldReturnUsers()
    {
        _context.Users.Add(new User { Username = "User1", Email="test@email.com" });
        _context.Users.Add(new User { Username = "User2", Email = "test@email.com" });
        _context.SaveChanges();

        var result = _service.GetAll();

        Assert.AreEqual(2, result.Count);
    }
}
