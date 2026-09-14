namespace TaxRadar_Application.Queries.TaxYearRates;

public record TaxYearRates(
    int Year,
    decimal VatThreshold,
    decimal VatImmediateThreshold,
    decimal PausalniDanBand1,
    decimal PausalniDanBand2,
    decimal PausalniDanBand3,
    decimal PausalDanIncomeLimit
    );