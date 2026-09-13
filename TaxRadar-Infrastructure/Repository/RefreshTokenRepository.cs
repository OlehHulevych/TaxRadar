using Microsoft.EntityFrameworkCore;
using Tax_Radar_Domain.Entities;
using TaxRadar_Application.Exceptions;
using TaxRadar_Application.Interfaces;
using TaxRadar_Infrastructure.Persistance;

namespace TaxRadar_Infrastructure.Repository;

public class RefreshTokenRepository(ApplicationDbContext context):IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(c=>c.Id==id,cancellationToken);
        if (refreshToken == null) throw new NotFoundException(nameof(RefreshToken), id);
        return refreshToken;
    }

    public async Task AddAsync(RefreshToken entity, CancellationToken cancellationToken)
    {
        await context.RefreshTokens.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(RefreshToken entity, CancellationToken cancellationToken)
    {
        context.RefreshTokens.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<RefreshToken?> GetByUserId(Guid id, CancellationToken cancellationToken)
    {
        return await context.RefreshTokens.Where(e=>e.UserId==id).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        
    }

    public async Task<RefreshToken?> GetRefreshTokenByHash(string hash, CancellationToken cancellationToken)
    {
        return await context.RefreshTokens.Where(rt => rt.TokenHash.Equals(hash)).FirstOrDefaultAsync(cancellationToken);
    }
}