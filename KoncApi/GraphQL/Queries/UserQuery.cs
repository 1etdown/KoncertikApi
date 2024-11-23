using Microsoft.EntityFrameworkCore;
namespace KoncApi;
using HotChocolate.Types;

[ExtendObjectType(name: "Query")]
public class UserQuery
{
    private readonly ApplicationDbContext _context;

    public UserQuery(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserReadDto>> GetAllUsersAsync()
    {
        return await _context.Users
            .Select(u => new UserReadDto
            {
                Id = u.Id,
                Name = u.Name,
                Surname = u.Surname,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email
            })
            .ToListAsync();
    }

    public async Task<UserReadDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
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
}
