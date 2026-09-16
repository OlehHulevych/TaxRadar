using Microsoft.EntityFrameworkCore;
using Tax_Radar_Domain.Entities;
using Tax_Radar_Domain.Enums;
using TaxRadar_Application.Exceptions;
using TaxRadar_Application.Interfaces;
using TaxRadar_Application.Queries.Invoices;
using TaxRadar_Infrastructure.Persistance;

namespace TaxRadar_Infrastructure.Repository;

public class InvoiceRepository(ApplicationDbContext context):IInvoiceRepository
{
    public async Task<Invoice?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var expense = await context.Invoices.Include(invoice=>invoice.Items).FirstOrDefaultAsync(c=>c.Id==id,cancellationToken);
        if (expense == null) throw new NotFoundException(nameof(Invoice), id);
        return expense;
    }

    public async Task AddAsync(Invoice entity, CancellationToken cancellationToken)
    {
        await context.Invoices.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Invoice entity, CancellationToken cancellationToken)
    {
        context.Invoices.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
    

    public async Task<List<Invoice>> GetByUserId(Guid id, CancellationToken cancellationToken)
    {
        var invoices = await context.Invoices.Include(i=>i.Items).Where(e=>e.UserId==id).ToListAsync(cancellationToken);
        return invoices;
    }

    public async Task AddItemToInvoice(Guid id, CreateInvoiceItemQuery query, CancellationToken cancellationToken)
    {
        var invoice = await context.Invoices.FirstOrDefaultAsync(invoice => invoice.Id == id, cancellationToken);
        if (invoice == null) throw new NotFoundException(nameof(Invoice));
        invoice.AddItem(query.Description,query.Quantity,query.UnitPrice,query.VatRatePercent);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveItemFromInvoice(Guid id, Guid itemId, CancellationToken cancellationToken)
    {
        var invoice = await context.Invoices.Include(invoice=>invoice.Items).FirstOrDefaultAsync(c=>c.Id==id,cancellationToken);
        if (invoice == null) throw new NotFoundException(nameof(Invoice), id);
        invoice.RemoveItem(itemId);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<decimal> GetTurnoverForPeriod(Guid userId, DateOnly fromDate, DateOnly toDate, CancellationToken ct)
    {
        var invoices = await context.Invoices.Include(i=>i.Items).Where(invoice =>
            invoice.UserId == userId && invoice.IssueDate >= fromDate && invoice.IssueDate <= toDate &&
            invoice.Status != InvoiceStatus.Draft && invoice.Status != InvoiceStatus.Cancelled).ToListAsync(ct);
        var turnoverForPeriod = invoices.Sum(i => i.GetTotalGrossAmount().Amount);
        return turnoverForPeriod;
    }
}