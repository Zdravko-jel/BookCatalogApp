using Data;
using Microsoft.EntityFrameworkCore;
using Services.Helpers;

namespace ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<BookCatalogDBContext>()
                .UseSqlite($"Data Source={DbPath.GetPath()}")
                .Options;

            var context = new BookCatalogDBContext(options);

            var conole = new ConsoleUI(context);

            conole.Run();   
        }
    }
}
