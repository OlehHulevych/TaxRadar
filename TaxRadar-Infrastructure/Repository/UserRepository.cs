using Microsoft.EntityFrameworkCore;
using Tax_Radar_Domain.Entities;
using Tax_Radar_Domain.ValueObjects;
using TaxRadar_Application.Exceptions;
using TaxRadar_Application.Interfaces;
using TaxRadar_Infrastructure.Persistance;

namespace TaxRadar_Infrastructure.Repository;

public class UserRepository(ApplicationDbContext context):IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(c=>c.Id==id,cancellationToken);
        if (user == null) throw new NotFoundException(nameof(User), id);
        return user;
    }

    public async Task AddAsync(User entity, CancellationToken cancellationToken)
    {
        await context.Users.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(User entity, CancellationToken cancellationToken)
    {
        context.Users.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync();
    }

    public async Task<bool> CheckIfUserExistByEmail(string email, CancellationToken cancellationToken)
    {
        var searchEmail = new EmailAddress(email);
        return await context.Users.AnyAsync(u => u.Email ==
                                                 searchEmail, cancellationToken);
    }

    public async Task<User?> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        var searchEmail = new EmailAddress(email);
        return await context.Users.FirstOrDefaultAsync(u => u.Email
                                                            == searchEmail, cancellationToken);
    }
}