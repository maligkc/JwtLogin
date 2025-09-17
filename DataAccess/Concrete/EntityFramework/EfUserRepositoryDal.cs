using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserRepositoryDal : IEfUserRepository
    {
        private readonly AppDbContext _context;
        public EfUserRepositoryDal(AppDbContext context)
        {
            _context = context;
        }

        public User GetByUserName(string username)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == username);
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }
    }
}
