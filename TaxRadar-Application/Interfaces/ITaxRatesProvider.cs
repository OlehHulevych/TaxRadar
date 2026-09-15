using TaxRadar_Application.Models;

namespace TaxRadar_Application.Interfaces;

public interface ITaxRatesProvider
{
    Task<TaxYearRates> GetRatesForYearAsync(int year, CancellationToken ct);
}