using TaxRadar_Application.Interfaces;
using TaxRadar_Application.Models;

namespace TaxRadar_Infrastructure.Services;

public class StaticTaxRatesProvider:ITaxRatesProvider
{
    private static readonly Dictionary<int, TaxYearRates> Rates = new()
    {
        [2025] = new TaxYearRates(2025, 2_000_000m, 2_536_500m,
            8_716m, 16_745m, 27_139m,
            1_000_000m, 1_500_000m, 2_000_000m),
        [2026] = new TaxYearRates(2026, 2_000_000m, 2_536_500m,
            9_162m, 16_745m, 27_139m,
            1_000_000m, 1_500_000m, 2_000_000m),
    };
    public Task<TaxYearRates> GetRatesForYearAsync(int year, CancellationToken ct)
    {
        if(!Rates.TryGetValue(year, out var rates)) throw new InvalidOperationException($"No tax rates configured for year {year}.");
        return Task.FromResult(rates);
    }
}