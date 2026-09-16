using Tax_Radar_Domain.Entities;
using TaxRadar_Application.Queries.Invoices;

namespace TaxRadar_Application.Interfaces;

public interface IInvoiceRepository:IRepository<Invoice>
{
    public Task<List<Invoice>> GetByUserId(Guid id, CancellationToken cancellationToken);
    public Task AddItemToInvoice(Guid id, CreateInvoiceItemQuery query, CancellationToken cancellationToken);
    public Task RemoveItemFromInvoice(Guid id, Guid itemId, CancellationToken cancellationToken);

    public Task<decimal> GetTurnoverForPeriod(Guid userId, DateOnly fromDate, DateOnly toDate, CancellationToken ct);
}