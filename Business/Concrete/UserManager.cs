using Business.Abstract;
using DataAccess.Abstract;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : IAuthService
    {
        private readonly IEfUserRepository _userRepository;
        public UserManager(IEfUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            }
        }
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(computedHash);
            }
        }

        public void Register(string username, string password)
        {
            if(_userRepository.GetByUserName(username) != null)
            {
                throw new Exception("Kullanıcı zaten mevcut!");
            }
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                UserName = username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "User"
            };

            _userRepository.Add(user);
        }

        public User Login(string username, string password)
        {
            var user = _userRepository.GetByUserName(username);
            if (user == null)
                throw new Exception("Kullanıcı Bulunamadı");

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Şifre Yanlış!");

            return user;
        }


        List<User> IAuthService.GetUsers()
        {
            return _userRepository.GetUsers();
        }
    }
}
