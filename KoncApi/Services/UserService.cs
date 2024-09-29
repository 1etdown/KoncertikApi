using System;
using System.Collections.Generic;
using System.Linq;

namespace KoncApi
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<UserReadDto> GetAllUsers()
        {
            return _context.Users
                .Select(u => new UserReadDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Surname = u.Surname,
                    PhoneNumber = u.PhoneNumber,
                    Email = u.Email
                })
                .ToList();
        }

        public UserReadDto GetUserById(Guid id)
        {
            var user = _context.Users.Find(id);

            if (user == null) return null;

            return new UserReadDto
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
        }

        public void AddUser(UserCreateDto userCreateDto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = userCreateDto.Name,
                Surname = userCreateDto.Surname,
                PhoneNumber = userCreateDto.PhoneNumber,
                Email = userCreateDto.Email
            };

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(Guid id, UserUpdateDto userUpdateDto)
        {
            var user = _context.Users.Find(id);
            if (user == null) return;

            user.Name = userUpdateDto.Name;
            user.Surname = userUpdateDto.Surname;
            user.PhoneNumber = userUpdateDto.PhoneNumber;
            user.Email = userUpdateDto.Email;

            _context.SaveChanges();
        }

        public void DeleteUser(Guid id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return;

            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
