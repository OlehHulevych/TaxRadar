using TaxRadar_Application.Models;

namespace TaxRadar_Application.Interfaces;

public interface ITaxRadarProvider
{
    Task<TaxYearRates> GetRatesForYearAsync(int year, CancellationToken ct);
}