using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService
    {
        void AddUser(string username, string email);

        User GetById(int id);

        List<User> GetAll();
    }
}
