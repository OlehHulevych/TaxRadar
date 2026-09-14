namespace TaxRadar_Application.Models;

public sealed record TaxYearRates(
    int Year,
    decimal VatThreshold,
    decimal VatImmediateThreshold,
    decimal PausalniDanBand1,
    decimal PausalniDanBand2,
    decimal PausalniDanBand3);
