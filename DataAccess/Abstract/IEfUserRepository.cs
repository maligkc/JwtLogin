using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IEfUserRepository
    {
        public void Add(User user);
        public User GetByUserName(string username);
        public List<User> GetUsers();
    }
}
