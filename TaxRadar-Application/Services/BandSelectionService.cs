using Tax_Radar_Domain.Enums;
using Tax_Radar_Domain.Models;
using TaxRadar_Application.Models;

namespace TaxRadar_Application.Services;

public class BandSelectionService
{
    public static PausalniDanEligibility BandEvaluation(decimal income, TaxYearRates rates)
    {
        if (income <= rates.PausalniDanBand1IncomeLimit) return new PausalniDanEligibility(true, PausalniDanBand.Band1);
        if (income <= rates.PausalniDanBand2IncomeLimit) return new PausalniDanEligibility(true, PausalniDanBand.Band2);
        if (income <= rates.PausalniDanBand3IncomeLimit) return new PausalniDanEligibility(true, PausalniDanBand.Band3);

        return new PausalniDanEligibility(false, PausalniDanBand.NotEligible);
    }
}