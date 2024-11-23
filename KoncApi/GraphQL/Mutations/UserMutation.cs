using Microsoft.EntityFrameworkCore;
using HotChocolate.Types;
namespace KoncApi;
[ExtendObjectType(name: "Mutation")]
public class UserMutation
{
    private readonly ApplicationDbContext _context;

    public UserMutation(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserReadDto?> CreateUserAsync(UserCreateDto newUserDto)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = newUserDto.Name,
            Surname = newUserDto.Surname,
            PhoneNumber = newUserDto.PhoneNumber,
            Email = newUserDto.Email
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return new UserReadDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email
        };
    }

    public async Task<UserReadDto?> UpdateUserAsync(Guid id, UserUpdateDto userUpdateDto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;

        user.Name = userUpdateDto.Name;
        user.Surname = userUpdateDto.Surname;
        user.PhoneNumber = userUpdateDto.PhoneNumber;
        user.Email = userUpdateDto.Email;

        await _context.SaveChangesAsync();

        return new UserReadDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email
        };
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
