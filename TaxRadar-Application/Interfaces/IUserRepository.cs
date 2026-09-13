using Tax_Radar_Domain.Entities;

namespace TaxRadar_Application.Interfaces;

public interface IUserRepository:IRepository<User>
{
    public Task<bool> CheckIfUserExistByEmail(string email, CancellationToken cancellationToken);
    public Task<User?> GetUserByEmail(string email, CancellationToken cancellationToken);
}