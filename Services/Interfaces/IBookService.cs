using Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBookService
    {
        List<Book> GetAll();
        Book GetById(int id);

        void Add(Book book);

        void Delete(int id);
        void Update(Book book);
    }
}
