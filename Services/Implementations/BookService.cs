using Data;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly BookCatalogDBContext _context;

        public BookService(BookCatalogDBContext context)
        {
            this._context = context;
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }

        public List<Book> GetAll()
        {
            return _context.Books.ToList();
        }

        public Book GetById(int id)
        {
            return _context.Books
                .Include(b => b.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefault(b => b.Id == id);
        }
    }
}
