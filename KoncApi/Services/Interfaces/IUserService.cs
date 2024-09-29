using System;
using System.Collections.Generic;

namespace KoncApi
{
    public interface IUserService
    {
        List<UserReadDto> GetAllUsers();
        UserReadDto GetUserById(Guid id);
        void AddUser(UserCreateDto userCreateDto);
        void UpdateUser(Guid id, UserUpdateDto userUpdateDto);
        void DeleteUser(Guid id);
    }
}
